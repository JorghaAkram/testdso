using Microsoft.EntityFrameworkCore;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao.Prove;
using Prove.Data.Data;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Impl
{
    public class GlossaryRepoImpl : IGlossaryRepo
    {
        private readonly CorePTKContext _coreContext;
        private readonly ProveExtContext _proveContext;

        public GlossaryRepoImpl(ProveExtContext proveContext, CorePTKContext coreContext)
        {
            _proveContext = proveContext;
            _coreContext = coreContext;
        }

        public async Task<BaseDTResponseModel<GlossaryJtable>> getGlossaryDatatable(GlossaryJtableParam param)
        {
            try
            {
                IQueryable<GlossaryJtable> itemData = null;
                List<GlossaryJtable> filteredData = new List<GlossaryJtable>();

                IQueryable<Glossary> allItemData = _proveContext.Glossary.Where(a => a.IsActive == GeneralConstant.YES
                                                                                  && a.IsDeleted == GeneralConstant.NO
                                                                                  && a.CompanyCode == param.companyCode)
                                                                         .OrderByDescending(b => b.CreatedAt);

                if (param.PageSize < 1)
                    return new BaseDTResponseModel<GlossaryJtable>
                    {
                        Data = new List<GlossaryJtable>(),
                        Draw = param.Draw,
                        RecordsFiltered = 0,
                        RecordsTotal = allItemData.Count()
                    };

                itemData = (from a in allItemData
                            where (string.IsNullOrWhiteSpace(param.istilah) || a.Term.ToLower().Contains(param.istilah))
                               && (string.IsNullOrWhiteSpace(param.pengertian) || a.Description.ToLower().Contains(param.pengertian))
                               && (string.IsNullOrWhiteSpace(param.referensi) || a.Reference.ToLower().Contains(param.referensi))
                            select new GlossaryJtable
                            {
                                Id = a.Id,
                                istilah = a.Term,
                                pengertian = a.Description,
                                referensi = a.Reference,
                                CreatedDate = a.CreatedAt
                            });

                if (string.IsNullOrWhiteSpace(param.ColumnIndex))
                {
                    filteredData = await itemData.OrderByDescending(b => b.CreatedDate).Skip(param.Skip).Take(param.PageSize).ToListAsync();
                }
                else
                {
                    if (param.SortDirection == "desc")
                        filteredData = await itemData.OrderByDescending(b => b.GetType().GetProperty(param.ColumnIndex).GetValue(b)).Skip(param.Skip).Take(param.PageSize).ToListAsync();
                    else
                        filteredData = await itemData.OrderBy(b => b.GetType().GetProperty(param.ColumnIndex).GetValue(b)).Skip(param.Skip).Take(param.PageSize).ToListAsync();
                }

                BaseDTResponseModel<GlossaryJtable> result = new BaseDTResponseModel<GlossaryJtable>
                {
                    Data = filteredData,
                    Draw = param.Draw,
                    RecordsFiltered = itemData.Count(),
                    RecordsTotal = allItemData.Count()
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Glossary> detailGlossary(int id, CancellationToken cancellationToken)
            => await _proveContext.Glossary.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        public async Task<bool> insertGlossary(GlossaryPost param)
        {
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    Glossary glossaryData = new Glossary();

                    glossaryData.Reference = param.referensi;
                    glossaryData.Term = param.istilah;
                    glossaryData.Description = param.desc;
                    glossaryData.CompanyCode = param.companyCode;
                    glossaryData.CreatedBy = param.username;
                    glossaryData.UpdatedBy = param.username;

                    await _proveContext.Glossary.AddAsync(glossaryData);
                    await _proveContext.SaveChangesAsync();

                    #region log activity

                    logActivity.Info = "Success Transaction Insert";
                    logActivity.Method = "insertGlossary";
                    logActivity.Remark = "Prove BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVE";
                    logActivity.ErrorMessage = "-";
                    logActivity.CreatedAt = DateTime.Now;

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Insert";
                logActivity.Method = "insertGlossary";
                logActivity.Remark = "Prove BusinessLogic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVE";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                _proveContext.ActivityLog.Add(logActivity);
                _proveContext.SaveChanges();

                #endregion

                return false;
            }
        }

        public async Task<bool> updateGlossary(GlossaryPost param)
        {
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    Glossary glossaryData = new Glossary();

                    glossaryData = await _proveContext.Glossary.Where(a => a.Id == param.id).FirstOrDefaultAsync();

                    glossaryData.Reference = param.referensi;
                    glossaryData.Term = param.istilah;
                    glossaryData.Description = param.desc;
                    glossaryData.UpdatedAt = DateTime.Now;
                    glossaryData.UpdatedBy = param.username;

                    _proveContext.Glossary.Update(glossaryData);
                    await _proveContext.SaveChangesAsync();

                    #region log activity

                    logActivity.Info = "Success Transaction Update";
                    logActivity.Method = "updateGlossary";
                    logActivity.Remark = "Prove BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVE";
                    logActivity.ErrorMessage = "-";
                    logActivity.CreatedAt = DateTime.Now;

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Update";
                logActivity.Method = "updateGlossary";
                logActivity.Remark = "Prove Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVE";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                return false;
            }
        }

        public async Task<bool> deleteGlossary(int id)
        {
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    Glossary glossaryData = new Glossary();

                    glossaryData = await _proveContext.Glossary.Where(a => a.Id == id).FirstOrDefaultAsync();

                    glossaryData.IsActive = GeneralConstant.NO;
                    glossaryData.IsDeleted = GeneralConstant.YES;
                    glossaryData.UpdatedAt = DateTime.Now;
                    glossaryData.UpdatedBy = "SYSTEM";

                    _proveContext.Glossary.Update(glossaryData);
                    await _proveContext.SaveChangesAsync();

                    #region log activity

                    logActivity.Info = "Success Transaction Delete";
                    logActivity.Method = "deleteGlossary";
                    logActivity.Remark = "Prove BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = "SYSTEM";
                    logActivity.Controller = "PROVE";
                    logActivity.ErrorMessage = "-";
                    logActivity.CreatedAt = DateTime.Now;

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Delete";
                logActivity.Method = "deleteGlossary";
                logActivity.Remark = "Prove Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "PROVE";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                return false;
            }
        }
    }
}
