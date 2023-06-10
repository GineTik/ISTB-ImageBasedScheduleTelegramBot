using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ISTB.Framework.TelegramBotApplication.Extensions.ReceiverOption
{
    public static class ReceiverOptionsExtensio
    {
        public static ReceiverOptions ConfigureAllowedUpdates(this ReceiverOptions options, params UpdateType[] allowedUpdates)
        {
            ArgumentNullException.ThrowIfNull(allowedUpdates);
            options.AllowedUpdates = allowedUpdates;
            return options;
        }

        public static ReceiverOptions ConfigureReceiverOptions(this ReceiverOptions options, Action<ReceiverOptions> configure)
        {
            configure(options);
            return options;
        }
    }
}
