using ISTB.Framework.Executors.Exceptions;
using ISTB.Framework.Executors.Storages.UserStateSaver.Interfaces;

namespace ISTB.Framework.Executors.Options
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
