using ISTB.BusinessLogic.DTOs.ScheduleDay;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Storages.UserState;
using ISTB.Framework.Session;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ISTB.TelegramBot.Executors.ScheduleDay
{
    public class EditDayExecutor : Executor
    {
        private readonly IUserStateStorage _userStateStorage;
        private readonly Session<int> _session;
        private readonly IScheduleDayService _dayService;

        public EditDayExecutor(IUserStateStorage userStateStorage, IScheduleDayService dayService, Session<int> session)
        {
            _userStateStorage = userStateStorage;
            _dayService = dayService;
            _session = session;
        }

        [TargetCallbacksDatas(nameof(EditImage))]
        public async Task EditImage(int dayId)
        {
            await Client.SendTextMessageAsync("Надішліть нове зображення");
            await _userStateStorage.SetAsync(nameof(EditImage));
            await _session.SetAsync(dayId);
        }

        [TargetCallbacksDatas(nameof(EditDescription))]
        public async Task EditDescription(int dayId)
        {
            await Client.SendTextMessageAsync("Надішліть новий опис");
            await _userStateStorage.SetAsync(nameof(EditDescription));
            await _session.SetAsync(dayId);
        }

        [TargetUpdateType(UpdateType.Message, UserStates = nameof(EditImage))]
        [UpdatePhotoNotNull(ErrorMessage = "Ви маєте надіслати зображення")]
        public async Task TakeImage()
        {
            var photo = UpdateContext.Update.Message!.Photo!.FirstOrDefault()!;
            
            await _dayService.EditPhotoAsync(new EditPhotoIdDTO
            {
                DayId = await _session.GetAndRemoveAsync(),
                PhotoId = photo.FileId
            });

            await Client.SendTextMessageAsync("Опис оновлено");
            await _userStateStorage.RemoveAsync();
        }

        [TargetUpdateType(UpdateType.Message, UserStates = nameof(EditDescription))]
        [UpdateTextNotNull(ErrorMessage = "Ви маєте надіслати текст")]
        public async Task TakeDescription()
        {
            var description = UpdateContext.Update.Message!.Text!;

            await _dayService.EditDescriptionAsync(new EditDescriptionDTO
            {
                DayId = await _session.GetAndRemoveAsync(),
                Description = description
            });

            await Client.SendTextMessageAsync("Опис оновлено");
            await _userStateStorage.RemoveAsync();
        }
    }
}
