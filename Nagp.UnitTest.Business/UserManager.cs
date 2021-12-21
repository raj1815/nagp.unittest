using Nagp.UnitTest.Business.Common;
using Nagp.UnitTest.Business.Exceptions;
using Nagp.UnitTest.Business.Interface;
using Nagp.UnitTest.Business.Model;
using Nagp.UnitTest.EntityFrameworkCore.Model;
using Nagp.UnitTest.EntityFrameworkCore.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nagp.UnitTest.Business
{
    public class UserManager : IUserManager
    {
        public readonly IUserRepository _users;

        public UserManager(IUserRepository users)
        {
            _users = users;
        }
        public User AddFunds(FundRequest request)
        {
            var user = this._users.GetById(request.UserID);
            if (user == null)
            {
                throw new BusinessException("User doesnot exist");
            }
            var amount = request.Amount;
            

            user.AvailableAmount = user.AvailableAmount + request.Amount -  brokerageCharges(amount);
            this._users.Update(user);
            return user;
        }

        public double brokerageCharges(double amount)
        {
            double brokerage = 0;
            if (amount >= 10000)
            {
                brokerage = 0.05 * amount;
            }
            return brokerage;
        }
    }
}
