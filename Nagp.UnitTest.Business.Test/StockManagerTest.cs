using Moq;
using Nagp.UnitTest.Business.Common;
using Nagp.UnitTest.Business.Exceptions;
using Nagp.UnitTest.Business.Interface;
using Nagp.UnitTest.Business.Model;
using Nagp.UnitTest.EntityFrameworkCore.Model;
using Nagp.UnitTest.EntityFrameworkCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nagp.UnitTest.Business.Test
{
    public class StockManagerFixture
    {
        public ControllerPreSetup GetNewInstance()
        {
            return new ControllerPreSetup();
        }
    }

    public class ControllerPreSetup
    {
        private IStockManager _stockManager;
    
        public Mock<IUserRepository> User { get; set; } = new Mock<IUserRepository>();

        public Mock<IRepository<Stock>> Stock { get; set; } = new Mock<IRepository<Stock>>();

        public Mock<IWrapper> Wrapper { get; set; } = new Mock<IWrapper>();

        public Mock<IRepository<HoldingShare>> HoldingShare { get; set; } = new Mock<IRepository<HoldingShare>>();
        public IStockManager GetStockManager()
        {
            _stockManager =
                new StockManager(
                    User.Object,
                    Stock.Object,
                    HoldingShare.Object,
                    Wrapper.Object);

            return _stockManager;
        }
    }

    public class StockManagerTest : IClassFixture<StockManagerFixture>
    {
        StockManagerFixture _fixture;

        public StockManagerTest(StockManagerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void When_sell_fail_if_user_not_exists()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock());

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(value: null);

            setup.Wrapper.Setup((c) => c.isTradingTime())
           .Returns(true);

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;

            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                {
                   manager.Sell(stockRequest);
                });

            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_fail_when_no_tradingtim()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;

            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                 {
                     manager.Sell(stockRequest);
                 });

            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_fail_if_stock_not_exists()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(value: null);

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User());

            setup.Wrapper.Setup((c) => c.isTradingTime())
           .Returns(true);

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;

            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                 {
                     manager.Sell(stockRequest);
                 });

            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_fail_if_stock_not_exists_for_User()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 10 });

            setup.Wrapper.Setup((c) => c.isTradingTime())
           .Returns(true);

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User()
            {
                Id = 5,
                HoldingShares = new List<HoldingShare>()
            });

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;
            stockRequest.StockId = 6;

            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                 {
                     manager.Sell(stockRequest);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_fail_if_stock_quantity_more_than_User_hold()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 10 });

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User()
            {
                Id = 5,
                HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 10, Price = 20, Quantity = 5 } }
            });

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);
            
            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;
            stockRequest.StockId = 10;
            stockRequest.Quantity = 20;
            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                 {
                     manager.Sell(stockRequest);
                 });
            Assert.Equal("Error Stock Quantity is more", exception.Message);
            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_fail_if_user_not_have_balance()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 10 , Price = 2});

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User()
            {
                Id = 5,
                AvailableAmount = 5,
                HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 10, Price = 2, Quantity = 5 } }
            });

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;
            stockRequest.StockId = 10;
            stockRequest.Quantity = 2;
            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                 {
                     manager.Sell(stockRequest);
                 });
            Assert.Equal("Error Insufficient funds", exception.Message);
            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_success_brokage_lessthan_20()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 10 ,Price = 10});

            setup.Wrapper.Setup((c) => c.isTradingTime())
                .Returns(true);

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User()
            {
                Id = 5,
                AvailableAmount = 10,
                HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 10, Price = 20, Quantity = 20 } }
            }); 

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 5;
            stockRequest.StockId = 10;
            stockRequest.Quantity = 5;
            //Act

            var response = manager.Sell(stockRequest);
            var holdShare = response.HoldingShares.Where(h => h.ShareId == stockRequest.StockId).FirstOrDefault();

            Assert.Equal(40, response.AvailableAmount);
            Assert.Equal(15, holdShare.Quantity);
            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_success_brokage_greaterthan_20()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 10, Price = 10 });

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User()
            {
                Id = 5,
                AvailableAmount = 10000,
                HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 10, Price = 20, Quantity = 2000 } }
            });

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 5;
            stockRequest.StockId = 10;
            stockRequest.Quantity = 1000;

            //Act

            var response = manager.Sell(stockRequest);
            var holdShare = response.HoldingShares.Where(h => h.ShareId == stockRequest.StockId).FirstOrDefault();

            Assert.Equal(19980, response.AvailableAmount);
            Assert.Equal(1000, holdShare.Quantity);
            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_fail_when_user_update_fail()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 10, Price = 10 });

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User()
            {
                Id = 5,
                AvailableAmount = 10000,
                HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 10, Price = 20, Quantity = 2000 } }
            });

            setup.User.Setup((c) => c.Update(It.IsAny<User>()))
            .Throws(new Exception());

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 5;
            stockRequest.StockId = 10;
            stockRequest.Quantity = 1000;

            //Act
            var exception =
                 Assert.Throws<Exception>(() =>
                 {
                     manager.Sell(stockRequest);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_fail_when_stock_update_fail()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 10, Price = 10 });

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User()
            {
                Id = 5,
                AvailableAmount = 10000,
                HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 10, Price = 20, Quantity = 2000 } }
            });

            setup.Stock.Setup((c) => c.Update(It.IsAny<Stock>()))
            .Throws(new Exception());

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 5;
            stockRequest.StockId = 10;
            stockRequest.Quantity = 1000;

            //Act
            var exception =
                 Assert.Throws<Exception>(() =>
                 {
                     manager.Sell(stockRequest);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_sell_fail_when_user_save_fail()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 10, Price = 10 });

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User()
            {
                Id = 5,
                AvailableAmount = 10000,
                HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 10, Price = 20, Quantity = 2000 } }
            });

            setup.User.Setup((c) => c.Save())
            .Throws(new Exception());

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 5;
            stockRequest.StockId = 10;
            stockRequest.Quantity = 1000;

            //Act
            var exception =
                 Assert.Throws<Exception>(() =>
                 {
                     manager.Sell(stockRequest);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_buy_fail_if_user_not_exists()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock());

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(value: null);

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;

            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                 {
                     manager.Buy(stockRequest);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_buy_fail_if_stock_not_exists()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(value: null);

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User());

            var manager =
                setup.GetStockManager();

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;

            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                 {
                     manager.Buy(stockRequest);
                 });
            Mock.VerifyAll();
        }
        [Fact]
        public void When_buy_fail_if_funds_not_sufficient()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 1, Price= 100, Quantity= 1000});

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User() {Id = 2, Firstname = "test", AvailableAmount = 50 }); ;

            setup.Wrapper.Setup((c) => c.isTradingTime())
             .Returns(true);

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 2;
            stockRequest.StockId = 1;
            stockRequest.Quantity = 5;

            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                 {
                     manager.Buy(stockRequest);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_buy_success_when_user_dont_have_share()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 1, Price = 100, Quantity = 1000 });

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User() { Id = 2, Firstname = "test", AvailableAmount = 5000 }); ;

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 2;
            stockRequest.StockId = 1;
            stockRequest.Quantity = 5;

            //Act
            var response = manager.Buy(stockRequest);
            var holdShare = response.HoldingShares.Where(h => h.ShareId == stockRequest.StockId).FirstOrDefault();
            var holdShareNotPresent = response.HoldingShares.Where(h => h.ShareId == 100).FirstOrDefault();

            Assert.NotNull(holdShare);
            Assert.Null(holdShareNotPresent);
            Mock.VerifyAll();
        }

        [Fact]
        public void When_buy_success_when_user_have_other_share()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();
            var availableAmount = 1000;
            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 2, Price = 10, Quantity = 1000 });

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User()
            {
                Id = 2,
                Firstname = "test",
                AvailableAmount = availableAmount,
                HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 1, Quantity = 5, Price = 100 } }
            });

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 2;
            stockRequest.StockId = 2;
            stockRequest.Quantity = 5;

            //Act
            var response = manager.Buy(stockRequest);
            var holdShare = response.HoldingShares.Where(h => h.ShareId == stockRequest.StockId).FirstOrDefault();
        
            Assert.NotNull(holdShare);
            Assert.Equal(2, response.HoldingShares.Count);
            Mock.VerifyAll();
        }

        [Fact]
        public void When_buy_success_when_user_have_share()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            var availableAmount = 5000;
            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock() { Id = 1, Price = 100, Quantity = 1000 });

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User() { Id = 2, Firstname = "test", AvailableAmount = availableAmount,
            HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 1, Quantity = 5, Price= 100} }
            }); 

            var manager =
                setup.GetStockManager();

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 2;
            stockRequest.StockId = 1;
            stockRequest.Quantity = 5;
          
            //Act
            var response = manager.Buy(stockRequest);
            var holdShare = response.HoldingShares.Where(h => h.ShareId == stockRequest.StockId).FirstOrDefault();            var holdShareNotPresent = response.HoldingShares.Where(h => h.ShareId == 100).FirstOrDefault();

            Assert.NotNull(holdShare);
            Assert.Equal(10, holdShare.Quantity);
            Assert.Equal(availableAmount - (100 * stockRequest.Quantity), response.AvailableAmount);
            Mock.VerifyAll();
        }

        [Fact]
        public void When_buy_fail_when_holdingShare_insert_fail()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock());

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User());

            setup.HoldingShare.Setup((c) => c.Insert(It.IsAny<HoldingShare>()))
            .Throws(new Exception());

            var manager =
                setup.GetStockManager();

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;

            //Act
            var exception =
                 Assert.Throws<Exception>(() =>
                 {
                     manager.Buy(stockRequest);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_buy_fail_when_holdingShare_update_fail()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock());

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
             .Returns(new User()
             {
                 Id = 5,
                 HoldingShares = new List<HoldingShare>() { new HoldingShare() { ShareId = 10, Price = 20, Quantity = 5 } }
             });

            setup.HoldingShare.Setup((c) => c.Update(It.IsAny<HoldingShare>()))
            .Throws(new Exception());

            var manager =
                setup.GetStockManager();

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;
            stockRequest.StockId = 10;

            //Act
            var exception =
                 Assert.Throws<Exception>(() =>
                 {
                     manager.Buy(stockRequest);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_buy_fail_when_user_update_fail()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock());

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User());

            setup.User.Setup((c) => c.Update(It.IsAny<User>()))
            .Throws(new Exception());

            var manager =
                setup.GetStockManager();

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;

            //Act
            var exception =
                 Assert.Throws<Exception>(() =>
                 {
                     manager.Buy(stockRequest);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_buy_fail_when_save_fail()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.Stock.Setup((c) => c.GetById(It.IsAny<int>()))
                .Returns(new Stock());

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new User());

            setup.User.Setup((c) => c.Save())
            .Throws(new Exception());

            var manager =
                setup.GetStockManager();

            setup.Wrapper.Setup((c) => c.isTradingTime())
            .Returns(true);

            StockRequest stockRequest = new StockRequest();
            stockRequest.UserID = 4;

            //Act
            var exception =
                 Assert.Throws<Exception>(() =>
                 {
                     manager.Buy(stockRequest);
                 });
            Mock.VerifyAll();
        }
    }
}
