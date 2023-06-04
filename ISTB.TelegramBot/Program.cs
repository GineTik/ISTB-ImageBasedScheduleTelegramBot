using ISTB.BusinessLogic.AutoMapper.Profiles;
using ISTB.Framework.BotApplication;
using ISTB.Framework.Extensions.Middlewares;
using ISTB.Framework.Extensions.Services;
using ISTB.TelegramBot.Extensions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.TelegramBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new BotApplicationBuilder();
            builder.Services.AddExecutors();
            builder.Services.AddData(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(GroupProfile));
            builder.Services.AddServices();

            var app = builder.Build();
            app.Use(async (updateContext, next) =>
            {
                try
                {
                    await next(updateContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            });
            app.UseExecutorCommands();
            app.Run();

            Console.ReadLine();
        }
    }
}
