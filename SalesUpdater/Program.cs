using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using SalesUpdater.Data;
using SalesUpdater.Core;
using SalesUpdater.Data.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace SalesUpdater
{
    class Program
    {

        static void Main(string[] args)
        {
            //Create instance of Email / Sheet Service through Dependency Injection
            var host = Startup.AppStartup();

            var app = ActivatorUtilities.CreateInstance<App>(host.Services);

            app.Run();

        }

    }
}