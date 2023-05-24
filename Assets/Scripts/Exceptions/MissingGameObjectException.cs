using System;

namespace Exceptions
{
    public class MissingGameObjectException : Exception
    {
        public MissingGameObjectException()
        {
        }
        
        public MissingGameObjectException(string message) : base(message)
        {
        }
    }
}