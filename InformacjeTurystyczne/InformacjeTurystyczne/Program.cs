using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; // CreateScope()
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InformacjeTurystyczne
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // jaki� useless defaultowy generowany syf (nie u�ywamy)
            //CreateHostBuilder(args).Build().Run();
            
            // inicjujemy baz� danych naszym stuffem poni�ej
            var host = CreateHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    context.Database.Migrate();
                    DbInitializer.Seed(context);
                }
                catch(Exception ex)
                {
                    // nie uda�o si� zainicjowa� bazy danych
                }
            }

            host.Run();
        }

        // metoda CreateDefaultBuilder wed�ug dokumentacji automatycznie ogarnia plik appsetting.json (ustawie� aplikacji)
        // doda�em tam connect stringa do bazy danych
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
