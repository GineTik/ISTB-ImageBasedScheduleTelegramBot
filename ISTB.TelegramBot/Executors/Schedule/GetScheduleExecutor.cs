using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.TelegramBot.Helpers;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class GetScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;

        public GetScheduleExecutor(IScheduleService service)
        {
            _service = service;
        }

        [TargetCommands("schedules")]
        public async Task GetMyScheduleCommand()
        {
            var buttons = await ScheduleHelper.GetMySchedulesAsButtonsAsync(_service, UpdateContext.TelegramUserId);

            var title = buttons.Count() switch
            {
                0 => "Ви ще не створили розклад",
                _ => "Ваші розклади"
            };

            await Client.SendTextResponseAsync(
                title,
                replyMarkup: new InlineKeyboardMarkup(buttons)
            );
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.SelectSchedule))]
        public async Task SelectScheduleButton(int scheduleId)
        {
            var schedule = await _service.GetByIdAsync(new GetScheduleByIdDTO
            {
                Id = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await Client.AnswerCurrentCallbackQueryAsync();

            if (schedule == null)
            {
                await Client.DeleteButtonWithCallbacksDatas(
                    UpdateContext.Update.CallbackQuery.Message,
                    $"{nameof(ScheduleButtons.SelectSchedule)} {scheduleId}"
                );
            }
            else
            {
                await ScheduleHelper.EditScheduleInfoMessageAsync(_service, UpdateContext, schedule.Id);                
            }
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.BackToMySchedules))]
        public async Task BackToMySchedulesButton()
        {
            await ScheduleHelper.EditYourSchedulesMessageAsync(_service, UpdateContext);
        }
    }
}
