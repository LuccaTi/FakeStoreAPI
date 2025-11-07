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

                #region DI Container

                string apiBaseDirectory = Path.GetDirectoryName(AppContext.BaseDirectory)!;

                builder.Configuration
                    .SetBasePath(apiBaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);


                if (string.IsNullOrEmpty(builder.Configuration["FakeStoreUrl"]))
                {
                    throw new Exception("The client URL was not provided via appsettings.json!");
                }

                string fakeStoreUrl = builder.Configuration["FakeStoreUrl"]!;
                int timeoutSeconds = Convert.ToInt32(builder.Configuration["TimeOut"]);

                builder.Services.AddHttpClient<IFakeStoreProductClient, FakeStoreProductClient>(client =>
                {
                    client.BaseAddress = new Uri(fakeStoreUrl);
                    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds == 0 ? 30 : timeoutSeconds);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
                builder.Services.AddHttpClient<IFakeStoreCartClient, FakeStoreCartClient>(client =>
                {
                    client.BaseAddress = new Uri(fakeStoreUrl);
                    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds == 0 ? 30 : timeoutSeconds);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
                builder.Services.AddHttpClient<IFakeStoreUserClient, FakeStoreUserClient>(client =>
                {
                    client.BaseAddress = new Uri(fakeStoreUrl);
                    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds == 0 ? 30 : timeoutSeconds);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });

                string logDirectory = Path.Combine(apiBaseDirectory, builder.Configuration["Startup:LogDirectory"] ?? "logs").Replace(@"/", "\\");
                Logger.InitLogger(logDirectory);
                StoreAPIConfig.LoadConfig();
                Logger.Info("Program.cs", "Main", "Application settings loaded, logger started!");

                //builder.Host.UseSerilog();

                builder.Services.AddScoped<IProductService, ProductService>();
                builder.Services.AddScoped<ICartService, CartService>();
                builder.Services.AddScoped<IUserService, UserService>();

                builder.Services.AddAutoMapper(typeof(Program).Assembly);

                builder.Services.AddControllers();
                bool useSwagger = Convert.ToBoolean(builder.Configuration["Startup:UseSwagger"]);
                if (useSwagger)
                {
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen();
                }

                #endregion

                var app = builder.Build();

                #region Middleware

                if (useSwagger)
                {
                    app.UseStaticFiles();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                // Automatically accesses swagger when clicking on the listening link (Now listening) - Console
                app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                #endregion

                Logger.Info("Program.cs", "Main", "All parameters loaded, application starting...");

                if (useSwagger && !app.Environment.IsDevelopment())
                {
                    // Always switch to use https
                    app.Lifetime.ApplicationStarted.Register(() =>
                    {
                        var address = app.Urls.FirstOrDefault();
                        if (address != null && address.StartsWith("http://"))
                        {
                            address = address.Replace("http://", "https://");
                        }
                        var swaggerUrl = $"{address}/swagger";
                        Logger.Info("Program.cs", "Main", $"===== Opening browser on: {swaggerUrl} =====");

                        OpenBrowser(swaggerUrl);
                    });
                }
                else
                {
                    Logger.Info("Program.cs", "Main", $"Swagger was disabled!");
                }

                app.Run();

                Logger.Info("Program.cs", "Main", "Request to finalize received, stopping the application...");
                Logger.Info("Program.cs", "Main", "Application stopped!");
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
