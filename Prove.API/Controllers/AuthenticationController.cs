using Microsoft.AspNetCore.Mvc;
using Prove.BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationRepo _proveAuth;

        public AuthenticationController(IAuthenticationRepo proveAuth)
        {
            _proveAuth = proveAuth;
        }
    }
}
