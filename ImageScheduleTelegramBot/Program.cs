using ISTB.Framework.BotConfigurations;
using ISTB.Framework.Extensions.Middlewares;
using ISTB.TelegramBot.Executors.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.TelegramBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new BotApplicationBuilder();
            builder.Services.AddTransient<StartCommand>();

            var app = builder.Build();
            app.UseCommand();
            app.Run();

            Console.ReadLine();
        }
    }
}
