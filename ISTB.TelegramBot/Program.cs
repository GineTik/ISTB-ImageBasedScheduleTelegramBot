using ISTB.Framework.BotConfigurations;
using ISTB.Framework.Extensions.Middlewares;
using ISTB.Framework.Extensions.Services;
using ISTB.TelegramBot.Extensions.Services;
using Telegram.Bot;

namespace ISTB.TelegramBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new BotApplicationBuilder();
            builder.Services.AddExecutors();
            builder.Services.AddData(builder.Configuration);

            var app = builder.Build();
            app.UseExecutorCommands();
            app.Run();

            Console.ReadLine();
        }
    }
}
