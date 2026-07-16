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
    public class RegulationSPController : Controller
    {
        private readonly IRegulationSPRepo _proveSPrin;

        public RegulationSPController(IRegulationSPRepo proveSPrin)
        {
            _proveSPrin = proveSPrin;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for retrieving SPrin data for datatable)")]
        [Route("GetSPDatatable")]
        public async Task<BaseDTResponseModel<SKSPJtable>> GetSPDatatable(SKSPJtableParam param)
        {
            var exec = await _proveSPrin.getSPDatatable(param);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving SPrin List)")]
        [Route("GetSPList/{companyCode}")]
        public async Task<BaseDTResponseModel<ReportSKSPJtable>> GetSPList(string companyCode)
        {
            var exec = await _proveSPrin.getSPList(companyCode);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving SPrin history list)")]
        [Route("GetRegulationSPHistory/{id}")]
        public async Task<List<HistoryModel>> GetRegulationSPHistory(int id)
        {
            var exec = await _proveSPrin.getRegulationSPHistory(id);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving SPrin detail)")]
        [Route("GetRegulationSPDetail/{id}")]
        public async Task<RegulationSKSP> GetRegulationSKDetail(int id, CancellationToken cancellationToken)
        {
            var exec = await _proveSPrin.detailSP(id, cancellationToken);

            return exec;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for insert SPrin)")]
        [Route("InsertSPrin")]
        public async Task<ReturnValues> InsertSPrin([FromForm] SKSPPost param)
        {
            var exec = await _proveSPrin.insertSP(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for uploaded SPrin)")]
        [Route("UploadedSPrin")]
        public async Task<ReturnValues> UploadedSPrin([FromForm] SKSPPost param)
        {
            var exec = await _proveSPrin.uploadedSP(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for revised SPrin)")]
        [Route("RevisedSPrin")]
        public async Task<ReturnValues> RevisedSPrin([FromForm] SKSPPost param)
        {
            var exec = await _proveSPrin.revisedSP(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for approve SPrin)")]
        [Route("ApproveSPrin")]
        public async Task<ReturnValues> ApproveSPrin([FromForm] SKSPPost param)
        {
            var exec = await _proveSPrin.approveSP(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for revise SPrin)")]
        [Route("ReviseSPrin")]
        public async Task<ReturnValues> ReviseSPrin([FromForm] SKSPPost param)
        {
            var exec = await _proveSPrin.reviseSP(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for review SPrin)")]
        [Route("ReviewSPrin")]
        public async Task<ReturnValues> ReviewSPrin([FromForm] SKSPPost param)
        {
            var exec = await _proveSPrin.reviewSP(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for need sign SPrin)")]
        [Route("NeedSignSPrin")]
        public async Task<ReturnValues> NeedSignSPrin([FromForm] SKSPPost param)
        {
            var exec = await _proveSPrin.needSignSP(param);

            return exec;
        }

        [HttpPut]
        [SwaggerOperation(Summary = "(This method is for signed SPrin)")]
        [Route("SignedSPrin")]
        public async Task<ReturnValues> SignedSPrin([FromForm] SKSPPost param)
        {
            var exec = await _proveSPrin.signedSP(param);

            return exec;
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "(This method is for delete SPrin)")]
        [Route("DeleteSPrin/{id}")]
        public async Task<ReturnValues> DeleteSPrin(int id)
        {
            var exec = await _proveSPrin.deleteSP(id);

            return exec;
        }
    }
}
