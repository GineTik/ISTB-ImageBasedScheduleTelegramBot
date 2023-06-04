using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Parsers;

namespace ISTB.Framework.Attributes.ValidateInputDataAttributes
{
    public class RequireCorrectParametersAttribute : ValidateInputDataAttribute
    {
        public Type ParametersType { get; set; }

        public RequireCorrectParametersAttribute(Type parametersType)
        {
            ParametersType = parametersType;
        }

        public override async Task<bool> ValidateAsync(UpdateContext updateContext)
        {
            var parseResult =  CommandParametersParser.Parse(ParametersType, updateContext.Update.Message?.Text);

            ErrorMessage ??= $"Обов'язкова кількість параметрів: {parseResult.RequiredParametersCount}\n" +
                           $"Тип помилки: {parseResult.ErrorType}";

            return parseResult.IsSuccess;
        }
    }
}
