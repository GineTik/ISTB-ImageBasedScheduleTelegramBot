using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Helpers.Factories.Interfaces;
using ISTB.Framework.Executors.Routing.Storages.UserState;
using ISTB.Framework.Session;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Builders;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.Enum.States;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class EditScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly IExecutorFactory _factory;
        private readonly Session<int> _session;
        private readonly IUserStateStorage _userState;

        public EditScheduleExecutor(IScheduleService service, IExecutorFactory factory, Session<int> session,
            IUserStateStorage userStateStorage)
        {
            _service = service;
            _factory = factory;
            _session = session;
            _userState = userStateStorage;
        }

        [TargetCommands("change_schedule_name", Description = "Змінити назву розкладу")]
        public async Task ChangeName()
        {
            var executor = _factory.CreateExecutor<GetScheduleExecutor>();
            await executor.SendSchedules(
                "Виберіть розклад, ім'я якого ви хочете змінити", 
                ScheduleButtons.SelectScheduleToChangeName
            );
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.SelectScheduleToChangeName))]
        public async Task SelectScheduleToChangeName(int scheduleId)
        {
            await Client.DeleteCallbackQueryMessageAsync();

            await _session.SetAsync(scheduleId);
            await _userState.SetAsync(nameof(UserStates.ChangeScheduleName));

            await Client.SendTextMessageAsync(
                "Введіть нову назву групи: ",
                replyMarkup: new InlineKeyboardBuilder()
                    .CallbackButton("Відмінити", nameof(ScheduleButtons.CancelChangeName))
                    .Build()
            );
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.CancelChangeName), UserState = nameof(UserStates.ChangeScheduleName))]
        public async Task CancelChange()
        {
            await _userState.RemoveAsync();

            await Client.EditMessageTextAsync(
                UpdateContext.ChatId,
                UpdateContext.MessageId,
                "Ви відмінили змінення назви розкладу",
                replyMarkup: null
            );
        }

        [TargetUpdateType(UpdateType.Message, UserState = nameof(UserStates.ChangeScheduleName))]
        public async Task TakeNewScheduleName()
        {
            await _userState.RemoveAsync();
            var schedulId = await _session.GetAndRemoveAsync();

            var newName = UpdateContext.Update.Message!.Text;
            if (newName == null)
            {
                await Client.SendTextMessageAsync("text field is empty");
                return;
            }

            await _service.ChangeNameAsync(new ChangeScheduleNameDTO
            {
                ScheduleId = schedulId,
                NewName = newName,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            await Client.SendTextMessageAsync($"Розклад змінено на {newName}");
        }
    }
}
