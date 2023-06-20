using ISTB.Framework.Executors.Configuration.Options;
using ISTB.Framework.Executors.Routing.Storages.UserState.Saver;
using ISTB.Framework.TelegramBotApplication.Context;
using Microsoft.Extensions.Options;

namespace ISTB.Framework.Executors.Routing.Storages.UserState
{
    public class UserStateStorage : IUserStateStorage
    {
        private readonly IUserStateSaver _saver;
        private readonly UpdateContext _updateContext;
        private readonly UserStateOptions _options;

        public UserStateStorage(IUserStateSaver saver, UpdateContextAccessor accessor, IOptions<UserStateOptions> options)
        {
            _saver = saver;
            _updateContext = accessor.UpdateContext;
            _options = options.Value;
        }

        public async Task<string> GetAsync(long? telegramUserId = null)
        {
            telegramUserId ??= _updateContext.TelegramUserId;
            var state = await _saver.LoadAsync(telegramUserId.Value);
            return state ?? _options.DefaultUserState;
        }

        public async Task RemoveAsync(long? telegramUserId = null)
        {
            telegramUserId ??= _updateContext.TelegramUserId;
            await _saver.RemoveAsync(telegramUserId.Value);
        }

        public async Task SetAsync(string status, long? telegramUserId = null)
        {
            telegramUserId ??= _updateContext.TelegramUserId;
            await _saver.SaveAsync(telegramUserId.Value, status);
        }
    }
}
