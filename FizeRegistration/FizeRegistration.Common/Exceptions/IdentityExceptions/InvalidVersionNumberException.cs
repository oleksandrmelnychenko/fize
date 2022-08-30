using System;

namespace FizeRegistration.Common.Exceptions.IdentityExceptions;

    public class InvalidVersionNumberException : Exception, IUserException
    {
        public const string INVALID_VERSION_NUMBER_MESSAGE = "Invalid version number";

        public string GetUserMessageException { get; private set; }
        public object Body { get; private set; }

        public void SetBody(object body)
        {
            Body = body;
        }

        public void SetUserMessage(string message)
        {
            GetUserMessageException = message;
        }
    }

