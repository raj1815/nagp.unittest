using Nagp.UnitTest.Business.Model;
using Nagp.UnitTest.EntityFrameworkCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nagp.UnitTest.Business.Interface
{
    public interface IUserManager
    {
        User AddFunds(FundRequest request);

    }
}
