using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Helpers.Builders;
using ISTB.TelegramBot.Executors.Schedule;
using Telegram.Bot;

namespace ISTB.TelegramBot.Views.Schedule
{
    public class EditScheduleView : Executor
    {
        public async Task ChooseScheduleToEdit(IEnumerable<ScheduleDTO> schedules)
        {
            await ExecuteAsync<ShowScheduleView>(v => v.ShowMySchedules(
                schedules,
                (s, _) => $"{nameof(EditScheduleExecutor.TakeChoseSchedule)} {s.Id}",
                hasUndoButton: true
            ));
        }

        public async Task InputNewName()
        {
            await Client.SendTextMessageAsync(
                "Введіть нову назву групи: ",
                replyMarkup: new InlineKeyboardBuilder()
                    .CallbackButton("Відмінити", nameof(EditScheduleExecutor.CancelEditName))
                    .Build()
            );
        }

        public async Task CancelEditName()
        {
            await Client.EditMessageTextAsync(
                UpdateContext.ChatId,
                UpdateContext.MessageId,
                "Ви відмінили змінення назви розкладу",
                replyMarkup: null
            );
        }

        public async Task ScheduleNameEdited(string name)
        {
            await Client.SendTextMessageAsync($"Розклад змінено на {name}");
        }
    }
}
