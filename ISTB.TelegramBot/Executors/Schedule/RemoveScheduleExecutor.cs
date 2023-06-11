using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.MessagePresets.Extensions.AdvancedTelegramBotClient;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.MessagePresets.SchedulesMenu;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class RemoveScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly SchedulePresets _presets;

        public RemoveScheduleExecutor(IScheduleService service, SchedulePresets presets)
        {
            _service = service;
            _presets = presets;
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.RemoveSchedule))]
        public async Task ConfirmRemoveSchedule(int scheduleId, int messageId)
        {
            await Client.AnswerCallbackQueryAsync();
            await Client.DeleteCallbackQueryMessageAsync();

            await _service.RemoveByIdAsync(new RemoveScheduleByIdDTO()
            {
                Id = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            var preset = await _presets.GetSchedulesAsync();
            await Client.EditMessageResponseAsync(
                messageId,
                preset
            );
        }

        //[TargetCallbacksDatas(nameof(ScheduleButtons.RemoveSchedule))]
        //public async Task RemoveSchedule(int scheduleId)
        //{
        //    await Client.AnswerCallbackQueryAsync();

        //    var schedule = await _service.GetByIdAsync(new GetScheduleByIdDTO
        //    {
        //        Id = scheduleId,
        //        TelegramUserId = UpdateContext.TelegramUserId
        //    });

        //    if (schedule == null)
        //    {
        //        await Client.DeleteCurrentCallbackButtonAsync();
        //        return;
        //    }

        //    await Client.EditMessageTextAsync(
        //        UpdateContext.ChatId,
        //        UpdateContext.Update.CallbackQuery!.Message!.MessageId,
        //        "Ви впевнені?",
        //        replyMarkup: new InlineKeyboardMarkup(new[] {
        //            InlineKeyboardButton.WithCallbackData(
        //                "Так", $"{nameof(ScheduleButtons.ConfirmRemoveSchedule)} {schedule.Id}"),
        //            InlineKeyboardButton.WithCallbackData(
        //                "Ні", $"{nameof(ScheduleButtons.FailtureRemoveSchedule)} {schedule.Id}")
        //        })
        //    );
        //}

        //[TargetCallbacksDatas(nameof(ScheduleButtons.ConfirmRemoveSchedule))]
        //public async Task ConfirmRemoveSchedule(int groupId)
        //{
        //    await Client.AnswerCallbackQueryAsync();

        //    await _service.RemoveByIdAsync(new RemoveScheduleByIdDTO()
        //    {
        //        Id = groupId,
        //        TelegramUserId = UpdateContext.TelegramUserId
        //    });

        //    var preset = await _presets.GetSchedulesAsync();
        //    await Client.EditMessageResponseAsync(
        //        UpdateContext.Update.CallbackQuery!.Message!.MessageId,
        //        preset
        //    );
        //}

        //[TargetCallbacksDatas(nameof(ScheduleButtons.FailtureRemoveSchedule))]
        //public async Task FailtureRemoveSchedule(int scheduleId)
        //{
        //    await Client.AnswerCallbackQueryAsync();

        //    var preset = 
        //        await _presets.GetScheduleInfoAsync(scheduleId) ??
        //        await _presets.GetSchedulesAsync();

        //    await Client.EditMessageResponseAsync(
        //        UpdateContext.Update.CallbackQuery!.Message!.MessageId,
        //        preset
        //    );
        //}
    }
}
