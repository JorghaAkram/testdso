using Prove.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Interface
{
    public interface IUserRepo
    {
        Task<bool> insertUser(UserPost param);
        Task<bool> updateUser(UserPost param);
        Task<bool> deleteUser(string userId);
    }
}
