using Microsoft.AspNetCore.Mvc;

namespace FizeRegistration.Common.WebApi;

public abstract class BaseAssignControllerRouteAttribute : RouteAttribute
{
    public string Version { get; protected set; }

    public string Environment { get; protected set; }

    public BaseAssignControllerRouteAttribute(string environment, int version, string routeTemplate)
        : base(routeTemplate)
    {

        Version = BuildRouteVersion(version);
        Environment = environment;
    }

    protected static string BuildRouteVersion(int number)
    {
        return $"v{number.ToString()}";
    }
}
