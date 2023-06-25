using ISTB.Framework.Executors.Helpers.Exceptions;
using ISTB.Framework.Executors.Storages.UserState.Saver;

namespace ISTB.Framework.Executors.Configuration.Options
{
    public class UserStateOptions
    {
        public string DefaultUserState { get; set; }

        private Type _saverType;
        public Type SaverType
        {
            get
            {
                return _saverType;
            }
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<IUserStateSaver>(value);
                _saverType = value;
            }
        }
    }
}
