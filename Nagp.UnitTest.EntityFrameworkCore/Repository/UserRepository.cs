﻿using Nagp.UnitTest.EntityFrameworkCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nagp.UnitTest.EntityFrameworkCore.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(eTraderDBContext _context) :base(_context)
        {
            

        }
        public User GetById(int id)
        {
            var record = base.GetById(id);
            var holdingShare = base._context.HoldingShare.Where(h => h.UserId == id).ToList();
            record.HoldingShares = holdingShare;
            return record;
        }
    }
}
