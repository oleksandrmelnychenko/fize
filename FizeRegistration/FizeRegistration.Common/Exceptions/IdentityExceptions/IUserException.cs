
namespace FizeRegistration.Common.Exceptions.IdentityExceptions;

public interface IUserException
{
    string GetUserMessageException { get; }

    object Body { get; }

    void SetUserMessage(string message);

    void SetBody(object body);
}

