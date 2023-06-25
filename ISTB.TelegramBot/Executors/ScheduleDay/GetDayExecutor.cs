using ISTB.BusinessLogic.DTOs.ScheduleDay;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Helpers.Factories.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.TelegramBot.Executors.Helpers;
using ISTB.TelegramBot.Views.Helpers;
using ISTB.TelegramBot.Views.ScheduleDay;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ISTB.TelegramBot.Executors.ScheduleDay
{
    public class GetDayExecutor : Executor
    {
        private readonly IScheduleDayService _dayService;
        private readonly IExecutorFactory _factory;

        public GetDayExecutor(IScheduleDayService dayService, IExecutorFactory factory)
        {
            _dayService = dayService;
            _factory = factory;
        }

        [TargetCallbacksDatas(nameof(ShowDay))]
        public async Task ShowDay(int dayNumber, int weekId)
        {
            await Client.AnswerCallbackQueryAsync();

            var day = await _dayService.GetByDayNumberAsync(new GetByDayNumberDTO
            {
                DayNumber = dayNumber,
                WeekId = weekId
            });

            await ExecuteAsync<ShowDayView>(v => v.ShowDay(day, weekId));
        }

        [TargetCallbacksDatas(nameof(ShowDayPreview))]
        public async Task ShowDayPreview(int dayId)
        {
            await Client.AnswerCallbackQueryAsync();
            
            var day = await _dayService.GetByIdAsync(dayId);

            var message = await ExecuteAsync<ShowDayView, Task<Message>>(v => v.ShowDayPreview(day));
            await ExecuteAsync<ConfirmActView>(e => e.ShowConfirmAct(
                nameof(DeleteMessageExecutor.DeleteMessage), 
                message.MessageId.ToString()
            ));
        }

        [TargetCommands("today")]
        [ParseErrorMessages(ArgsLengthIsLess = "Ви маємте ввести код розкладу який вам надасть власник розкладу")]
        public async Task GetToday(int scheduleCode)
        {
            var day = await _dayService.GetTodayByScheduleIdAsync(scheduleCode);
            await ExecuteAsync<ShowDayView>(v => v.ShowDayPreview(day));
        }
    }
}
