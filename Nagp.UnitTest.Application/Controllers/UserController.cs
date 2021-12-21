using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nagp.UnitTest.Application.Common;
using Nagp.UnitTest.Application.Model;
using Nagp.UnitTest.Business.Interface;
using Nagp.UnitTest.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nagp.UnitTest.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region fields

        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        #endregion

        #region constructors

        public UserController(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        #endregion

        #region methods

        [Route("addfund")]
        [HttpPost]
        public BaseResponseDto AddFunds(FundRequestDto requestDto)
        {
            var response = new BaseResponseDto();
            try
            {
                var request = _mapper.Map<FundRequestDto, FundRequest>(requestDto); ;
                var sellResponse = _userManager.AddFunds(request);
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
