using System;

namespace SimpleSettings.Exceptions
{
    public class ActiveTypeLockException : Exception
    {
        public ActiveTypeLockException(string message) : base("Active TypeLock: " + message)
        {
        }
    }
}