using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class RemoveScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;

        public RemoveScheduleExecutor(IScheduleService service)
        {
            _service = service;
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.RemoveSchedule))]
        public async Task RemoveSchedule(int scheduleId)
        {
            await Client.AnswerCurrentCallbackQueryAsync();

            var schedule = await _service.GetByIdAsync(new GetScheduleByIdDTO
            {
                Id = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            if (schedule == null)
            {
                await Client.DeleteButtonWithCallbacksDatas(
                    UpdateContext.Update.CallbackQuery!.Message!,
                    $"{nameof(ScheduleButtons.RemoveSchedule)} {scheduleId}"
                );
                return;
            }
         
            await Client.EditMessageTextAsync(
                UpdateContext.ChatId,
                UpdateContext.Update.CallbackQuery!.Message!.MessageId,
                "Ви впевнені?",
                replyMarkup: new InlineKeyboardMarkup(new[] {
                    InlineKeyboardButton.WithCallbackData(
                        "Так", $"{nameof(ScheduleButtons.ConfirmRemoveSchedule)} {schedule.Id}"),
                    InlineKeyboardButton.WithCallbackData(
                        "Ні", $"{nameof(ScheduleButtons.FailtureRemoveSchedule)} {schedule.Id}")
                })
            );
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.ConfirmRemoveSchedule))]
        public async Task ConfirmRemoveSchedule(int groupId)
        {
            await Client.AnswerCurrentCallbackQueryAsync();

            await _service.RemoveByIdAsync(new RemoveScheduleByIdDTO()
            {
                Id = groupId,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            await ScheduleHelper.EditYourSchedulesMessageAsync(_service, UpdateContext);
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.FailtureRemoveSchedule))]
        public async Task FailtureRemoveSchedule(int scheduleId)
        {
            await Client.AnswerCurrentCallbackQueryAsync();

            var schedule = await _service.GetByIdAsync(new GetScheduleByIdDTO
            {
                Id = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            if (schedule == null)
            {
                await ScheduleHelper.EditYourSchedulesMessageAsync(_service, UpdateContext);
            }
            else
            {
                await ScheduleHelper.EditScheduleInfoMessageAsync(_service, UpdateContext, schedule.Id);
            }
        }
    }
}
