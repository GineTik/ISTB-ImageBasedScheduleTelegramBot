using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Parsers.ExecutorParameters;
using ISTB.Framework.Executors.Storages.UserState.Saver.Implementations;

namespace ISTB.Framework.Executors.Configuration.Options
{
    public class ExecutorOptions
    {
        public ParameterParserOptions ParameterParser { get; set; } = new ParameterParserOptions
        {
            DefaultSeparator = " ",
            ParserType = typeof(ParametersParser),
            ErrorMessages = new ParseErrorMessagesAttribute()
            {
                TypeParseError = "Parse type error",
                ArgsLengthIsLess = "Args length is less"
            }
        };

        public UserStateOptions UserState { get; set; } = new UserStateOptions
        {
            DefaultUserState = "",
            SaverType = typeof(MemoryUserStateSaver),
        };
    }
}
