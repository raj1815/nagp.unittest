using System;
using System.Collections.Generic;
using System.Text;

namespace Nagp.UnitTest.Business.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string name)
            : base(String.Format("Error {0}", name))
        {

        }
    }
}
