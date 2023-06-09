﻿using ISTB.Framework.TelegramBotApplication.AdvancedBotClient;
using ISTB.Framework.TelegramBotApplication.Configuration.Middlewares;
using ISTB.Framework.TelegramBotApplication.Configuration.Middlewares.UpdateContexts;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.Framework.TelegramBotApplication.Delegates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ISTB.Framework.TelegramBotApplication
{
    public class BotApplication
    {
        private NextDelegate _firstMiddleware;

        private readonly ICollection<Func<NextDelegate, NextDelegate>> _middlewares;
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;
        private readonly ReceiverOptions _receiverOptions;
        private IServiceProvider _serviceProvider;
        private UpdateContext _currentUpdateContext;
        private string _apiKey;

        public BotApplication(BotApplicationBuilder builder)
        {
            _middlewares = new List<Func<NextDelegate, NextDelegate>>();
            _services = builder.Services;
            _configuration = builder.Configuration;
            _receiverOptions = builder.ReceiverOptions;

            UseMiddleware<UpdateContextMiddleware>();
        }

        public BotApplication Use(Func<UpdateContext, NextDelegate, Task> middlware)
        {
            ArgumentNullException.ThrowIfNull(middlware);

            _middlewares.Add(next =>
                async () => await middlware(_currentUpdateContext, next));

            return this;
        }

        public BotApplication Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware)
        {
            ArgumentNullException.ThrowIfNull(middlware);

            _middlewares.Add(next =>
                async () => await middlware(_serviceProvider, _currentUpdateContext, next));

            return this;
        }

        public BotApplication UseMiddleware<T>()
            where T : class, IMiddleware
        {
            _services.AddTransient<T>();
            _middlewares.Add(next =>
                async () => await (_serviceProvider.GetRequiredService<T>()).InvokeAsync(_currentUpdateContext, next));

            return this;
        }

        public void Run(string? apiKey = null)
        {
            _apiKey = apiKey ?? _configuration["ApiKey"] ??
                throw new ArgumentNullException("ApiKey is null in appsettings.json and parameters");
            _serviceProvider = _services.BuildServiceProvider();

            _firstMiddleware = () => Task.CompletedTask;
            foreach (var middlwareFactory in _middlewares.Reverse())
                _firstMiddleware = middlwareFactory.Invoke(_firstMiddleware);

            var client = new TelegramBotClient(_apiKey);
            client.StartReceiving(invokeMiddlewares, handlePollingErrorAsync, _receiverOptions);
        }

        private async Task invokeMiddlewares(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _currentUpdateContext = new UpdateContext();
            _currentUpdateContext.Update = update;
            _currentUpdateContext.CancellationToken = cancellationToken;
            _currentUpdateContext.Client = new AdvancedTelegramBotClient(_apiKey, _currentUpdateContext);

            await _firstMiddleware.Invoke();
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
