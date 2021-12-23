using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nagp.UnitTest.EntityFrameworkCore.Model
{
    public class HoldingShare
    {
        [Key]
        public int Id { get; set; }

        public int ShareId { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public int UserId { get; set; }
    }
}
