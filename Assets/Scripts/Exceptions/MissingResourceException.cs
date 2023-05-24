using System;

namespace Exceptions
{
    public class MissingResourceException : Exception
    {
        public MissingResourceException()
        {
        }
        
        public MissingResourceException(string message) : base(message)
        {
        }
    }
}