using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nagp.UnitTest.EntityFrameworkCore.Model
{
    public partial class User
    {
        [Key]
        public int Id { get; set; }

        public string  Firstname{ get; set; }

        public double AvailableAmount { get; set; }

        public virtual List<HoldingShare> HoldingShares { get; set; }
    }
}
