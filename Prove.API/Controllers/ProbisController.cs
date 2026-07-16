using Microsoft.AspNetCore.Mvc;
using Prove.Data.Services.Prove.Interface;
using Prove.Data.Services.Prove.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProbisController : Controller
    {
        private readonly IProveProbis _proveProbis;

        public ProbisController(IProveProbis proveProbis)
        {
            _proveProbis = proveProbis;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving process business")]
        [Route("GetProbis")]
        public async Task<List<ProbisHierarchy>> GetProbis()
        {
            var exec = await _proveProbis.GetProbisHierarchy();

            return exec;
        }
    }
}
