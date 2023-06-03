using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Parsers;
using Telegram.Bot;

namespace ISTB.Framework.Attributes.ValidateInputDataAttributes
{
    public class ValidateParametersAttribute : ValidateInputDataAttribute
    {
        public Type ParametersType { get; set; }
        public Func<UpdateContext, ParseResult, Task>? ErrorCallbackAsync { get; set; }

        public ValidateParametersAttribute(Type parametersType)
        {
            ParametersType = parametersType;
        }

        public override async Task<bool> ValidateAsync(UpdateContext updateContext)
        {
            var parseResult = CommandParametersParser.Parse(ParametersType, updateContext.Update.Message?.Text);

            if (parseResult.IsSuccess == false)
                await (ErrorCallbackAsync ?? SendErrorResponseAsync).Invoke(updateContext, parseResult);

            return parseResult.IsSuccess;
        }

        public async Task SendErrorResponseAsync(UpdateContext updateContext, ParseResult result)
        {
            await updateContext.Client.SendTextMessageAsync(
                updateContext.ChatId,
                $"Обов'язкова кількість параметрів: {result.RequiredParametersCount}\nТип помилки: {result.ErrorType}");
        }
    }
}
