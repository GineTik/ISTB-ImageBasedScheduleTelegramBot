using ISTB.Framework.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ISTB.Framework.BotConfigurations
{
    public class BotApplication
    {
        private readonly List<Type> _middlwaresType;
        private readonly IServiceCollection _services;
        private readonly string _apiKey;
        private IServiceProvider _serviceProvider;

        public BotApplication(string apiKey, IServiceCollection services)
        {
            _middlwaresType = new();
            _services = services;
            _apiKey = apiKey;
        }

        public BotApplication UseMiddleware<T>()
            where T : class, IMiddleware
        {
            _services.AddTransient<T>();
            _middlwaresType.Add(typeof(T));
            return this;
        }

        public void Run()
        {
            _serviceProvider = _services.BuildServiceProvider();
            var client = new TelegramBotClient(_apiKey);

            client.StartReceiving(invokeMiddlewares, handlePollingErrorAsync);
        }

        private async Task invokeMiddlewares(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            int nextMiddlewareIndex = 0;
            var next = configurateNext(botClient, update, nextMiddlewareIndex);
            await next.Invoke();
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

        private Func<Task> configurateNext(ITelegramBotClient botClient, Update update, int nextMiddlewareIndex)
        {
            return async () =>
            {
                if (_middlwaresType.Count <= nextMiddlewareIndex)
                    return;

                var middlawareType = _middlwaresType[nextMiddlewareIndex];
                var middleware = (IMiddleware)_serviceProvider.GetRequiredService(middlawareType);

                var next = configurateNext(botClient, update, ++nextMiddlewareIndex);
                await middleware.InvokeAsync(botClient, update, next);
            };
        }
    }
}
