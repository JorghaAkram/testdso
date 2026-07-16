using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingController : Controller
    {
        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for testing only)")]
        [Route("Testjson")]
        public async Task<string>  Testjson(IFormCollection reqFormData)
        {

            string json = JsonConvert.SerializeObject(reqFormData.ToArray());

            //write string to file
            System.IO.File.WriteAllText(@"D:\testingjson\file.json", json);

            return "success";
        }
    }
}
