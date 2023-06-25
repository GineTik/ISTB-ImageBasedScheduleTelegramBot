using ISTB.DataAccess.Entities;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Storages.UserState;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Helpers.Builders;
using ISTB.TelegramBot.Executors.Schedule;
using Telegram.Bot;

namespace ISTB.TelegramBot.Views.Schedule
{
    public class CreateScheduleView : Executor
    {
        public async Task InputNewScheduleName()
        {
            await Client.SendTextMessageAsync(
                "Ведіть назву розкладу",
                replyMarkup: new InlineKeyboardBuilder()
                    .CallbackButton("Я передумав", nameof(CreateScheduleExecutor.CancelCreateSchedule))
                    .Build()
            );
        }

        public async Task CancelCreate()
        {
            await Client.EditMessageTextAsync(
                UpdateContext.ChatId,
                UpdateContext.MessageId,
                "Ви передумали",
                replyMarkup: null
            );
        }

        public async Task ScheduleCreated(string name)
        {
            await Client.SendTextMessageAsync("Створенна нова група з назвою: " + name);
        }
    }
}
