using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nagp.UnitTest.Application.Model
{
    public class BaseResponseDto
    {
        public string Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}
