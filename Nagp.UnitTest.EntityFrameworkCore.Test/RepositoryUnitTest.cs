using Microsoft.EntityFrameworkCore;
using Nagp.UnitTest.EntityFrameworkCore;
using Nagp.UnitTest.EntityFrameworkCore.Model;
using Nagp.UnitTest.EntityFrameworkCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nagp.UnitTest.EntityFrameworkCoreTest
{
    public class DatabaseFixture : IDisposable
    {
        public DbContextOptions option;

        public DatabaseFixture()
        {
            option = new DbContextOptionsBuilder<eTraderDBContext>().UseInMemoryDatabase(databaseName: "eTraderDB").Options;
            using (var context = new eTraderDBContext(option))
            {
                context.User.Add(new User()
                {
                    Id = 1,
                    Firstname = "raj",
                    AvailableAmount = 1000,
                });
                context.User.Add(new User()
                {
                    Id = 2,
                    Firstname = "test",
                    AvailableAmount = 10000,
                });
                context.Stock.Add(new Stock()
                {
                    Id = 1,
                    Price = 10,
                    Quantity = 1000,
                });
                context.Stock.Add(new Stock()
                {
                    Id = 2,
                    Price = 15,
                    Quantity = 1000,
                });
                context.HoldingShare.Add(new HoldingShare()
                {
                    Id = 2,
                    Price = 15,
                    Quantity = 1000,
                    UserId = 1
                });
                context.SaveChanges();
            }
            
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }
    }

    public class RepositoryUnitTest : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;
        public RepositoryUnitTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Check_StockEnity_Fetched_Correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                GenericRepository<Stock> repo = new GenericRepository<Stock>(context);
                var stock = repo.GetById(1);
                Assert.Equal("10", stock.Price.ToString());
            };

        }
        [Fact]
        public void Check_StockAllEnity_Fetched_Correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                GenericRepository<User> repo = new GenericRepository<User>(context);
                var stock = repo.GetAll();
                Assert.Equal(2, stock.ToList().Count);
            };

        }
        [Fact]
        public void check_StockEnity_Is_Added_Correctly()
        {
            var stock = new Stock() { Id = 10, Price = 10, Quantity = 100 };
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                GenericRepository<Stock> repo = new GenericRepository<Stock>(context);
                var oldStock = context.Stock.FirstOrDefault(s => s.Id == 10);

                repo.Insert(stock);
                repo.Save();
                var newStock = context.Stock.FirstOrDefault(s => s.Id == 10);
                Assert.Null(oldStock);
                Assert.NotNull(newStock);
            };

        }
        [Fact]
        public void check_StockEnity_Is_Price_Updated_Correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                GenericRepository<Stock> repo = new GenericRepository<Stock>(context);
                var stock = repo.GetById(1);
                var oldValue = stock.Price;
                stock.Price = 20;
                repo.Update(stock);
                repo.Save();
                Assert.Equal("10", oldValue.ToString());
                Assert.Equal("20", stock.Price.ToString());
            };

        }
        [Fact]
        public void check_StockEnity_Is_Quantity_Updated_Correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                GenericRepository<Stock> repo = new GenericRepository<Stock>(context);
                var stock = repo.GetById(1);
                var oldValue = stock.Quantity;
                stock.Quantity = 2000;
                repo.Update(stock);
                repo.Save();
                Assert.Equal("1000", oldValue.ToString());
                Assert.Equal("2000", stock.Quantity.ToString());
            };

        }
        [Fact]
        public void check_userEnity_Fetched_Correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                UserRepository repo = new UserRepository(context);
                var user = repo.GetById(1);
                Assert.Equal("raj", user.Firstname);
                Assert.Equal("1", user.HoldingShares.Count.ToString());
            };

        }
    }
}
