using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nagp.UnitTest.EntityFrameworkCore.Model
{
    public partial class Stock
    {
        [Key]
        public int Id { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }
    }
}
