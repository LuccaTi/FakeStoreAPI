using FakeStoreAPI.Host.Logging;

namespace FakeStoreAPI.Host.Configuration
{
    public static class StoreAPIConfig
    {
        #region Atributes
        private const string _className = "StoreAPIConfig";
        private static IConfiguration? _config;
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
