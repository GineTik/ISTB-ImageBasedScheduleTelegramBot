using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.BotApplication.Delegates;
using ISTB.Framework.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ISTB.Framework.BotApplication
{
    public class BotApplication
    {
        private NextDelegate _firstMiddleware;

        private readonly ICollection<Func<NextDelegate, NextDelegate>> _middlewares;
        private readonly IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public BotApplication(BotApplicationBuilder builder)
        {
            _middlewares = new List<Func<NextDelegate, NextDelegate>>();
            _services = builder.Services;
            _configuration = builder.Configuration;

            UseMiddleware<UpdateContextMiddleware>();
        }

        public BotApplication Use(Func<UpdateContext, NextDelegate, Task> middlware)
        {
            ArgumentNullException.ThrowIfNull(middlware);

            _middlewares.Add(next =>
                async context => await middlware(context, next));

            return this;
        }

        public BotApplication UseMiddleware<T>()
            where T : class, IMiddleware
        {
            _services.AddTransient<T>();
            _middlewares.Add(next =>
                async context => await (_serviceProvider.GetService<T>()).InvokeAsync(context, next));

            return this;
        }

        public void Run(string? apiKey = null)
        {
            apiKey ??= _configuration["ApiKey"] ??
                throw new ArgumentNullException("ApiKey is null in appsettings.json and parameters");

            _serviceProvider = _services.BuildServiceProvider();

            NextDelegate firstMiddlware = context => Task.CompletedTask;

            foreach (var middlwareFactory in _middlewares.Reverse())
                firstMiddlware = middlwareFactory.Invoke(firstMiddlware);

            _firstMiddleware = firstMiddlware;

            var client = new TelegramBotClient(apiKey);
            client.StartReceiving(invokeMiddlewares, handlePollingErrorAsync);
        }

        private async Task invokeMiddlewares(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await _firstMiddleware.Invoke(new UpdateContext
            {
                Client = botClient,
                Update = update,
            });
        }

        private Task handlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}
