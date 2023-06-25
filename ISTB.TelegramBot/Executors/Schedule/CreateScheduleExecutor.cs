using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Storages.UserState;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.TelegramBot.Views.Schedule;
using Telegram.Bot.Types.Enums;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public sealed class CreateScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly IUserStateStorage _userStateStorage;

        public CreateScheduleExecutor(IScheduleService service, IUserStateStorage stateStorage)
        {
            _service = service;
            _userStateStorage = stateStorage;
        }

        [TargetCommands("create_schedule, cs")]
        [ParametersSeparator("")]
        public async Task Create(string? scheduleName)
        {
            if (scheduleName == null)
            {
                await _userStateStorage.SetAsync(nameof(TakeNewName));
                await ExecuteAsync<CreateScheduleView>(v => v.InputNewScheduleName());
            }
            else
            {
                await createWithName(scheduleName);
            }
        }

        [TargetUpdateType(UpdateType.Message, UserStates = nameof(TakeNewName))]
        [UpdateTextNotNull(ErrorMessage = "Ви маєте надіслати текст")]
        public async Task TakeNewName()
        {
            await _userStateStorage.RemoveAsync();
            await createWithName(UpdateContext.Update.Message!.Text!);
        }

        [TargetCallbacksDatas(nameof(CancelCreateSchedule), UserStates = nameof(TakeNewName))]
        public async Task CancelCreateSchedule()
        {
            await _userStateStorage.RemoveAsync();
            await ExecuteAsync<CreateScheduleView>(v => v.CancelCreate());
        }

        private async Task createWithName(string scheduleName)
        {
            var schedule = await _service.CreateAsync(new CreateScheduleDTO
            {
                Name = scheduleName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await ExecuteAsync<CreateScheduleView>(v => v.ScheduleCreated(schedule.Name));
        }
    }
}
