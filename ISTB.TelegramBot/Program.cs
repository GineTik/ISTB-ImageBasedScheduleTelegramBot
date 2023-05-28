using ISTB.Framework.BotConfigurations;
using ISTB.Framework.Extensions.Middlewares;
using ISTB.Framework.Extensions.Services;

namespace ISTB.TelegramBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new BotApplicationBuilder();
            builder.Services.AddExecutors();

            var app = builder.Build();
            app.UseExecutorCommands();
            app.Run();

            Console.ReadLine();
        }
    }
}
