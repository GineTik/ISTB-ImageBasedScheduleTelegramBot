using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using Telegram.Bot.Types.Enums;
using ISTB.TelegramBot.Enum.States;
using ISTB.Framework.Executors.Storages.UserStateSaver.Interfaces;
using ISTB.Framework.TelegramBotApplication.Builders;
using Telegram.Bot;
using ISTB.TelegramBot.Enum.Buttons;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class CreateScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly IUserStateSaver _userStateSaver;

        public CreateScheduleExecutor(IScheduleService service, IUserStateSaver userStateSaver)
        {
            _service = service;
            _userStateSaver = userStateSaver;
        }

        [TargetCommands("create_schedule, cs")]
        [ParametersSeparator("")]
        public async Task Create(string? scheduleName)
        {
            if (scheduleName == null)
            {
                await _userStateSaver.SaveAsync(UpdateContext.TelegramUserId, nameof(UserStates.CreateSchedule));
                await Client.SendTextMessageAsync(
                    "Ведіть назву розкладу", 
                    replyMarkup: new InlineKeyboardBuilder()
                        .CallbackButton("Я передумав", nameof(ScheduleButtons.CancelCreateSchedule))
                        .Build()
                );
            }
            else
            {
                await CreateByName(scheduleName);
            }
        }

        [TargetUpdateType(UpdateType.Message, UserState = nameof(UserStates.CreateSchedule))]
        public async Task CreateByNameTarget()
        {
            await _userStateSaver.RemoveAsync(UpdateContext.TelegramUserId);
            await CreateByName(UpdateContext.Update.Message!.Text!);
        }

        public async Task CreateByName(string scheduleName)
        {
            var schedule = await _service.CreateAsync(new CreateScheduleDTO
            {
                Name = scheduleName,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            await Client.SendTextMessageAsync("Створенна нова група з назвою: " + schedule.Name);
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.CancelCreateSchedule), UserState = nameof(UserStates.CreateSchedule))]
        public async Task CancelCreateSchedule()
        {
            await _userStateSaver.RemoveAsync(UpdateContext.TelegramUserId);

            await Client.EditMessageTextAsync(
                UpdateContext.ChatId,
                UpdateContext.MessageId,
                "Ви передумали",
                replyMarkup: null
            );
        }
    }
}
