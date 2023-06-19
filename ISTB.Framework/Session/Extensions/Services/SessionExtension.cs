using ISTB.Framework.Session.Storages.Implementations;
using ISTB.Framework.Session.Storages.Interfaces;
using ISTB.Framework.Session.Storages.SessionSaver.Implementations;
using ISTB.Framework.Session.Storages.SessionSaver.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.Session.Extensions.Services
{
    public static class SessionExtension
    {
        public static IServiceCollection AddSessions(this IServiceCollection services)
        {
            return services.AddSessions<MemorySessionDataSaver>();
        }

        public static IServiceCollection AddSessions<TSaver>(this IServiceCollection services)
            where TSaver : class, ISessionDataSaver
        {
            services.AddTransient(typeof(Session<>));
            services.AddTransient<ISessionDataStorage, SessionDataStorage>();
            services.AddSingleton<ISessionDataSaver, TSaver>();
            return services;
        }
    }
}
