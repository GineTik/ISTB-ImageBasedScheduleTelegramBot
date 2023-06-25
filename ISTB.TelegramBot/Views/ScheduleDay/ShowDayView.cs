using ISTB.BusinessLogic.DTOs.ScheduleDay;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Helpers.Builders;
using ISTB.TelegramBot.Executors.ScheduleDay;
using ISTB.TelegramBot.Executors.ScheduleWeek;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ISTB.TelegramBot.Views.ScheduleDay
{
    public class ShowDayView : Executor
    {
        public async Task ShowDay(ScheduleDayDTO day, int weekId)
        {
            await Client.EditMessageTextAsync(
                $"{day.Name}\n{day.Description}",
                replyMarkup: new InlineKeyboardBuilder()
                    .CallbackButton("Предпросмотр", $"{nameof(GetDayExecutor.ShowDayPreview)} {day.Id}").EndRow()
                    .CallbackButton("Змінити зображення", $"{nameof(EditDayExecutor.EditImage)} {day.Id}").EndRow()
                    .CallbackButton("Змінити опис", $"{nameof(EditDayExecutor.EditDescription)} {day.Id}").EndRow()
                    .CallbackButton("<<< Назад", $"{nameof(GetWeekExecutor.ShowWeek)} {weekId}").EndRow()
                    .Build()
            );
        }

        public async Task<Message> ShowDayPreview(ScheduleDayDTO day)
        {
            var text = day.Name + "\n" + day.Description;
            if (String.IsNullOrEmpty(day.ImageFileUrl))
            {
                return await Client.SendTextMessageAsync(
                    "[Немає зображення]\n" + text
                );
            }
            else
            {
                var media = new InputMediaPhoto(InputFile.FromFileId(day.ImageFileUrl));
                media.Caption = text;

                return (await Client.SendMediaGroupAsync(
                    UpdateContext.ChatId,
                    new[] { media }
                )).First();
            }
        }
    }
}
