using Microsoft.EntityFrameworkCore;
using Nagp.UnitTest.EntityFrameworkCore.Model;

using System;
using System.Collections.Generic;
using System.Text;

namespace Nagp.UnitTest.EntityFrameworkCore
{
    public partial class eTraderDBContext : DbContext
    {

        public eTraderDBContext(DbContextOptions options)
        : base(options) {
           // this.LoadUser();
        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }

        public virtual DbSet<HoldingShare> HoldingShare{ get; set; }
    }
}
