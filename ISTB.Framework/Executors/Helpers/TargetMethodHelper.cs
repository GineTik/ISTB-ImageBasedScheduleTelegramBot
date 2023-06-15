using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors.Storages.Implementations;
using System.Reflection;
using Telegram.Bot.Types.Enums;

namespace ISTB.Framework.Executors.Helpers
{
    public static class TargetMethodHelper
    {
        public static Dictionary<UpdateType, ICollection<TargetMethodInfo>> MethodsToMethodRoutes(IEnumerable<MethodInfo> methodInfos)
        {
            var updateTypes = Enum.GetValues(typeof(UpdateType)).Cast<UpdateType>();
            var dictionary = updateTypes.ToDictionary<UpdateType, UpdateType, ICollection<TargetMethodInfo>>(
                updateType => updateType,
                updateType => new List<TargetMethodInfo>()
            );

            foreach (var methodInfo in methodInfos)
            {
                var targetAttributes = methodInfo
                    .GetCustomAttributes<TargetAttribute>();

                foreach (var targetAttribute in targetAttributes)
                {
                    var updateTypeAttributes = targetAttribute
                        .GetType()
                        .GetCustomAttributes<TargetUpdateTypeAttribute>()
                        .ToList();

                    if (updateTypeAttributes.Count() == 0)
                    {
                        updateTypeAttributes.Add(new TargetUpdateTypeAttribute(UpdateType.Unknown));
                    }

                    foreach (var updateTypeAttribute in updateTypeAttributes)
                    {
                        var targetMethodInfo = dictionary[updateTypeAttribute.UpdateType]
                            .FirstOrDefault(info => info.MethodInfo == methodInfo);

                        if (targetMethodInfo == null)
                        {
                            dictionary[updateTypeAttribute.UpdateType].Add(new TargetMethodInfo
                            {
                                MethodInfo = methodInfo,
                                TargetAttributes = new List<TargetAttribute> { targetAttribute }
                            });
                        }
                        else
                        {
                            targetMethodInfo.TargetAttributes.Add(targetAttribute);
                        }
                    }
                }
            }

            return dictionary;
        }
    }
}
