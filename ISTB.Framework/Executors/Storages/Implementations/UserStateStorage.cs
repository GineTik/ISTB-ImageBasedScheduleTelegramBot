using ISTB.Framework.Executors.Options;
using ISTB.Framework.Executors.Storages.Interfaces;
using ISTB.Framework.Executors.Storages.UserStateSaver.Interfaces;
using ISTB.Framework.TelegramBotApplication.Context;
using Microsoft.Extensions.Options;

namespace ISTB.Framework.Executors.Storages.Implementations
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

        public async Task SetAsync(string status)
        {
            await _saver.SaveAsync(_updateContext.TelegramUserId, status);
        }

        public async Task SetAsync(long telegramUserId, string status)
        {
            await _saver.SaveAsync(telegramUserId, status);
        }
    }
}
