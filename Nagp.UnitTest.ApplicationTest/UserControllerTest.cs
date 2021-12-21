using AutoMapper;
using Moq;
using Nagp.UnitTest.Application.Common;
using Nagp.UnitTest.Application.Controllers;
using Nagp.UnitTest.Application.Model;
using Nagp.UnitTest.Business.Interface;
using Nagp.UnitTest.Business.Model;
using Nagp.UnitTest.EntityFrameworkCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nagp.UnitTest.ApplicationTest
{
    public class UserControllerTestFixture
    {
        public UserControllerPreSetup GetNewInstance()
        {
            return new UserControllerPreSetup();
        }
    }

    public class UserControllerPreSetup
    {
        private UserController _controller;
        public Mock<IUserManager> UserManager { get; set; } = new Mock<IUserManager>();

        public Mock<IMapper> ObjectMapper { get; set; } = new Mock<IMapper>();


        public UserController GetController()
        {
            if (_controller != null)
                return _controller;

            _controller = new UserController(UserManager.Object, ObjectMapper.Object);

            return _controller;
        }
    }
    public class UserControllerTests : IClassFixture<UserControllerTestFixture>
    {
        UserControllerTestFixture _fixture;

        public UserControllerTests(UserControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void AddFunds_Success()
        {
            var setup = _fixture.GetNewInstance();
            var request = new FundRequestDto()
            {
                Amount = 100,
                UserID = 45
            };
            setup.ObjectMapper
                .Setup(x =>
                x.Map<FundRequestDto, FundRequest>(request))
                .Returns(new FundRequest());

            setup.UserManager.Setup(x => x.AddFunds(It.IsAny<FundRequest>())).Returns(new User() { });

            var UserController = setup.GetController();
            //// act
            var response = UserController.AddFunds(request);
            Assert.Equal(Constants.Successful, response.Status);
        }

        //[Fact]
        //public async void AddFunds_ModelStateInvalid()
        //{
        //    var setup = _fixture.GetNewInstance();
        //    var request = new StockRequestDto()
        //    {
        //        StockId = 1,
        //        UserID = 45
        //    };
        //    setup.ObjectMapper
        //        .Setup(x =>
        //        x.Map<StockRequestDto, StockRequest>(request))
        //        .Returns(new StockRequest());

        //    setup.StockManager.Setup(x => x.Buy(It.IsAny<StockRequest>())).Returns(new User() { });
        //    //setup.GetController().va
        //    var UserController = setup.GetController();

        //    //// act
        //    var response = UserController.TryValidateModel(request);
        //    Assert.False(response);
        //}

        [Fact]
        public async void AddFunds_Returns_NULL()
        {
            var setup = _fixture.GetNewInstance();
            var request = new FundRequestDto()
            {
                Amount = 100,
                UserID = 45
            };
            setup.ObjectMapper
                .Setup(x =>
                x.Map<FundRequestDto, FundRequest>(request))
                .Returns(new FundRequest());

            setup.UserManager.Setup(x => x.AddFunds(It.IsAny<FundRequest>())).Returns(value: null);

            var UserController = setup.GetController();
            //// act
            var response = UserController.AddFunds(request);

            Assert.Equal(Constants.UnSuccessful, response.Status);
        }

        [Fact]
        public async void AddFunds_Throw_Exceptiosn()
        {
            var setup = _fixture.GetNewInstance();
            var request = new FundRequestDto()
            {
                Amount = 100,
                UserID = 45
            };
            setup.ObjectMapper
                .Setup(x =>
                x.Map<FundRequestDto, FundRequest>(request))
                .Returns(new FundRequest());

            setup.UserManager.Setup(x => x.AddFunds(It.IsAny<FundRequest>())).Throws(new Exception() { });

            var UserController = setup.GetController();
            //// act
            var response = UserController.AddFunds(request);
            Assert.Equal(Constants.UnSuccessful, response.Status);
        }


    }
}
