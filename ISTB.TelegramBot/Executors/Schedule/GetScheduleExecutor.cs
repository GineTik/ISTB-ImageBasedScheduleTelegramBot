using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using ISTB.TelegramBot.Enum.Buttons;

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
        public async Task GetMyCommand()
        {
            var buttons = await getMySchedulesAsButtonsAsync();

            var title = buttons.Count() switch
            {
                0 => "Ви ще не створили розкладу",
                _ => "Ваші розклади"
            };

            await Client.SendTextResponseAsync(
                title,
                replyMarkup: new InlineKeyboardMarkup(buttons)
            );
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.SelectSchedule))]
        public async Task SelectButton(int groupId)
        {
            var schedule = await _service.GetByIdAsync(groupId);
            await Client.AnswerCurrentCallbackQueryAsync();

            if (schedule == null)
            {
                await Client.DeleteButtonWithCallbacksDatas(
                    UpdateContext.Update.CallbackQuery.Message,
                    $"{nameof(ScheduleButtons.SelectSchedule)} {groupId}"
                );
            }
            else
            {
                await Client.EditMessageTextAsync(
                    UpdateContext.ChatId,
                    UpdateContext.Update.CallbackQuery.Message.MessageId,
                    "Назва групи: " + schedule.Name,
                    replyMarkup: new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithCallbackData("<<< Назад", nameof(ScheduleButtons.BackToMySchedules))
                    )
                );
            }
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.BackToMySchedules))]
        public async Task BackToMyButton()
        {
            var buttons = await getMySchedulesAsButtonsAsync();

            await Client.EditMessageTextAsync(
                UpdateContext.ChatId,
                UpdateContext.Update.CallbackQuery.Message.MessageId,
                "Ваші групи",
                replyMarkup: new InlineKeyboardMarkup(buttons)
            );
        }

        private async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> getMySchedulesAsButtonsAsync()
        {
            var schedules = await _service.GetListByTelegramUserIdAsync(UpdateContext.TelegramUserId);

            var buttons = schedules.Select(schedule => new[] { 
                InlineKeyboardButton.WithCallbackData(
                    schedule.Name,
                    $"{nameof(ScheduleButtons.SelectSchedule)} {schedule.Id}")
            });

            return buttons;
        }
    }
}
