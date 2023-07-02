using ISTB.BusinessLogic.Services.Implementations;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Storages.Command;
using ISTB.Framework.Executors.Storages.UserState;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Context;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors
{
    public sealed class BasicExecutor : Executor
    {
        private readonly ICommandStorage _commandStorage;
        private readonly IUserStateStorage _userStateStorage;
        private readonly IUserService _userService;

        public BasicExecutor(ICommandStorage commandStorage, IUserStateStorage stateStorage, IUserService userService)
        {
            _commandStorage = commandStorage;
            _userStateStorage = stateStorage;
            _userService = userService;
        }

        [TargetCommands("start")]
        public async Task StartCommand()
        {
            await Client.SendTextMessageAsync("Success");
        }
    }
}
