using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolftechApi.ExeptionClasses
{
    public class LoadingError : Exception
    {
        public LoadingError(string message) : base(message)
        {

        }
    }
}
