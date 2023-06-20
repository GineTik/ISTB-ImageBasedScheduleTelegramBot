using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using System.Reflection;
using Telegram.Bot.Types.Enums;

namespace ISTB.Framework.Executors.Routing.Storages.TargetMethod.RouteDictionaries
{
    public class UpdateTypeDictionary : Dictionary<UpdateType, UserStateMethodInfoDictionary>
    {
        private string _defaultUserState;

        public UpdateTypeDictionary(string defaultUserState)
        {
            _defaultUserState = defaultUserState;

            var updateTypes = getExistngUpdateTypes();

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
            var targetAttributes = method.GetCustomAttributes<TargetAttribute>();

            foreach (var targetAttribute in targetAttributes)
            {
                var updateTypes = getUpdateTypesOf(targetAttribute);

                if (updateTypes.Count() == 0)
                {
                    updateTypes = new List<UpdateType>() { UpdateType.Unknown };
                }

                foreach (var updateType in updateTypes)
                {
                    addMethodToUserState(method, targetAttribute, updateType);
                }
            }
        }

        private void addMethodToUserState(MethodInfo method, TargetAttribute targetAttribute, UpdateType updateType)
        {
            var userStateDictionary = this[updateType];
            var userState = targetAttribute.UserState ?? _defaultUserState;
            userStateDictionary.AddMethod(userState, method, targetAttribute);
        }

        private static IEnumerable<UpdateType> getExistngUpdateTypes()
        {
            return Enum.GetValues(typeof(UpdateType)).Cast<UpdateType>();
        }

        private static IEnumerable<UpdateType> getUpdateTypesOf(TargetAttribute targetAttribute)
        {
            return targetAttribute
                .GetType()
                .GetCustomAttributes<TargetUpdateTypeAttribute>()
                .Select(attr => attr.UpdateType);
        }
    }
}
