using Microsoft.AspNetCore.Mvc;
using Prove.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Interface
{
    public interface IAuthenticationRepo
    {
        Task<IActionResult> Login(LoginModel param);
    }
}
