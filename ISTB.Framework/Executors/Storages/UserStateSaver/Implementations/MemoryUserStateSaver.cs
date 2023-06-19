using ISTB.Framework.Executors.Storages.UserStateSaver.Interfaces;

namespace ISTB.Framework.Executors.Storages.UserStateSaver.Implementations
{
    public class MemoryUserStateSaver : IUserStateSaver
    {
        private readonly Dictionary<long, string> _usersStates;

        public MemoryUserStateSaver()
        {
            _usersStates = new();
        }

        public async Task<string?> LoadAsync(long userId)
        {
            return await Task.Run(() =>
            {
                if (_usersStates.ContainsKey(userId) == false)
                    return null;

                return _usersStates[userId];
            });
        }

        public async Task RemoveAsync(long userId)
        {
            await Task.Run(() => _usersStates.Remove(userId));
        }

        public async Task SaveAsync(long userId, string state)
        {
            await Task.Run(() =>
            {
                if (_usersStates.ContainsKey(userId) == true)
                {
                    _usersStates[userId] = state!;
                }
                else
                {
                    _usersStates.Add(userId, state!);
                }
            });
        }
    }
}
