using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.TelegramBot.Executors.Group
{
    public class RemoveGroupExecutor : Executor
    {
        private readonly IGroupService _service;

        public RemoveGroupExecutor(IGroupService service)
        {
            _service = service;
        }

        [TargetCommands("remove_group, rmg")]
        public async Task RemoveGroupCommand(string groupName)
        {
            var group = await _service.GetGroupByNameAsync(new GetGroupByNameDTO
            {
                Name = groupName,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            if (group == null)
            {
                await UpdateContext.Client.SendTextResponseAsync("Такої групи не існує або ви не є її власником");
            }
            else
            {
                await UpdateContext.Client.SendTextResponseAsync(
                    "Ви впевнені?",
                    replyMarkup: new InlineKeyboardMarkup(new[] {
                        InlineKeyboardButton.WithCallbackData("Так", "confirm_remove_group " + group.Id),
                        InlineKeyboardButton.WithCallbackData("Ні", "remove_callbackquery_message")
                    })
                );
            }
        }

        [TargetCallbacksDatas("confirm_remove_group")]
        public async Task ConfirmRemoveGroupButton(int groupId)
        {
            await _service.RemoveGroupByIdAsync(new RemoveGroupByIdDTO()
            {
                Id = groupId,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await UpdateContext.Client.DeleteCallbackQueryMessage();
            await UpdateContext.Client.SendTextResponseAsync($"Група видалена");
        }

        [TargetCallbacksDatas("remove_callbackquery_message")]
        public async Task RemoveCallbackQueryMessageButton()
        {
            await UpdateContext.Client.DeleteCallbackQueryMessage();
        }
    }
}
