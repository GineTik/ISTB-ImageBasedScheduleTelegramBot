using ISTB.Framework.Attributes.BaseAttributes;
using System.Reflection;

namespace ISTB.Framework.Executors.Storages.TargetMethodRoutes
{
    public class UserStateMethodInfoDictionary : Dictionary<string, ICollection<TargetMethodInfo>>
    {
        public void AddMethod(string state, MethodInfo method, TargetAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(state);
            ArgumentNullException.ThrowIfNull(method);
            ArgumentNullException.ThrowIfNull(attribute);

            if (ContainsKey(state) == false)
            {
                Add(state, new List<TargetMethodInfo>());
            }

            var targetMethods = this[state];
            var targetMethod = targetMethods
                .FirstOrDefault(info => info.MethodInfo == method);

            if (targetMethod == null)
            {
                targetMethods.Add(new TargetMethodInfo
                {
                    MethodInfo = method,
                    TargetAttributes = new List<TargetAttribute> { attribute }
                });
            }
            else
            {
                targetMethod.TargetAttributes.Add(attribute);
            }
        }
    }
}
