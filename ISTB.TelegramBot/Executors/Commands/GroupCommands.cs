using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.TelegramBot.Executors.Commands
{
    public class GroupCommands : Executor
    {
        private readonly IGroupService _service;

        public GroupCommands(IGroupService service)
        {
            _service = service;
        }

        [TargetCommands("create_group, cg")]
        public async Task CreateGroup(string groupName)
        {
            var group = await _service.CreateGroupAsync(new CreateGroupDTO
            {
                Name = groupName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await SendTextAsync("Створенна нова група з назвою: " + group.Name);
        }
        
        [TargetCommands("remove_group, rmg")]
        public async Task RemoveGroup(string groupName)
        {
            var group = await _service.GetGroupByNameAsync(new GetGroupByNameDTO
            {
                Name = groupName,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            if (group == null)
            {
                await UpdateContext.Client.SendTextMessageAsync(
                    UpdateContext.ChatId,
                    "Такої групи не існує або ви не є її власником"
                );
            }
            else
            {
                await UpdateContext.Client.SendTextMessageAsync(
                    UpdateContext.ChatId,
                    "Ви впевнені?",
                    replyMarkup: new InlineKeyboardMarkup(new[] {
                        InlineKeyboardButton.WithCallbackData("Так", "confirm_remove_group " + group.Id),
                        InlineKeyboardButton.WithCallbackData("Ні", "remove_callbackquery_message")
                    })
                );
            }
        }

        [TargetCallbacksDatas("confirm_remove_group")]
        public async Task ConfirmRemoveGroup(int groupId)
        {
            await _service.RemoveGroupByIdAsync(new RemoveGroupByIdDTO()
            {
                Id = groupId,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await RemoveCallbackQueryMessage();
            await SendTextAsync($"Група видалена");
        }

        [TargetCallbacksDatas("remove_callbackquery_message")]
        public async Task RemoveCallbackQueryMessage()
        {
            await UpdateContext.Client.AnswerCallbackQueryAsync(
                UpdateContext.Update.CallbackQuery.Id
            );

            await UpdateContext.Client.DeleteMessageAsync(
                UpdateContext.ChatId,
                UpdateContext.Update.CallbackQuery.Message.MessageId
            );
        }

        [TargetCommands("groups, g")]
        public async Task GetMyGroups()
        {
            var groups = await _service.GetGroupsByTelegramUserIdAsync(UpdateContext.TelegramUserId);

            if (groups.Count() == 0)
            {
                await SendTextAsync("Ви ще не створили групу");
            }
            else
            {
                var buttons = groups.Select(group => 
                    new[] { InlineKeyboardButton.WithCallbackData(group.Name, $"print_group {group.Id}") }
                );
                await UpdateContext.Client.SendTextMessageAsync(
                    UpdateContext.ChatId,
                    "Ваші групи",
                    replyMarkup: new InlineKeyboardMarkup(buttons)
                );
            }
        }

        [TargetCallbacksDatas("print_group")]
        public async Task PrintGroup(int groupId)
        {
            var group = await _service.GetGroupByIdAsync(groupId);

            await UpdateContext.Client.AnswerCallbackQueryAsync(
                UpdateContext.Update.CallbackQuery.Id
            );

            if (group == null)
            {
                var messageKeyboard = UpdateContext.Update.CallbackQuery.Message.ReplyMarkup.InlineKeyboard;
                var callbackData = UpdateContext.Update.CallbackQuery.Data;
                var messageId = UpdateContext.Update.CallbackQuery.Message.MessageId;

                var newKeyboard = messageKeyboard
                    .Select(row => row.SkipWhile(button => button.CallbackData == callbackData));

                await UpdateContext.Client.EditMessageReplyMarkupAsync(
                    UpdateContext.ChatId,
                    messageId,
                    replyMarkup: new InlineKeyboardMarkup(newKeyboard)
                );
            }
            else
            {
                await UpdateContext.Client.SendTextMessageAsync(
                    UpdateContext.ChatId,
                    group.Name
                );
            }
        }

        [TargetCommands("change_gname, chgn", Description = "Змінити назву групи")]
        public async Task ChangeName(string oldName, string newName)
        {
            await _service.ChangeGroupNameAsync(new ChangeGroupNameDTO
            {
                OldName = oldName,
                NewName = newName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await SendTextAsync("Ім'я групи змінено");
        }
    }
}
