﻿using ISTB.Framework.Delegates;
using ISTB.Framework.Executors.Context;
using ISTB.Framework.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ISTB.Framework.BotConfigurations
{
    public class BotApplication
    {
        private readonly ICollection<Func<UpdateDelegate, UpdateDelegate>> _middlewares;
        private readonly IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public BotApplication(BotApplicationBuilder builder)
        {
            _middlewares = new List<Func<UpdateDelegate, UpdateDelegate>>();
            _services = builder.Services;
            _configuration = builder.Configuration;

            UseMiddleware<UpdateContextMiddleware>();
        }

        public BotApplication Use(Func<UpdateContext, UpdateDelegate, Task> middlware)
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

            var client = new TelegramBotClient(apiKey);

            client.StartReceiving(invokeMiddlewares, handlePollingErrorAsync);
        }

        private async Task invokeMiddlewares(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _serviceProvider = _services.BuildServiceProvider();

            UpdateDelegate firstMiddlware = context => Task.CompletedTask;

            foreach (var middlwareFactory in _middlewares.Reverse())
                firstMiddlware = middlwareFactory.Invoke(firstMiddlware);

            await firstMiddlware.Invoke(new UpdateContext
            {
                BotClient = botClient,
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
