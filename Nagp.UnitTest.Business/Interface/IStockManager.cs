using Nagp.UnitTest.Business.Model;
using Nagp.UnitTest.EntityFrameworkCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nagp.UnitTest.Business.Interface
{
    public interface IStockManager
    {
        User Buy(StockRequest stockRequest);

        User Sell(StockRequest stockRequest);
    }
}
