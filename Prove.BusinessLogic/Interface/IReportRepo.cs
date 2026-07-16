using Prove.BusinessLogic.Models;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Interface
{
    public interface IReportRepo
    {
        Task<BaseDTResponseModel<ReportSKSPJtable>> getSKSPDatatable(ReportSKSPJtableParam param);
        Task<BaseDTResponseModel<ReportSTKJtable>> getSTKDatatable(ReportSTKJtableParam param);
        Task<BaseDTResponseModel<ReportSTKJtable>> getSTKList(ReportSTKJtableParam param);
        Task<BaseDTResponseModel<ReportSKSPJtable>> getSKSPList(ReportSKSPJtableParam param);
    }
}
