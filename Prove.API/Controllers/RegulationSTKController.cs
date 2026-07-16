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
    public class RegulationSTKController : Controller
    {
        private readonly IRegulationSTKRepo _proveSTK;

        public RegulationSTKController(IRegulationSTKRepo proveSTK)
        {
            _proveSTK = proveSTK;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for retrieving STK data for datatable)")]
        [Route("GetSTKDatatable")]
        public async Task<BaseDTResponseModel<STKJtable>> GetSTKDatatable(STKJtableParam param)
        {
            var exec = await _proveSTK.getSTKDatatable(param);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving STK List)")]
        [Route("GetSTKList/{companyCode}")]
        public async Task<BaseDTResponseModel<ReportSTKJtable>> GetSTKList(string companyCode)
        {
            var exec = await _proveSTK.getSTKList(companyCode);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving STK history list)")]
        [Route("GetRegulationSTKHistory/{id}")]
        public async Task<List<HistoryModel>> GetRegulationSTKHistory(int id)
        {
            var exec = await _proveSTK.getRegulationSTKHistory(id);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving STK detail)")]
        [Route("GetRegulationSTKDetail/{id}")]
        public async Task<RegulationSTK> GetRegulationSTKDetail(int id, CancellationToken cancellationToken)
        {
            var exec = await _proveSTK.detailSTK(id, cancellationToken);

            return exec;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for insert STK)")]
        [Route("InsertSTK")]
        public async Task<ReturnValues> InsertSTK([FromForm] STKPost param)
        {
            var exec = await _proveSTK.insertSTK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for uploaded STK)")]
        [Route("UploadedSTK")]
        public async Task<ReturnValues> UploadedSTK([FromForm] STKPost param)
        {
            var exec = await _proveSTK.uploadedSTK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for revised STK)")]
        [Route("RevisedSTK")]
        public async Task<ReturnValues> RevisedSTK([FromForm] STKPost param)
        {
            var exec = await _proveSTK.revisedSTK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for approve STK)")]
        [Route("ApproveSTK")]
        public async Task<ReturnValues> ApproveSTK([FromForm] STKPost param)
        {
            var exec = await _proveSTK.approveSTK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for revise STK)")]
        [Route("ReviseSTK")]
        public async Task<ReturnValues> ReviseSTK([FromForm] STKPost param)
        {
            var exec = await _proveSTK.reviseSTK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for review STK)")]
        [Route("ReviewSTK")]
        public async Task<ReturnValues> ReviewSTK([FromForm] STKPost param)
        {
            var exec = await _proveSTK.reviewSTK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for need sign STK)")]
        [Route("NeedSignSTK")]
        public async Task<ReturnValues> NeedSignSTK([FromForm] STKPost param)
        {
            var exec = await _proveSTK.needSignSTK(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for signed STK)")]
        [Route("SignedSTK")]
        public async Task<ReturnValues> SignedSTK([FromForm] STKPost param)
        {
            var exec = await _proveSTK.signedSTK(param);

            return exec;
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "(This method is for delete STK)")]
        [Route("DeleteSTK/{id}")]
        public async Task<ReturnValues> DeleteSTK(int id)
        {
            var exec = await _proveSTK.deleteSTK(id);

            return exec;
        }
    }
}
