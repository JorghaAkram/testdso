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
    public interface IRegulationSKRepo
    {
        Task<BaseDTResponseModel<SKSPJtable>> getSKDatatable(SKSPJtableParam param);
        Task<BaseDTResponseModel<ReportSKSPJtable>> getSKList(string companyCode);
        Task<List<HistoryModel>> getRegulationSKHistory(int id);
        Task<RegulationSKSP> detailSK(int id, CancellationToken cancellationToken);
        Task<ReturnValues> insertSK(SKSPPost param);
        Task<ReturnValues> uploadedSK(SKSPPost param);
        Task<ReturnValues> revisedSK(SKSPPost param);
        Task<ReturnValues> deleteSK(int id);
        Task<ReturnValues> approveSK(SKSPPost param);
        Task<ReturnValues> reviseSK(SKSPPost param);
        Task<ReturnValues> reviewSK(SKSPPost param);
        Task<ReturnValues> needSignSK(SKSPPost param);
        Task<ReturnValues> signedSK(SKSPPost param);
    }
}
