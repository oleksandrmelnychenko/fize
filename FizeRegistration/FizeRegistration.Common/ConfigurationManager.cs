using System.IO;
using FizeRegistration.Common.Helpers;
using Microsoft.Extensions.Configuration;

namespace FizeRegistration.Common;

public class ConfigurationManager
{
    private static string _databaseConnectionString;

    private static AppSettings _appSettings;

    private static EmailCredentials _emailCredentials;

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

        EmailCredentials emailCredentials = new EmailCredentials()
        {
            UserName = configuration.GetSection("EmailCredentials")["UserName"],
            Password = configuration.GetSection("EmailCredentials")["Password"],
        };

        _appSettings = appSettings;
        _emailCredentials = emailCredentials;
    }

    public static string DatabaseConnectionString => _databaseConnectionString;

    public static AppSettings AppSettings => _appSettings;

    public static EmailCredentials EmailCredentials => _emailCredentials;

}
