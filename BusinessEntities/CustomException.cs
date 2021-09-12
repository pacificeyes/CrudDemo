using System;

namespace BusinessEntities
{
    public class CustomException : Exception
    {
        public CustomException(string exceptionMessage) : base(exceptionMessage)
        {
        }
    }
}
