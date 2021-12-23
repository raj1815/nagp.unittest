using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nagp.UnitTest.Application.Common;
using Nagp.UnitTest.Application.Model;
using Nagp.UnitTest.Business.Exceptions;
using Nagp.UnitTest.Business.Interface;
using Nagp.UnitTest.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nagp.UnitTest.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        #region fields

        private readonly IStockManager _stockManager;
        private readonly IMapper _mapper;

        #endregion

        #region constructors

        public StockController(IStockManager stockManager, IMapper mapper)
        {
            _stockManager = stockManager;
            _mapper = mapper;
        }

        #endregion

        #region methods

        [Route("buy")]
        [HttpPost]
        public BaseResponseDto Buy(StockRequestDto requestDto)
        {
     
            var response = new BaseResponseDto();

            try
            {
                var request = _mapper.Map<StockRequestDto, StockRequest>(requestDto); ;
                var buyResponse = _stockManager.Buy(request);
                if (buyResponse == null)
                {
                    response.Status = Constants.UnSuccessful;
                    response.ErrorMessage = Constants.ErrorMessage;
                }
                else
                {
                    response.Status = Constants.Successful;
                }

            }
            catch (BusinessException ex)
            {
                response.Status = Constants.UnSuccessful;
                response.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                response.Status = Constants.UnSuccessful;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        [Route("Sell")]
        [HttpPost]
        public BaseResponseDto Sell(StockRequestDto requestDto)
        {
            var response = new BaseResponseDto();
            try
            {
                var request = _mapper.Map<StockRequestDto, StockRequest>(requestDto); ;
                var sellResponse = _stockManager.Sell(request);
                if (sellResponse == null)
                {
                    response.Status = Constants.UnSuccessful;
                    response.ErrorMessage = Constants.ErrorMessage;
                }
                else
                {
                    response.Status = Constants.Successful;
                }

            }
            catch (BusinessException ex)
            {
                response.Status = Constants.UnSuccessful;
                response.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                response.Status = Constants.UnSuccessful;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        #endregion
    }
}
