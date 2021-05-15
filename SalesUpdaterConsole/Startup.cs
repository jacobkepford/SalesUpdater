using Microsoft.Extensions.DependencyInjection;
using System;
using EmailApi;

namespace SalesUpdaterConsole
{
    public static class Startup
    {
        public static IServiceProvider ConfigureService()
        {
            var provider = new ServiceCollection().AddSingleton<IEmailService, EmailService>().BuildServiceProvider();

            return provider;
        }
    }
}