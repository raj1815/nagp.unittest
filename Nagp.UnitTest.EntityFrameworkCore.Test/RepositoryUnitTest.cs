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
                context.User.Add(new User()
                {
                    Id = 3,
                    Firstname = "random",
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
                    StockId = 2,
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
           // context.Database.EnsureDeleted();
        }

        [Fact]
        public void Check_stockallenity_fetched_correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                GenericRepository<Stock> repo = new GenericRepository<Stock>(context);
                var stock = repo.GetAll();
                Assert.True(stock.ToList().Count > 1);
            };

        }

        [Fact]
        public void Check_stockenity_fetched_correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                GenericRepository<Stock> repo = new GenericRepository<Stock>(context);
                var stock = repo.GetById(1);
                Assert.Equal("10", stock.Price.ToString());
            };

        }

        [Fact]
        public void Check_StockEnity_is_price_updated_correctly()
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
        public void Check_StockEnity_is_Quantity_updated_correctly()
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
        public void Check_userEnity_fetched_correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                UserRepository repo = new UserRepository(context);
                var user = repo.GetById(1);
                Assert.Equal("raj", user.Firstname);
                Assert.Equal("1", user.HoldingShares.Count.ToString());
            };

        }
        [Fact]
        public void Check_userAllEnity_fetched_correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                UserRepository repo = new UserRepository(context);
                var user = repo.GetAll();
                Assert.True(user.ToList().Count > 2);
            };

        }
        [Fact]
        public void Check_userEnity_is_added_correctly()
        {
            var user = new User() { Id = 10, Firstname ="test4", AvailableAmount = 100 };
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                UserRepository repo = new UserRepository(context);
                var oldUser = context.User.FirstOrDefault(s => s.Id == 10);

                repo.Insert(user);
                repo.Save();
                var newUser = context.User.FirstOrDefault(s => s.Id == 10);
                Assert.Null(oldUser);
                Assert.NotNull(newUser);
            };

        }
        [Fact]
        public void Check_userEnity_is_Price_updated_correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                UserRepository repo = new UserRepository(context);
                var user = repo.GetById(1);
                var oldValue = user.AvailableAmount;
                user.AvailableAmount = 20000;
                repo.Update(user);
                repo.Save();
                Assert.Equal(1000, oldValue);
                Assert.Equal(20000, user.AvailableAmount);
            };

        }
        [Fact]
        public void Check_userEnity_is_quantity_updated_correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                UserRepository repo = new UserRepository(context);
                var user = repo.GetById(1);
                var oldValue = user.Firstname;
                user.Firstname = "Test_raj";
                repo.Update(user);
                repo.Save();
                Assert.Equal("raj", oldValue);
                Assert.Equal("Test_raj", user.Firstname);
            };

        }
        [Fact]
        public void Check_stockenity_is_added_correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                var stock = new Stock() { Id = 10, Price = 10, Quantity = 100 };
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
        public void Check_holdshareenity_fetched_correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                GenericRepository<HoldingShare> repo = new GenericRepository<HoldingShare>(context);
                var user = repo.GetById(1);
                Assert.Equal(15, user.Price);
                Assert.Equal(1000, user.Quantity);
            };

        }
        [Fact]
        public void Check_holdshare_all_enity_fetched_correctly()
        {
            using (var context = new eTraderDBContext(this.fixture.option))
            {
                GenericRepository<HoldingShare> repo = new GenericRepository<HoldingShare>(context);
                var user = repo.GetAll();
                Assert.Single(user.ToList());
            };
        }
    }
}
