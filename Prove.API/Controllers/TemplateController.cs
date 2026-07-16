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
    public class TemplateController : Controller
    {
        private readonly ITemplateRepo _proveTemplate;

        public TemplateController(ITemplateRepo proveTemplate)
        {
            _proveTemplate = proveTemplate;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for retrieving template data for datatable)")]
        [Route("GetTemplateDatatable")]
        public async Task<BaseDTResponseModel<TemplateJtable>> GetTemplateDatatable(TemplateJtableParam param)
        {
            var exec = await _proveTemplate.getTemplateDatatable(param);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving Template detail)")]
        [Route("GetTemplateDetail/{id}")]
        public async Task<Template> GetTemplateDetail(int id, CancellationToken cancellationToken)
        {
            var exec = await _proveTemplate.detailTemplate(id, cancellationToken);

            return exec;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for insert template data)")]
        [Route("InsertTemplate")]
        public async Task<bool> InsertTemplate([FromForm]TemplatePost param)
        {
            var exec = await _proveTemplate.insertTemplate(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for update template data)")]
        [Route("UpdateTemplate")]
        public async Task<bool> UpdateTemplate([FromForm]TemplatePost param)
        {
            var exec = await _proveTemplate.updateTemplate(param);

            return exec;
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "(This method is for delete template data)")]
        [Route("DeleteTemplate/{id}")]
        public async Task<bool> DeleteTemplate(int id)
        {
            var exec = await _proveTemplate.deleteTemplate(id);

            return exec;
        }
    }
}
