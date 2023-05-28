using ISTB.DataAccess.EF;
using ISTB.Framework.BotConfigurations;
using ISTB.Framework.Extensions.Middlewares;
using ISTB.Framework.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.TelegramBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new BotApplicationBuilder();
            builder.Services.AddExecutors();
            builder.Services.AddDbContext<DataContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));//("name=ConnectionStrings:LocalConnection"));


            var app = builder.Build();
            app.UseExecutorCommands();
            app.Run();

            Console.ReadLine();
        }
    }
}
