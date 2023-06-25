using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PdfConverter.PDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            Converter app = serviceProvider.GetService<Converter>();
            try
            {
                app.Start();
            }
            catch (Exception ex)
            {
                app.HandleError(ex);
            }
            finally
            {
                app.Stop();
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<Converter>();
            services.AddTransient<IPdfExecutor, PdfExecutor>();

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddNLog("nlog.config");
            });
        }
    }
}