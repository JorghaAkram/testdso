using Microsoft.AspNetCore.Mvc;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepo _proveUser;

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for insert user data)")]
        [Route("InsertUser")]
        public async Task<bool> InsertUser(UserPost param)
        {
            var exec = await _proveUser.insertUser(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for update user data)")]
        [Route("UpdateUser")]
        public async Task<bool> UpdateUser(UserPost param)
        {
            var exec = await _proveUser.updateUser(param);

            return exec;
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "(This method is for delete user data)")]
        [Route("DeleteUser/{userId}")]
        public async Task<bool> DeleteUser(string userId)
        {
            var exec = await _proveUser.deleteUser(userId);

            return exec;
        }
    }
}
