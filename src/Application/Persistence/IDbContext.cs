using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IDbContext
    {
        User Register(User user);
        User GetUserByUsernameAndPassword(string username, string password);
        User GetUserById(Guid id);
    }
}
