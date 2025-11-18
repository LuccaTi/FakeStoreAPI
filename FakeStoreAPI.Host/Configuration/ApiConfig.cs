using FakeStoreAPI.Host.Logging;

namespace FakeStoreAPI.Host.Configuration
{
    public static class ApiConfig
    {
        #region Atributes
        private const string _className = "ApiConfig";
        private static string? _apiBaseDirectory;
        private static bool _useSwaggerProduction;
        private static bool _useSerilog;
        private static string? _fakeStoreUrl;
        private static int _timeout;
        #endregion

        #region Dependencies
        private static IConfiguration? _config;
        #endregion

        #region Properties
        public static string? ApiBaseDirectory
        {
            get { return _apiBaseDirectory; }
        }
        public static bool UseSwaggerProduction
        {
            get { return _useSwaggerProduction; }
        }
        public static bool UseSerilog
        {
            get { return _useSerilog; }
        }
        public static string? FakeStoreUrl
        {
            get { return _fakeStoreUrl; }
        }
        public static int Timeout
        {
            get { return _timeout; }
        }
        #endregion

        #region Methods
        public static void LoadConfig()
        {
            try
            {
                _config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                _apiBaseDirectory = Path.GetDirectoryName(AppContext.BaseDirectory)!;
                string logDirectory = Path.Combine(_apiBaseDirectory, _config["AppConfig:LogDirectory"] ?? "logs").Replace(@"/", "\\");
                Logger.InitLogger(logDirectory);
                Logger.Info("Logger started");
                Logger.Info("Loading parameters...");

                _useSwaggerProduction = Convert.ToBoolean(_config["AppConfig:UseSwaggerProduction"]);
                Logger.Debug(_className, "LoadConfig", $"UseSwaggerProduction: {_useSwaggerProduction}");

                _useSerilog = Convert.ToBoolean(_config["AppConfig:UseSerilog"]);
                Logger.Debug(_className, "LoadConfig", $"UseSerilog: {_useSerilog}");

                _fakeStoreUrl = _config["AppConfig:FakeStoreUrl"] ?? throw new Exception("The client URL (fakestoreapi.com) was not provided via appsettings.json!");
                Logger.Debug(_className, "LoadConfig", $"FakeStoreUrl: {_fakeStoreUrl}");

                _timeout = Convert.ToInt32(_config["AppConfig:Timeout"]);
                if (_timeout == 0)
                    _timeout = 30;
                Logger.Debug(_className, "LoadConfig", $"Timeout = {_timeout} seconds");

                Logger.Info("Application parameters loaded");

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while loading the application settings!", ex);
            }
        }

        /// <summary>
        /// Get configuration parameter by key, if key is an object than parameter = "key:attribute" format
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string Get(string parameter)
        {
            try
            {
                return _config?[parameter]!;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "Get", $"{ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
