﻿using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Commands
{
    [TargetCommand("start")]
    public class StartCommand : Executor
    {
        public override async Task ExecuteAsync()
        {
            await SendTextAsync("Success");
        }
    }
}
