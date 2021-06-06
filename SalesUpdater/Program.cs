using Microsoft.Extensions.DependencyInjection;

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