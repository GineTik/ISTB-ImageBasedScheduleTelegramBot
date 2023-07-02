using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Storages.UserState;
using ISTB.Framework.Session;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.TelegramBot.Views.Schedule;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class EditScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly ISession<int> _session;
        private readonly IUserStateStorage _userState;

        public EditScheduleExecutor(IScheduleService service, ISession<int> session,
            IUserStateStorage userStateStorage)
        {
            _service = service;
            _session = session;
            _userState = userStateStorage;
        }

        [TargetCommands("change_schedule_name", Description = "Змінити назву розкладу")]
        public async Task EditName()
        {
            var schedules = await _service.GetListByTelegramUserIdAsync(UpdateContext.TelegramUserId);
            await ExecuteAsync<EditScheduleView>(v => v.ChooseScheduleToEdit(schedules));
        }

        [TargetCallbacksDatas(nameof(TakeChoseSchedule))]
        public async Task TakeChoseSchedule(int scheduleId)
        {
            await Client.DeleteMessageAsync();

            await _session.SetAsync(scheduleId);
            await _userState.SetAsync(nameof(TakeNewScheduleName));

            await ExecuteAsync<EditScheduleView>(v => v.InputNewName());
        }

        [TargetCallbacksDatas(nameof(CancelEditName), UserStates = nameof(TakeNewScheduleName))]
        public async Task CancelEditName()
        {
            await _userState.RemoveAsync();
            await ExecuteAsync<EditScheduleView>(v => v.CancelEditName());
        }

        [TargetUpdateType(UpdateType.Message, UserStates = nameof(TakeNewScheduleName))]
        [UpdateMessageTextNotNull(ErrorMessage = "Назва має бути у виді тексту")]
        public async Task TakeNewScheduleName()
        {
            await _userState.RemoveAsync();
            var schedulId = await _session.GetAndRemoveAsync();
            var newName = UpdateContext.Update.Message!.Text!;

            await _service.ChangeNameAsync(new ChangeScheduleNameDTO
            {
                ScheduleId = schedulId,
                NewName = newName,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            await ExecuteAsync<EditScheduleView>(v => v.ScheduleNameEdited(newName));
        }
    }
}
