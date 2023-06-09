using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.TelegramBot.Enum.Buttons;
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

        [TargetCommands("remove_schedule, rms")]
        public async Task RemoveGroupCommand(string scheduleName)
        {
            var schedule = await _service.GetByNameAsync(new GetScheduleByNameDTO
            {
                Name = scheduleName,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            if (schedule == null)
            {
                await UpdateContext.Client.SendTextResponseAsync("Такого розкладу не існує або ви не є її власником");
            }
            else
            {
                await UpdateContext.Client.SendTextResponseAsync(
                    "Ви впевнені?",
                    replyMarkup: new InlineKeyboardMarkup(new[] {
                        InlineKeyboardButton.WithCallbackData("Так", $"{nameof(ScheduleButtons.ConfirmRemoveSchedule)} {schedule.Id}"),
                        InlineKeyboardButton.WithCallbackData("Ні", nameof(ScheduleButtons.RemoveCallbackQueryMessage))
                    })
                );
            }
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.ConfirmRemoveSchedule))]
        public async Task ConfirmRemoveGroupButton(int groupId)
        {
            await _service.RemoveByIdAsync(new RemoveScheduleByIdDTO()
            {
                Id = groupId,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await UpdateContext.Client.DeleteCallbackQueryMessage();
            await UpdateContext.Client.SendTextResponseAsync($"Група видалена");
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.RemoveCallbackQueryMessage))]
        public async Task RemoveCallbackQueryMessageButton()
        {
            await UpdateContext.Client.DeleteCallbackQueryMessage();
        }
    }
}
