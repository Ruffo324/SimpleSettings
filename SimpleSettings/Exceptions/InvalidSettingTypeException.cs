using System;

namespace SimpleSettings.Exceptions
{
    public class InvalidSettingTypeException : Exception
    {
        public InvalidSettingTypeException(string message) : base("Invalid setting type: " + message)
        {
        }
    }
}