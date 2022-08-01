using System.IO;
using FizeRegistration.Common.Helpers;
using Microsoft.Extensions.Configuration;

namespace FizeRegistration.Common;

public class ConfigurationManager
{
    private static string _databaseConnectionString;

    private static AppSettings _appSettings;

    public static void SetAppSettingsProperties(IConfiguration configuration)
    {
        _databaseConnectionString = configuration.GetConnectionString(ConnectionStringNames.DefaultConnection);

        AppSettings appSettings = new AppSettings
        {
            TokenSecret = configuration.GetSection("ApplicationSettings")["TokenSecret"],
            TokenExpiryDays = int.Parse(configuration.GetSection("ApplicationSettings")["TokenExpiryDays"]),
            PasswordWeakErrorMessage =
                configuration.GetSection("ApplicationSettings")["PasswordWeakErrorMessage"],
            PasswordStrongRegex = configuration.GetSection("ApplicationSettings")["PasswordStrongRegex"],
            PasswordExpiryDays =
                int.Parse(configuration.GetSection("ApplicationSettings")["PasswordExpiryDays"]),
        };

        _appSettings = appSettings;
    }

    public static string DatabaseConnectionString => _databaseConnectionString;

    public static AppSettings AppSettings => _appSettings;

}
