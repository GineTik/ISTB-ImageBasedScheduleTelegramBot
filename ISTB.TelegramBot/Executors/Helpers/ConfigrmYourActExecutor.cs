using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.TelegramBot.Views.Helpers;

namespace ISTB.TelegramBot.Executors.Helpers
{
    public class ConfigrmYourActExecutor : Executor
    {
        [TargetCallbacksDatas("confirm_act")]
        [ParametersSeparator(" | ")]
        public async Task ConfirmYourAct(string targetMethod, string data)
        {
            await Client.AnswerCallbackQueryAsync();
            await ExecuteAsync<ConfirmActView>(v => v.ShowConfirmAct(targetMethod, data));
        }
    }
}
