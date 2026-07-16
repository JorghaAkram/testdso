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
    public class RegulationSKController : Controller
    {
        private readonly IRegulationSKRepo _proveSKpts;

        public RegulationSKController(IRegulationSKRepo proveSKpts)
        {
            _proveSKpts = proveSKpts;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for retrieving SKpts data for datatable)")]
        [Route("GetSKDatatable")]
        public async Task<BaseDTResponseModel<SKSPJtable>> GetSKDatatable(SKSPJtableParam param)
        {
            var exec = await _proveSKpts.getSKDatatable(param);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving SKpts List)")]
        [Route("GetSKList/{companyCode}")]
        public async Task<BaseDTResponseModel<ReportSKSPJtable>> GetSKList(string companyCode)
        {
            var exec = await _proveSKpts.getSKList(companyCode);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving SKpts history list)")]
        [Route("GetRegulationSKHistory/{id}")]
        public async Task<List<HistoryModel>> GetRegulationSKHistory(int id)
        {
            var exec = await _proveSKpts.getRegulationSKHistory(id);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving SKpts detail)")]
        [Route("GetRegulationSKDetail/{id}")]
        public async Task<RegulationSKSP> GetRegulationSKDetail(int id, CancellationToken cancellationToken)
        {
            var exec = await _proveSKpts.detailSK(id, cancellationToken);

            return exec;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for insert SKpts)")]
        [Route("InsertSKpts")]
        public async Task<ReturnValues> InsertSKpts([FromForm] SKSPPost param)
        {
            var exec = await _proveSKpts.insertSK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for uploaded SKpts)")]
        [Route("UploadedSKpts")]
        public async Task<ReturnValues> UploadedSKpts([FromForm] SKSPPost param)
        {
            var exec = await _proveSKpts.uploadedSK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for revised SKpts)")]
        [Route("RevisedSKpts")]
        public async Task<ReturnValues> RevisedSKpts([FromForm] SKSPPost param)
        {
            var exec = await _proveSKpts.revisedSK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for approve SKpts)")]
        [Route("ApproveSKpts")]
        public async Task<ReturnValues> ApproveSKpts([FromForm] SKSPPost param)
        {
            var exec = await _proveSKpts.approveSK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for revise SKpts)")]
        [Route("ReviseSKpts")]
        public async Task<ReturnValues> ReviseSKpts([FromForm] SKSPPost param)
        {
            var exec = await _proveSKpts.reviseSK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for review SKpts)")]
        [Route("ReviewSKpts")]
        public async Task<ReturnValues> ReviewSKpts([FromForm] SKSPPost param)
        {
            var exec = await _proveSKpts.reviewSK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for need sign SKpts)")]
        [Route("NeedSignSKpts")]
        public async Task<ReturnValues> NeedSignSKpts([FromForm] SKSPPost param)
        {
            var exec = await _proveSKpts.needSignSK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for signed SKpts)")]
        [Route("SignedSKpts")]
        public async Task<ReturnValues> SignedSKpts([FromForm] SKSPPost param)
        {
            var exec = await _proveSKpts.signedSK(param);

            return exec;
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "(This method is for delete SKpts)")]
        [Route("DeleteSKpts/{id}")]
        public async Task<ReturnValues> DeleteSKpts(int id)
        {
            var exec = await _proveSKpts.deleteSK(id);

            return exec;
        }
    }
}
