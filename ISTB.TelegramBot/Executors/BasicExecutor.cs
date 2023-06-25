using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Storages.Command;
using ISTB.Framework.Executors.Storages.UserState;
using ISTB.Framework.Session;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using System.Text.Json;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors
{
    public class TestDialogModel
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
    }

    public sealed class BasicExecutor : Executor
    {
        private readonly ICommandStorage _commandStorage;
        private readonly IUserStateStorage _stateStorage;
        private readonly Session<TestDialogModel> _dialogSession;

        public BasicExecutor(ICommandStorage commandStorage, IUserStateStorage stateStorage, Session<TestDialogModel> dialogSession)
        {
            _commandStorage = commandStorage;
            _stateStorage = stateStorage;
            _dialogSession = dialogSession;
        }

        [TargetCommands("start")]
        public async Task StartCommand()
        {
            await Client.SendTextMessageAsync("Success");
        }

        [TargetCommands("dialog")]
        public async Task StartDialog()
        {
            await Client.SendTextMessageAsync("Simple dialog is started, input your name");
            await _stateStorage.SetAsync(nameof(SimpleDialogHandle));
        }

        [TargetUserState(nameof(SimpleDialogHandle))]
        [UpdateMessageTextNotNull]
        public async Task SimpleDialogHandle()
        {
            var model = await _dialogSession.GetAsync();
            model ??= new TestDialogModel();

            var text = UpdateContext.Message.Text!;

            if (model.Name == null)
            {
                model.Name = text;
                await _dialogSession.SetAsync(model);
                await Client.SendTextMessageAsync("Enter your age");
            }
            else if (model.Age == null)
            {
                model.Age = int.Parse(text);
                await _dialogSession.SetAsync(model);
                await Client.SendTextMessageAsync("Enter your gender");
            }
            else
            {
                model.Gender = text;
                
                await _stateStorage.RemoveAsync();
                await _dialogSession.RemoveAsync();

                await Client.SendTextMessageAsync(
                    JsonSerializer.Serialize(model)
                );
            }
        }
    }
}
