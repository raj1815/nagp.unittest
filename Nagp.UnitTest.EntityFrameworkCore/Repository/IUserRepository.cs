using Nagp.UnitTest.EntityFrameworkCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nagp.UnitTest.EntityFrameworkCore.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        User GetById(int id);
    }
}
