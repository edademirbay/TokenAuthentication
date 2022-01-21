using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SharedExpression.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException():base()
        {
        }

        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
        }

       
    }
}
