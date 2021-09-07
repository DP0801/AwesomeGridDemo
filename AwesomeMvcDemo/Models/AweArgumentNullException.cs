using System;

namespace AwesomeMvcDemo.Models
{
    public class AweArgumentNullException : ArgumentNullException
    {
        public AweArgumentNullException(string paramName) : base(paramName)
        {
        }
    }
}