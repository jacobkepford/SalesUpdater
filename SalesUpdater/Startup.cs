using Microsoft.Extensions.DependencyInjection;
using System;
using SalesUpdater.Data;

namespace SalesUpdater
{
    public static class Startup
    {
        public static IServiceProvider ConfigureService()
        {
            var provider = new ServiceCollection()
                                .AddSingleton<IEmailService, EmailService>()
                                .AddSingleton<ISheetService, SheetService>()
                                .BuildServiceProvider();

            return provider;
        }
    }
}