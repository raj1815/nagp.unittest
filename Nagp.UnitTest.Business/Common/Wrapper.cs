using System;
using System.Collections.Generic;
using System.Text;

namespace Nagp.UnitTest.Business.Common
{
    class Wrapper : IWrapper
    {
        public bool isTradingTime()
        {
            return Helper.isTradingTime();
        }
    }
}
