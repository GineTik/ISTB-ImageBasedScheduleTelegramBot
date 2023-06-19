﻿using ISTB.Framework.TelegramBotApplication.Context;
using System.Reflection;

namespace ISTB.Framework.Executors.Storages.Interfaces
{
    public interface ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        Task<MethodInfo?> GetMethodInfoToExecuteAsync(UpdateContext updateContext);
    }
}
