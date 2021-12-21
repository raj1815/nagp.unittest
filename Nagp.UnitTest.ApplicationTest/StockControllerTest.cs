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
    public class StockControllerTestFixture
    {
        public StockControllerPreSetup GetNewInstance()
        {
            return new StockControllerPreSetup();
        }
    }

    public class StockControllerPreSetup
    {
        private StockController _controller;
        public Mock<IStockManager> StockManager { get; set; } = new Mock<IStockManager>();

        public Mock<IMapper> ObjectMapper { get; set; } = new Mock<IMapper>();


        public StockController GetController()
        {
            if (_controller != null)
                return _controller;

            _controller = new StockController(StockManager.Object, ObjectMapper.Object);
          
            return _controller;
        }
    }
    public class StockControllerTests : IClassFixture<StockControllerTestFixture>
    {
        StockControllerTestFixture _fixture;

        public StockControllerTests(StockControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Buy_Success()
        {
            var setup = _fixture.GetNewInstance();
            var request = new StockRequestDto()
            {
                StockId = 1,
                Quantity = 2,
                UserID = 45
            };
            setup.ObjectMapper
                .Setup(x =>
                x.Map<StockRequestDto, StockRequest>(request))
                .Returns(new StockRequest());

            setup.StockManager.Setup(x => x.Buy(It.IsAny<StockRequest>())).Returns(new User() { });

            var StockController = setup.GetController();   
            //// act
            var response = StockController.Buy(request);
            Assert.Equal(Constants.Successful, response.Status);
        }

        //[Fact]
        //public async void Buy_ModelStateInvalid()
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
        //    var stockController = setup.GetController();
        
        //    //// act
        //    var response = stockController.TryValidateModel(request);
        //    Assert.False(response);
        //}

        [Fact]
        public async void Buy_Returns_NULL()
        {
            var setup = _fixture.GetNewInstance();
            var request = new StockRequestDto()
            {
                StockId = 1,
                UserID = 45
            };
            setup.ObjectMapper
                .Setup(x =>
                x.Map<StockRequestDto, StockRequest>(request))
                .Returns(new StockRequest());

            setup.StockManager.Setup(x => x.Buy(It.IsAny<StockRequest>())).Returns(value: null);

            var StockController = setup.GetController();
            //// act
            var response = StockController.Buy(request);
            Assert.Equal(Constants.UnSuccessful, response.Status);
        }

        [Fact]
        public async void Buy_Throw_Exceptiosn()
        {
            var setup = _fixture.GetNewInstance();
            var request = new StockRequestDto()
            {
                StockId = 1,
                UserID = 45
            };
            setup.ObjectMapper
                .Setup(x =>
                x.Map<StockRequestDto, StockRequest>(request))
                .Returns(new StockRequest());

            setup.StockManager.Setup(x => x.Buy(It.IsAny<StockRequest>())).Throws(new Exception());

            var StockController = setup.GetController();
            //// act
            var response = StockController.Buy(request);
            Assert.Equal(Constants.UnSuccessful, response.Status);
        }

        [Fact]
        public async void Sell_Success()
        {
            var setup = _fixture.GetNewInstance();
            var request = new StockRequestDto()
            {
                StockId = 1,
                Quantity = 2,
                UserID = 45
            };
            setup.ObjectMapper
                .Setup(x =>
                x.Map<StockRequestDto, StockRequest>(request))
                .Returns(new StockRequest());

            setup.StockManager.Setup(x => x.Sell(It.IsAny<StockRequest>())).Returns(new User() { });

            var StockController = setup.GetController();
            //// act
            var response = StockController.Sell(request);
            Assert.Equal(Constants.Successful, response.Status);
        }

        //[Fact]
        //public async void Sell_ModelStateInvalid()
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
        //    var stockController = setup.GetController();

        //    //// act
        //    var response = stockController.TryValidateModel(request);
        //    Assert.False(response);
        //}

        [Fact]
        public async void Sell_Returns_NULL()
        {
            var setup = _fixture.GetNewInstance();
            var request = new StockRequestDto()
            {
                StockId = 1,
                UserID = 45
            };
            setup.ObjectMapper
                .Setup(x =>
                x.Map<StockRequestDto, StockRequest>(request))
                .Returns(new StockRequest());

            setup.StockManager.Setup(x => x.Sell(It.IsAny<StockRequest>())).Returns(value: null);

            var StockController = setup.GetController();
            //// act
            var response = StockController.Sell(request);
            Assert.Equal(Constants.UnSuccessful, response.Status);
        }

        [Fact]
        public async void Sell_Throw_Exceptiosn()
        {
            var setup = _fixture.GetNewInstance();
            var request = new StockRequestDto()
            {
                StockId = 1,
                UserID = 45
            };
            setup.ObjectMapper
                .Setup(x =>
                x.Map<StockRequestDto, StockRequest>(request))
                .Returns(new StockRequest());

            setup.StockManager.Setup(x => x.Sell(It.IsAny<StockRequest>())).Throws(new Exception());

            var StockController = setup.GetController();
            //// act
            var response = StockController.Sell(request);
            Assert.Equal(Constants.UnSuccessful, response.Status);
        }
    }
}
