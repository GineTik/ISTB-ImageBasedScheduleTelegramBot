﻿using ISTB.Framework.TelegramBotApplication.Configuration.Services;
using ISTB.Framework.TelegramBotApplication.Helpers.Factories.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Polling;

namespace ISTB.Framework.TelegramBotApplication
{
    public class BotApplicationBuilder
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
        public ReceiverOptions ReceiverOptions { get; }

        public BotApplicationBuilder()
        {
            Services = new ServiceCollection();
            Configuration = new ConfigurationFactory().CreateConfiguration();
            ReceiverOptions = new ReceiverOptions();

            Services.AddSingleton(Configuration);
            Services.AddUpdateContextAccessor();
        }

        public BotApplication Build()
        {
            return new BotApplication(this);
        }
    }
}
