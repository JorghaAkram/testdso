using Prove.BusinessLogic.Models;
using Prove.Data.Dao.Prove;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Interface
{
    public interface IRegulationSTKRepo
    {
        Task<BaseDTResponseModel<STKJtable>> getSTKDatatable(STKJtableParam param);
        Task<BaseDTResponseModel<ReportSTKJtable>> getSTKList(string CompanyCode);
        Task<List<HistoryModel>> getRegulationSTKHistory(int id);
        Task<RegulationSTK> detailSTK(int id, CancellationToken cancellationToken);
        Task<ReturnValues> insertSTK(STKPost param);
        Task<ReturnValues> uploadedSTK(STKPost param);
        Task<ReturnValues> revisedSTK(STKPost param);
        Task<ReturnValues> deleteSTK(int id);
        Task<ReturnValues> approveSTK(STKPost param);
        Task<ReturnValues> reviseSTK(STKPost param);
        Task<ReturnValues> reviewSTK(STKPost param);
        Task<ReturnValues> needSignSTK(STKPost param);
        Task<ReturnValues> signedSTK(STKPost param);
    }
}
