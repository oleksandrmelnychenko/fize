using System;

namespace FizeRegistration.Common.Exceptions.IdentityExceptions;

public class InvalidIdentityException : Exception, IUserException
{
    public string GetUserMessageException { get; private set; }
    public object Body { get; private set; }
    public void SetUserMessage(string message)
    {
        GetUserMessageException = message;
    }

    public void SetBody(object body)
    {
        Body = body;
    }
}
