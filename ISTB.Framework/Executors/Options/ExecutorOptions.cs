using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Parsers.Implementations;
using ISTB.Framework.Executors.Storages.UserStateSaver.Implementations;

namespace ISTB.Framework.Executors.Options
{
    public class ExecutorOptions
    {
        public ParameterParserOptions ParameterParser { get; set; } = new ParameterParserOptions
        {
            DefaultSeparator = " ",
            ParserType = typeof(ExecutorParametersParser),
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
