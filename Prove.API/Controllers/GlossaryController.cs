using Microsoft.AspNetCore.Mvc;
using Prove.API.Attributes;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao.Prove;
using Prove.Utilities.Base;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prove.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiKey]
    public class GlossaryController : Controller
    {
        private readonly IGlossaryRepo _proveGlossary;

        public GlossaryController(IGlossaryRepo proveGlossary)
        {
            _proveGlossary = proveGlossary;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for retrieving glossary data for datatable)")]
        [Route("GetGlossaryDatatable")]
        public async Task<BaseDTResponseModel<GlossaryJtable>> GetGlossaryDatatable(GlossaryJtableParam param)
        {
            var exec = await _proveGlossary.getGlossaryDatatable(param);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving Glossary detail)")]
        [Route("GetGlossaryDetail/{id}")]
        public async Task<Glossary> GetGlossaryDetail(int id, CancellationToken cancellationToken)
        {
            var exec = await _proveGlossary.detailGlossary(id, cancellationToken);

            return exec;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for insert glossary data)")]
        [Route("InsertGlossary")]
        public async Task<bool> InsertGlossary(GlossaryPost param)
        {
            var exec = await _proveGlossary.insertGlossary(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for update glossary data)")]
        [Route("UpdateGlossary")]
        public async Task<bool> UpdateGlossary(GlossaryPost param)
        {
            var exec = await _proveGlossary.updateGlossary(param);

            return exec;
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "(This method is for delete glossary data)")]
        [Route("DeleteGlossary/{id}")]
        public async Task<bool> DeleteGlossary(int id)
        {
            var exec = await _proveGlossary.deleteGlossary(id);

            return exec;
        }
    }
}
