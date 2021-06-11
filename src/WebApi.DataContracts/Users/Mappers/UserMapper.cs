using Domain.Entities;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DataContracts.Users.Mappers
{
    public static class UserMapper
    {
        public static User MapDataContractToEntity(UserDataContract dc)
        {
            return new User
            {
                Username = dc.Username,
                Name = dc.Name,
                PasswordHash = dc.Password.Hash(dc.Username)
            };
        }
        public static UserDataContract MapEntityToDataContract(User user)
        {
            return new UserDataContract
            {
                Username = user.Username,
                Password = user.PasswordHash,
                Name = user.Name
            };
        }
    }
}
