using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class NotOwnerException : Exception
    {
        public NotOwnerException()
        {
        }

        public NotOwnerException(string? message) : base(message)
        {
        }
    }
}
