using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolftechApi.ExeptionClasses
{
    public class ParsingError : Exception
    {
        public ParsingError(string message) : base(message)
        {
        }
    }
}
