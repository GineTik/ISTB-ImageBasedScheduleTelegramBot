using ISTB.BusinessLogic.DTOs.ScheduleDay;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.Builders;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using ISTB.TelegramBot.Enum.Buttons;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors.ScheduleDay
{
    public class GetDayExecutor : Executor
    {
        private readonly IScheduleDayService _dayService;

        public GetDayExecutor(IScheduleDayService dayService)
        {
            _dayService = dayService;
        }

        [TargetCallbacksDatas(nameof(DayButtons.SelectScheduleDay))]
        public async Task GetDayInfo(int dayNumber, int weekId)
        {
            await Client.AnswerCallbackQueryAsync();
            var messageId = UpdateContext.Update.CallbackQuery!.Message!.MessageId;

            var day = await _dayService.GetByDayNumber(new GetByDayNumberDTO
            {
                DayNumber = dayNumber,
                WeekId = weekId
            });

            await Client.EditMessageTextAsync(
                UpdateContext.ChatId,
                messageId,
                $"{day.Name}\n{day.Description}",
                replyMarkup: new InlineKeyboardBuilder()
                    .CallbackButton("<<< Назад", $"{nameof(WeekButtons.SelectScheduleWeek)} {weekId}").EndRow()
                    .Build()
            );
        }
    }
}
