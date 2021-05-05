using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ticketsystem_backend
{
    public class Program
    {
        // build host instance and start it
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // configure host
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
