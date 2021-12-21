using Nagp.UnitTest.Business.Interface;
using Nagp.UnitTest.Business.Model;
using Nagp.UnitTest.EntityFrameworkCore.Model;
using Nagp.UnitTest.EntityFrameworkCore.Repository;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Nagp.UnitTest.Business.Common;
using Nagp.UnitTest.Business.Exceptions;

namespace Nagp.UnitTest.Business
{
    public class StockManager : IStockManager
    {
        public readonly IUserRepository _users;
        public readonly IRepository<Stock> _stock;
        public readonly IRepository<HoldingShare> _holdingShare;
        public readonly IWrapper _wrapper;
        public StockManager(IUserRepository users, IRepository<Stock> stock, IRepository<HoldingShare> holdingShare, IWrapper wrapper)
        {
            _users = users;
            _stock = stock;
            _holdingShare = holdingShare;
            _wrapper = wrapper;
        }
        public User Buy(StockRequest stockRequest)
        {
            User response = null;
            if (_wrapper.isTradingTime())
            {
                var user = this._users.GetById(stockRequest.UserID);
                var stock = this._stock.GetById(stockRequest.StockId);

                if (user == null)
                {
                    throw new BusinessException("User doesnot exist");
                }

                if (stock == null)
                {
                    throw new BusinessException("Stock doesnot exist");
                }

                if((stock.Price * stockRequest.Quantity) > user.AvailableAmount)
                {
                    throw new BusinessException("Insufficient funds");
                }

                user.AvailableAmount = user.AvailableAmount - (stock.Price * stockRequest.Quantity);

                var stockExist = user.HoldingShares?.FirstOrDefault(s => s.Id == stockRequest.StockId);
                if (stockExist != null)
                {
                    stockExist.Quantity += stockRequest.Quantity;
                    this._holdingShare.Update(stockExist);
                    user.HoldingShares = user.HoldingShares;
                }
                else
                {
                    HoldingShare newStock = new HoldingShare()
                    {
                        Id = stock.Id,
                        Price = stock.Price,
                        Quantity = stockRequest.Quantity,
                        UserId = stockRequest.UserID
                    };

                    this._holdingShare.Insert(newStock);
                    user.HoldingShares = new List<HoldingShare>() { newStock };
                }
                this._users.Update(user);
                this._users.Save();
                response = user;
            }
            return response;
        }

        public User Sell(StockRequest stockRequest)
        {
            var response = new User();
            if (_wrapper.isTradingTime())
            {
                response = this._users.GetById(stockRequest.UserID);
                var stock = this._stock.GetById(stockRequest.StockId);

                if (response == null)
                {
                    throw new BusinessException("User doesnot exist");
                }

                if (stock == null)
                {
                    throw new BusinessException("Stock doesnot exist");
                }

                var stockExist = response.HoldingShares.FirstOrDefault(s => s.Id == stockRequest.StockId);

                if (stockExist == null)
                {
                    throw new BusinessException("Stock is present to user");
                }

                if (stockRequest.Quantity > stockExist.Quantity)
                {
                    throw new BusinessException("Stock Quantity is more");
                }

                var brokerageCharge = (double)0.05 * (stock.Price * stockRequest.Quantity);

                if (brokerageCharge < 20)
                {
                    brokerageCharge = 20;
                }
                var amountAfterBrokerage = (stock.Price * stockRequest.Quantity) - brokerageCharge;

                if (amountAfterBrokerage + response.AvailableAmount < 20)
                {
                    throw new BusinessException("Insufficient funds");
                }
                response.AvailableAmount = response.AvailableAmount + (int)amountAfterBrokerage;

                stockExist.Quantity -= stockRequest.Quantity;

                stock.Quantity += stockRequest.Quantity;

                this._users.Update(response);
                this._stock.Update(stock);
                this._users.Save();
            }
            else
            {
                throw new BusinessException("Not a trading time");
            }
            return response;
        }
    }
}
