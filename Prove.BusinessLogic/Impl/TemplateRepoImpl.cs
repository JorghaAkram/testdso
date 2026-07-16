using Microsoft.EntityFrameworkCore;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao.Prove;
using Prove.Data.Data;
using Prove.Data.Services.FileUpload.Interface;
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
    public class TemplateRepoImpl : ITemplateRepo
    {
        private readonly ProveExtContext _proveContext;
        private readonly IFileUploadRepo _fileUploadService;

        public TemplateRepoImpl(IFileUploadRepo fileUploadService, ProveExtContext proveContext)
        {
            _fileUploadService = fileUploadService;
            _proveContext = proveContext;
        }

        public async Task<BaseDTResponseModel<TemplateJtable>> getTemplateDatatable(TemplateJtableParam param)
        {
            try
            {
                IQueryable<TemplateJtable> itemData = null;
                List<TemplateJtable> filteredData = new List<TemplateJtable>();

                IQueryable<Template> allItemData = _proveContext.Template.Include(a => a.Type)
                                                              .Where(b => b.IsActive == GeneralConstant.YES
                                                                       && b.IsDeleted == GeneralConstant.NO
                                                                       && b.CompanyCode == param.companyCode)
                                                              .OrderByDescending(c => c.CreatedAt);

                if (param.PageSize < 1)
                    return new BaseDTResponseModel<TemplateJtable>
                    {
                        Data = new List<TemplateJtable>(),
                        Draw = param.Draw,
                        RecordsFiltered = 0,
                        RecordsTotal = allItemData.Count()
                    };

                itemData = (from a in allItemData
                            where (string.IsNullOrWhiteSpace(param.namaFile) || a.FileName.ToLower().Contains(param.namaFile))
                               && (param.tipeDokumen == 0 || a.Type.Id == param.tipeDokumen)
                            select new TemplateJtable
                            {
                                Id = a.Id,
                                namaFile = a.FileName,
                                pengunggah = a.CreatedBy,
                                fileID = a.FileId,
                                status = a.IsActive == "Y" ? "Aktif" : "Tidak Aktif",
                                tglUnggah = a.CreatedAt,
                                tipeDokumen = a.Type.Description,
                                tipeDokumenId = a.Type.Id,
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

                BaseDTResponseModel<TemplateJtable> result = new BaseDTResponseModel<TemplateJtable>
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

        public async Task<Template> detailTemplate(int id, CancellationToken cancellationToken)
            => await _proveContext.Template.Include(a => a.Type).FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        public async Task<bool> insertTemplate(TemplatePost param)
        {
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    Template templateData = new Template();
                    TemplateType templateTypeData = new TemplateType();

                    templateTypeData = await _proveContext.TemplateType.Where(a => a.Id == Convert.ToInt32(param.typeDoc)).FirstOrDefaultAsync();

                    FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                       path: $"STK/Template/" + templateTypeData.Description + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Second + "/file/",
                       createBy: param.username,
                       trxId: Convert.ToInt32(param.typeDoc),
                       file: param.file,
                       Flag: "Template_STK",
                       autoRename: true,
                       isUpdate: true,
                       remark: "Template_STK");
                    FileUpload data = FileData;

                    templateData.FileId = FileData.Id;
                    templateData.FileName = string.IsNullOrWhiteSpace(param.docName) ? FileData.FileName : param.docName;
                    templateData.FileType = FileData.ContentType;
                    templateData.Type = templateTypeData;
                    templateData.CompanyCode = param.companyCode;
                    templateData.CreatedBy = param.username;
                    templateData.UpdatedBy = param.username;

                    await _proveContext.Template.AddAsync(templateData);
                    await _proveContext.SaveChangesAsync();

                    #region log activity

                    logActivity.Info = "Success Transaction Insert";
                    logActivity.Method = "insertTemplate";
                    logActivity.Remark = "STK BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "STKTemplateRepoImpl";
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
                logActivity.Method = "insertTemplate";
                logActivity.Remark = "STK Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "STKTemplateRepoImpl";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                return false;
            }
        }

        public async Task<bool> updateTemplate(TemplatePost param)
        {
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    Template templateData = new Template();
                    TemplateType templateTypeData = new TemplateType();

                    templateTypeData = await _proveContext.TemplateType.Where(a => a.Id == Convert.ToInt32(param.typeDoc)).FirstOrDefaultAsync();

                    templateData = await _proveContext.Template.Where(a => a.Id == Convert.ToInt32(param.id)).FirstOrDefaultAsync();

                    if (param.file == null)
                    {
                        templateData.FileName = param.docName;
                        templateData.Type = templateTypeData;
                        templateData.UpdatedAt = DateTime.Now;
                        templateData.UpdatedBy = param.username;

                        _proveContext.Template.Update(templateData);
                        await _proveContext.SaveChangesAsync();
                    }
                    else
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/Template/" + templateTypeData.Description + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Second + "/file/",
                           createBy: param.username,
                           trxId: Convert.ToInt32(param.typeDoc),
                           file: param.file,
                           Flag: "Template_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "Template_STK");
                        FileUpload data = FileData;

                        templateData.FileId = FileData.Id;
                        templateData.FileName = string.IsNullOrWhiteSpace(param.docName) ? FileData.FileName : param.docName;
                        templateData.FileType = FileData.ContentType;
                        templateData.Type = templateTypeData;
                        templateData.CreatedBy = param.username;
                        templateData.UpdatedBy = param.username;

                        _proveContext.Template.Update(templateData);
                        await _proveContext.SaveChangesAsync();
                    }

                    #region log activity

                    logActivity.Info = "Success Transaction Update";
                    logActivity.Method = "updateTemplate";
                    logActivity.Remark = "STK BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "STKTemplateRepoImpl";
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
                logActivity.Method = "updateTemplate";
                logActivity.Remark = "STK Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "STKTemplateRepoImpl";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                _proveContext.ActivityLog.Add(logActivity);
                _proveContext.SaveChanges();

                #endregion

                return false;
            }
        }

        public async Task<bool> deleteTemplate(int id)
        {
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    Template templateData = new Template();

                    templateData = await _proveContext.Template.Where(a => a.Id == id).FirstOrDefaultAsync();

                    templateData.IsActive = GeneralConstant.NO;
                    templateData.IsDeleted = GeneralConstant.YES;
                    templateData.UpdatedAt = DateTime.Now;
                    templateData.UpdatedBy = "SYSTEM";

                    _proveContext.Template.Update(templateData);
                    await _proveContext.SaveChangesAsync();

                    #region log activity

                    logActivity.Info = "Success Transaction Delete";
                    logActivity.Method = "deleteTemplate";
                    logActivity.Remark = "STK BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = "SYSTEM";
                    logActivity.Controller = "STKTemplateRepoImpl";
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
                logActivity.Method = "deleteTemplate";
                logActivity.Remark = "STK Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "STKTemplateRepoImpl";
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
