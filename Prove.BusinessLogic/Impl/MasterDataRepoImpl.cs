using Microsoft.EntityFrameworkCore;
using Prove.BusinessLogic.Interface;
using Prove.Data.Dao.CorePTK;
using Prove.Data.Dao.Prove;
using Prove.Data.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Prove.Data.Dao.Usman;
using Prove.Utilities.Constants;

namespace Prove.BusinessLogic.Impl
{
    public class MasterDataRepoImpl : IMasterDataRepo
    {
        private readonly ProveExtContext _proveContext;
        private readonly CorePTKContext _coreContext;
        private readonly UsmanContext _usmanContext;

        public MasterDataRepoImpl(CorePTKContext coreContext, ProveExtContext proveContext, UsmanContext usmanContext)
        {
            _coreContext = coreContext;
            _proveContext = proveContext;
            _usmanContext = usmanContext;
        }

        public async Task<List<ProductOfLawSKSP>> getProductOfLawSKSP(string DocType, string CompanyCode)
        {
            var data = await (from a in _proveContext.ProductOfLawSKSP
                              where a.CompanyCode == CompanyCode
                              select new ProductOfLawSKSP
                              {
                                  Id = a.Id,
                                  CreatedAt = a.CreatedAt,
                                  CreatedBy = a.CreatedBy,
                                  Description = DocType == "SK" ? "Kpts " + a.Description : "Prin " + a.Description,
                                  IsActive = a.IsActive,
                                  IsDeleted = a.IsDeleted,
                                  KBO = a.KBO,
                                  PositionNumber = a.PositionNumber,
                                  UpdatedAt = a.UpdatedAt,
                                  UpdatedBy = a.UpdatedBy
                              }).ToListAsync();

            return data;
        }

        public async Task<List<ProductOfLawSTK>> getProductOfLawSTK(string CompanyCode)
        {
            return await _proveContext.ProductOfLawSTK.Include(a => a.Type).Where(b => b.CompanyCode == CompanyCode).ToListAsync();
        }

        public async Task<List<SaveCode>> getSaveCode()
        {
            return await _proveContext.SaveCode.ToListAsync();
        }

        public async Task<List<STKType>> getSTKType()
        {
            return await _proveContext.STKType.ToListAsync();
        }

        public async Task<List<KBO>> getKBO()
        {
            return await _coreContext.KBO.Include(a => a.EmployeePosition).ToListAsync();
        }

        public async Task<Role> getRolebyPosId(string posId, string companyCode)
        {
            return await _usmanContext.UserRoles.Include(a => a.PositionUser)
                                                .Include(b => b.Role)
                                                .ThenInclude(c => c.Application).Where(d => d.PositionUser.PositionId == posId
                                                                                         && d.Role.Application.ApplicationName == "PROVE"
                                                                                         && d.PositionUser.CompanyCode == companyCode
                                                                                         && d.IsActive == GeneralConstant.YES
                                                                                         && d.IsDeleted == GeneralConstant.NO).Select(e => e.Role).FirstOrDefaultAsync();
        }

        public async Task<List<EmployeePosition>> getFungsi()
        {
            return await _coreContext.EmployeePositions.Include(a => a.EmployeePositionAbbrv)
                                                       .Include(b => b.Organization)
                                                       .Include(c => c.PAreaSub)
                                                       .ToListAsync();
        }

        public async Task<List<Probis>> getProbis()
        {
            return await _proveContext.Probis.ToListAsync();
        }

        public async Task<List<TemplateType>> getTemplateType()
        {
            return await _proveContext.TemplateType.ToListAsync();
        }

        public async Task<FileUpload> getDataFileUpload(int id) => await _proveContext.FileUpload.FirstOrDefaultAsync(a => a.Id == id);
    }
}
