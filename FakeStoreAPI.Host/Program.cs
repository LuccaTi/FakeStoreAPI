using FakeStoreAPI.Host.Logging;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using FakeStoreAPI.Host.Configuration;
using FakeStoreAPI.Host.Clients.Interfaces;
using FakeStoreAPI.Host.Services.Interfaces;
using FakeStoreAPI.Host.Services;
using FakeStoreAPI.Host.Clients.Internal;
using FakeStoreAPI.Host.Clients;

namespace FakeStoreAPI.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                ApiConfig.LoadConfig();

                builder.Configuration
                    .SetBasePath(ApiConfig.ApiBaseDirectory!)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

                #region DI Container
                builder.Services.AddHttpClient<IFakeStoreProductClient, FakeStoreProductClient>(client =>
                {
                    client.BaseAddress = new Uri(ApiConfig.FakeStoreUrl!);
                    client.Timeout = TimeSpan.FromSeconds(ApiConfig.Timeout == 0 ? 30 : ApiConfig.Timeout);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
                builder.Services.AddHttpClient<IFakeStoreCartClient, FakeStoreCartClient>(client =>
                {
                    client.BaseAddress = new Uri(ApiConfig.FakeStoreUrl!);
                    client.Timeout = TimeSpan.FromSeconds(ApiConfig.Timeout == 0 ? 30 : ApiConfig.Timeout);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
                builder.Services.AddHttpClient<IFakeStoreUserClient, FakeStoreUserClient>(client =>
                {
                    client.BaseAddress = new Uri(ApiConfig.FakeStoreUrl!);
                    client.Timeout = TimeSpan.FromSeconds(ApiConfig.Timeout == 0 ? 30 : ApiConfig.Timeout);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
                Logger.Info("Httpclients added");

                if (ApiConfig.UseSerilog)
                    builder.Host.UseSerilog();

                builder.Services.AddScoped<IProductService, ProductService>();
                builder.Services.AddScoped<ICartService, CartService>();
                builder.Services.AddScoped<IUserService, UserService>();
                Logger.Info("Dependencies injected");

                builder.Services.AddAutoMapper(typeof(Program).Assembly);
                Logger.Info("AutoMapper added");

                builder.Services.AddControllers();
                Logger.Info("Controllers added");

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                #endregion

                var app = builder.Build();

                #region Middleware

                app.UseStaticFiles();
                app.UseSwagger();
                app.UseSwaggerUI();

                // Automatically accesses swagger when clicking on the listening link (Now listening) - Console
                app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                #endregion

                Logger.Info("All settings loaded, application starting...");

                if (!app.Environment.IsDevelopment())
                {
                    if (ApiConfig.UseSwaggerProduction)
                    {
                        // Always switch to use https
                        app.Lifetime.ApplicationStarted.Register(() =>
                        {
                            var address = app.Urls.FirstOrDefault();
                            if (address != null)
                            {
                                if (address.StartsWith("http://"))
                                {
                                    address = address.Replace("http://", "https://");
                                }

                                address = address.Replace("0.0.0.0", "localhost");
                            }

                            var swaggerUrl = $"{address}/swagger";
                            Logger.Debug("Program.cs", "Main", $" ===== Now listening on: {swaggerUrl} ===== ");

                            OpenBrowser(swaggerUrl);
                        });
                    }
                }

                app.Run();

                Logger.Info("Request to finalize received, stopping the application...");
                Logger.Info("Application stopped!");
            }
            catch (Exception ex)
            {
                HandleStartupError(ex);
                Console.WriteLine($"{DateTime.Now} - Error in application startup: {ex}");
                Environment.Exit(1);
            }
        }

        private static void OpenBrowser(string url)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Program.cs", "OpenBrowser", $"Error while opening browser: {ex.Message}");
                throw;
            }
        }

        private static void HandleStartupError(Exception exception)
        {
            // Creates a file due to the chance that Logger might not have been started

            string apiDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            string fatalErrorDirectory = Path.Combine(apiDirectory, "StartupErrors");
            if (!Directory.Exists(fatalErrorDirectory))
                Directory.CreateDirectory(fatalErrorDirectory);

            string timeStamp = DateTime.Now.Date.ToString("yyyyMMdd");
            string file = Path.Combine(fatalErrorDirectory, $"{timeStamp}_ERROR_.txt");
            string errorMsg = $"{DateTime.Now} - Error in application startup: {exception.ToString()}{Environment.NewLine}";
            File.AppendAllText(file, errorMsg);
        }
    }
}
