using Irontrax.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Irontrax.Services.Interfaces
{
    public interface IUserManageable
    {
        Task<IrontraxUser> CreateUser(IrontraxUser user);
        Task<IrontraxUser> UpdateUser(IrontraxUser user);
        Task<IrontraxUser> FindById(string id);
        Task<IrontraxUser> FindByUsername(string username);
    }
}
