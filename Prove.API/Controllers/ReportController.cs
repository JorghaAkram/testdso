using Microsoft.AspNetCore.Mvc;
using Prove.API.Attributes;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Utilities.Base;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiKey]
    public class ReportController : Controller
    {
        private readonly IReportRepo _proveReport;

        public ReportController(IReportRepo proveReport)
        {
            _proveReport = proveReport;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for retrieving SKpts and SPrin for datatable)")]
        [Route("GetSKSPDatatable")]
        public async Task<BaseDTResponseModel<ReportSKSPJtable>> GetSKSPDatatable(ReportSKSPJtableParam param)
        {
            var exec = await _proveReport.getSKSPDatatable(param);

            return exec;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for retrieving STK for datatable)")]
        [Route("GetSTKDatatable")]
        public async Task<BaseDTResponseModel<ReportSTKJtable>> GetSTKDatatable(ReportSTKJtableParam param)
        {
            var exec = await _proveReport.getSTKDatatable(param);

            return exec;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for retrieving list STK)")]
        [Route("GetSTKList")]
        public async Task<BaseDTResponseModel<ReportSTKJtable>> GetSTKList(ReportSTKJtableParam param)
        {
            var exec = await _proveReport.getSTKList(param);

            return exec;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "(This method is for retrieving list SKSP)")]
        [Route("GetSKSPList")]
        public async Task<BaseDTResponseModel<ReportSKSPJtable>> GetSKSPList(ReportSKSPJtableParam param)
        {
            var exec = await _proveReport.getSKSPList(param);

            return exec;
        }
    }
}
