using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors.Group
{
    public class GetGroupExecutor : Executor
    {
        private readonly IGroupService _service;

        public GetGroupExecutor(IGroupService service)
        {
            _service = service;
        }

        [TargetCommands("groups")]
        public async Task GetMyGroupsCommand()
        {
            var buttons = await getMyGroupsAsButtonAsync();

            var title = buttons.Count() switch
            {
                0 => "Ви ще не створили груп",
                _ => "Ваші групи"
            };

            await Client.SendTextResponseAsync(
                title,
                replyMarkup: new InlineKeyboardMarkup(buttons)
            );
        }

        [TargetCallbacksDatas("select_group")]
        public async Task SelectGroupButton(int groupId)
        {
            var group = await _service.GetGroupByIdAsync(groupId);
            await Client.AnswerCurrentCallbackQueryAsync();

            if (group == null)
            {
                await Client.DeleteButtonWithCallbacksDatas(
                    UpdateContext.Update.CallbackQuery.Message,
                    "select_group " + groupId
                );
            }
            else
            {
                await Client.EditMessageTextAsync(
                    UpdateContext.ChatId,
                    UpdateContext.Update.CallbackQuery.Message.MessageId,
                    "Назва групи: " + group.Name,
                    replyMarkup: new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithCallbackData("<<< Назад", "back_to_my_groups")
                    )
                );
            }
        }

        [TargetCallbacksDatas("back_to_my_groups")]
        public async Task BackToMyGroupsButton()
        {
            var buttons = await getMyGroupsAsButtonAsync();

            await Client.EditMessageTextAsync(
                UpdateContext.ChatId,
                UpdateContext.Update.CallbackQuery.Message.MessageId,
                "Ваші групи",
                replyMarkup: new InlineKeyboardMarkup(buttons)
            );
        }

        private async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> getMyGroupsAsButtonAsync()
        {
            var groups = await _service.GetGroupsByTelegramUserIdAsync(UpdateContext.TelegramUserId);

            var buttons = groups.Select(group =>
                new[] { InlineKeyboardButton.WithCallbackData(group.Name, $"select_group {group.Id}") }
            );

            return buttons;
        }
    }
}
