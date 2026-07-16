using Prove.Data.Dao.CorePTK;
using Prove.Data.Dao.Prove;
using Prove.Data.Dao.Usman;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Interface
{
    public interface IMasterDataRepo
    {
        Task<List<ProductOfLawSKSP>> getProductOfLawSKSP(string DocType, string CompanyCode);
        Task<FileUpload> getDataFileUpload(int id);
        Task<List<ProductOfLawSTK>> getProductOfLawSTK(string CompanyCode);
        Task<List<SaveCode>> getSaveCode();
        Task<List<STKType>> getSTKType();
        Task<List<KBO>> getKBO();
        Task<Role> getRolebyPosId(string posId, string companyCode);
        Task<List<EmployeePosition>> getFungsi();
        Task<List<Probis>> getProbis();
        Task<List<TemplateType>> getTemplateType();
    }
}
