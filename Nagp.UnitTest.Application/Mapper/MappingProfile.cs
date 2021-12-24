using AutoMapper;
using Nagp.UnitTest.Application.Model;
using Nagp.UnitTest.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nagp.UnitTest.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StockRequestDto, StockRequest>();
            CreateMap<FundRequestDto, FundRequest>();
        }
    }
}
