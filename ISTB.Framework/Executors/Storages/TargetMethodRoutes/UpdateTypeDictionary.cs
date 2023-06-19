using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using System.Reflection;
using Telegram.Bot.Types.Enums;

namespace ISTB.Framework.Executors.Storages.TargetMethodRoutes
{
    public class UpdateTypeDictionary : Dictionary<UpdateType, UserStateMethodInfoDictionary>
    {
        public UpdateTypeDictionary()
        {
            var updateTypes = Enum.GetValues(typeof(UpdateType)).Cast<UpdateType>();

            foreach (var updateType in updateTypes)
            {
                Add(updateType, new UserStateMethodInfoDictionary());
            }
        }

        public void AddMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                AddMethod(method);
            }
        }

        public void AddMethod(MethodInfo method)
        {
            var targetAttributes = method
                .GetCustomAttributes<TargetAttribute>();

            foreach (var targetAttribute in targetAttributes)
            {
                var updateTypes = targetAttribute
                    .GetType()
                    .GetCustomAttributes<TargetUpdateTypeAttribute>()
                    .Select(attr => attr.UpdateType);

                if (updateTypes.Count() == 0)
                {
                    updateTypes = new List<UpdateType>() { UpdateType.Unknown };
                }

                foreach (var updateType in updateTypes)
                {
                    var userStateMethodInfo = this[updateType];
                    userStateMethodInfo.AddMethod(targetAttribute.UserState ?? "", method, targetAttribute);
                }
            }
        }
    }
}
