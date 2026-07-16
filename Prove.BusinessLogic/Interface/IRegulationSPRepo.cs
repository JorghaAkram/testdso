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
    public interface IRegulationSPRepo
    {
        Task<BaseDTResponseModel<SKSPJtable>> getSPDatatable(SKSPJtableParam param);
        Task<BaseDTResponseModel<ReportSKSPJtable>> getSPList(string companyCode);
        Task<List<HistoryModel>> getRegulationSPHistory(int id);
        Task<RegulationSKSP> detailSP(int id, CancellationToken cancellationToken);
        Task<ReturnValues> insertSP(SKSPPost param);
        Task<ReturnValues> uploadedSP(SKSPPost param);
        Task<ReturnValues> revisedSP(SKSPPost param);
        Task<ReturnValues> deleteSP(int id);
        Task<ReturnValues> approveSP(SKSPPost param);
        Task<ReturnValues> reviseSP(SKSPPost param);
        Task<ReturnValues> reviewSP(SKSPPost param);
        Task<ReturnValues> needSignSP(SKSPPost param);
        Task<ReturnValues> signedSP(SKSPPost param);
    }
}
