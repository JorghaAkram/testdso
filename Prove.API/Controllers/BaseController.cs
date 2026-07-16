using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Prove.API.Controllers
{
    public class BaseController : ControllerBase
    {
        //public void DecodeToken(string token)
        //{
        //    var handler = new JwtSecurityTokenHandler().ValidateToken(token,new TokenValidationParameters {
        //    IssuerSigningKey = 
        //    });
        //}

        protected string GetUserName() => User.Claims.FirstOrDefault(b => b.Type == ClaimTypes.Name).Value;}
}
