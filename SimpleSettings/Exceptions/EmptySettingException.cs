using System;

namespace SimpleSettings.Exceptions
{
    public class EmptySettingException : Exception
    {
        public EmptySettingException(string message) : base("Empty setting: " + message)
        {
        }
    }
}