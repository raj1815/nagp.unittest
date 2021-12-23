using Moq;
using Nagp.UnitTest.Business.Exceptions;
using Nagp.UnitTest.Business.Interface;
using Nagp.UnitTest.Business.Model;
using Nagp.UnitTest.EntityFrameworkCore.Model;
using Nagp.UnitTest.EntityFrameworkCore.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nagp.UnitTest.Business.Test
{
    public class UserManagerFixture
    {
        public UserManagerPreSetup GetNewInstance()
        {
            return new UserManagerPreSetup();
        }
    }

    public class UserManagerPreSetup
    {
        private IUserManager _UserManager;

        public Mock<IUserRepository> User { get; set; } = new Mock<IUserRepository>();
    

        public IUserManager GetUserManager()
        {
            _UserManager =
                new UserManager(
                    User.Object);

            return _UserManager;
        }
    }

    public class UserManagerTest : IClassFixture<UserManagerFixture>
    {
        UserManagerFixture _fixture;

        public UserManagerTest(UserManagerFixture fixture)
        {
            _fixture = fixture;
        }
        public class TestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {100},
                new object[] {200},
                new object[] {500},
                new object[] {1000},
                new object[] {2000},
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Fact]
        public void When_addFunds_fail_if_user_not_exists()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();


            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(value: null);

            var manager =
                setup.GetUserManager();

            //Act
            var exception =
                 Assert.Throws<BusinessException>(() =>
                 {
                     manager.AddFunds(new FundRequest());
                 });

            Mock.VerifyAll();
        }

        [Theory, ClassData(typeof(TestData))]
        public void Check_brokerageCharges_Zero(double amount)
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new EntityFrameworkCore.Model.User() { Id = 10, AvailableAmount = 100 });

             Mock<IUserRepository> User = new Mock<IUserRepository>();

              UserManager userManager = new UserManager(User.Object);

                FundRequest request = new FundRequest();
                request.UserID = 10;
                request.Amount = 100;

            var response = userManager.brokerageCharges(amount);

            Assert.Equal(0, response);
            Mock.VerifyAll();
        }

        [Fact]
        public void Check_brokerageCharges_Higher()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new EntityFrameworkCore.Model.User() { Id = 10, AvailableAmount = 100 });

            Mock<IUserRepository> User = new Mock<IUserRepository>();

            UserManager userManager = new UserManager(User.Object);

            var amount = 100000;

            var response = userManager.brokerageCharges(amount);

            Assert.Equal(5000, response);
            Mock.VerifyAll();
        }

        [Fact]
        public void When_addFunds_success()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new EntityFrameworkCore.Model.User() { Id = 10, AvailableAmount = 100 });

            var manager =
                setup.GetUserManager();

            FundRequest request = new FundRequest();
            request.UserID = 10;
            request.Amount = 100;

            var response = manager.AddFunds(request);

            Assert.Equal(200, response.AvailableAmount);
            Mock.VerifyAll();
        }

        [Fact]
        public void When_addFunds_fail_when_user_update_fail()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new EntityFrameworkCore.Model.User() { Id = 10, AvailableAmount = 100 });

            setup.User.Setup((c) => c.Update(It.IsAny<User>()))
                .Throws(new Exception());

            var manager =
                setup.GetUserManager();

            FundRequest request = new FundRequest();
            request.UserID = 10;
            request.Amount = 100;

            //Act
            var exception =
                 Assert.Throws<Exception>(() =>
                 {
                     manager.AddFunds(request);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void When_addFunds_fail_when_user_save_fail()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup.User.Setup((c) => c.GetById(It.IsAny<int>()))
            .Returns(new EntityFrameworkCore.Model.User() { Id = 10, AvailableAmount = 100 });

            setup.User.Setup((c) => c.Save())
                .Throws(new Exception());

            var manager =
                setup.GetUserManager();

            FundRequest request = new FundRequest();
            request.UserID = 10;
            request.Amount = 100;

            //Act
            var exception =
                 Assert.Throws<Exception>(() =>
                 {
                     manager.AddFunds(request);
                 });
            Mock.VerifyAll();
            Mock.VerifyAll();
        }
    }
}
