using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao;
using Prove.Data.Dao.Prove;
using Prove.Data.Data;
using Prove.Data.Services.SendMail.Interface;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Prove.BusinessLogic.Models.WhitelistModel;

namespace Prove.BusinessLogic.Impl
{
    public class RegulationSPRepoImpl : IRegulationSPRepo
    {
        private readonly ProveExtContext _proveContext;
        private readonly UsmanContext _usmanContext;
        private readonly IFileUploadRepo _fileUploadService;
        private readonly ISendMail _mailService;
        private readonly IHttpClientFactory _httpClient;

        public RegulationSPRepoImpl(ProveExtContext proveContext, IFileUploadRepo fileUploadService, ISendMail mailService, IHttpClientFactory httpClient, UsmanContext usmanContext)
        {
            _proveContext = proveContext;
            _fileUploadService = fileUploadService;
            _mailService = mailService;
            _httpClient = httpClient;
            _usmanContext = usmanContext;
        }

        public async Task<BaseDTResponseModel<SKSPJtable>> getSPDatatable(SKSPJtableParam param)
        {
            try
            {
                IQueryable<SKSPJtable> itemData = null;
                List<SKSPJtable> filteredData = new List<SKSPJtable>();
                IQueryable<RegulationSKSP> allItemData = null;

                if (param.isAdmin == "true")
                {
                    allItemData = _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                                    .Include(b => b.SaveCode)
                                                                    .Where(c => c.IsActive == GeneralConstant.YES
                                                                             && c.IsDeleted == GeneralConstant.NO
                                                                             && c.TypeId == TypeSurat.Perintah
                                                                             && c.CompanyCode == param.companyCode)
                                                                    .OrderByDescending(d => d.CreatedAt);
                }
                else
                {
                    //if (param.conceptorId == param.conceptorDirId)
                    //{
                    //    allItemData = _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                    //                                                    .Include(b => b.SaveCode)
                    //                                                    .Where(c => c.IsActive == GeneralConstant.YES
                    //                                                             && c.IsDeleted == GeneralConstant.NO
                    //                                                             && c.TypeId == TypeSurat.Perintah
                    //                                                             && (param.conceptorDivId == "0" ? c.ConceptorDirId == param.conceptorDirId : c.ConceptorDivId == param.conceptorDivId)
                    //                                                             && c.CompanyCode == param.companyCode)
                    //                                                    .OrderByDescending(d => d.CreatedAt);
                    //}
                    //else
                    //{
                        allItemData = _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                                        .Include(b => b.SaveCode)
                                                                        .Where(c => c.IsActive == GeneralConstant.YES
                                                                                 && c.IsDeleted == GeneralConstant.NO
                                                                                 && c.TypeId == TypeSurat.Perintah
                                                                                 && (c.ConceptorDirId == param.conceptorDirId)
                                                                                 && c.CompanyCode == param.companyCode)
                                                                        .OrderByDescending(d => d.CreatedAt);
                    //}

                }

                if (param.PageSize < 1)
                    return new BaseDTResponseModel<SKSPJtable>
                    {
                        Data = new List<SKSPJtable>(),
                        Draw = param.Draw,
                        RecordsFiltered = 0,
                        RecordsTotal = allItemData.Count()
                    };

                itemData = (from a in allItemData
                            where (string.IsNullOrWhiteSpace(param.nomor) || a.SKSPNumber.ToLower().Contains(param.nomor.ToLower()))
                               && (string.IsNullOrWhiteSpace(param.perihal) || (a.Title != null ? a.Title.ToLower().Contains(param.perihal.ToLower()) : a.Title == param.perihal))
                               && (param.produkHukum == 0 || a.ProductOfLawSKSP.Id == param.produkHukum)
                               && (string.IsNullOrWhiteSpace(param.status) || (a.Title != null ? a.Status.ToLower().Contains(param.status.ToLower()) : a.Status == param.status))
                               && (param.tmtBerlaku == null || a.TmtApplies == param.tmtBerlaku)
                               && (param.expiredDate == null || a.ExpiredDate == param.expiredDate)
                            select new SKSPJtable
                            {
                                Id = a.Id,
                                tmtBerlaku = a.TmtApplies,
                                //regulationCategoryId = (int)a.RegCategoryId,
                                //regulationCategoryName = a.RegCategoryId.ToString(),
                                //code = a.Code,
                                fileId = a.FileId,
                                fileSupportId = a.FileSupportId,
                                //fileName = c.FileName,
                                //fileSupportName = jk == null ? string.Empty : jk.FileName,
                                //kbo = a.KBO,
                                //kodeSimpan = a.SaveCode.Code,
                                //kodeSimpanId = a.SaveCode.Id,
                                //nomor = a.Number,
                                nomorSKSP = a.SKSPNumber,
                                perihal = a.Title,
                                //positionNumber = a.PositionNumber,
                                produkHukumName = "Prin " + a.ProductOfLawSKSP.Description,
                                produkHukumId = a.ProductOfLawSKSP.Id,
                                status = a.Status,
                                //statusDoc = a.StatusDocId.ToString(),
                                //statusDocId = (int)a.StatusDocId,
                                tahun = a.Year,
                                //type = a.TypeId.ToString(),
                                CreatedDate = a.CreatedAt,
                                expiredDate = a.ExpiredDate,
                                //jointReviewDate = a.JointReviewDate
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

                BaseDTResponseModel<SKSPJtable> result = new BaseDTResponseModel<SKSPJtable>
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

        public async Task<BaseDTResponseModel<ReportSKSPJtable>> getSPList(string companyCode)
        {
            try
            {
                List<ReportSKSPJtable> itemData = new List<ReportSKSPJtable>();
                List<ReportSKSPJtable> filteredData = new List<ReportSKSPJtable>();
                List<RegulationSKSP> allItemData = new List<RegulationSKSP>();

                allItemData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                                .Include(b => b.SaveCode)
                                                                .Where(c => c.IsActive == GeneralConstant.YES
                                                                         && c.IsDeleted == GeneralConstant.NO
                                                                         && c.TypeId == TypeSurat.Perintah
                                                                         && c.CompanyCode == companyCode)
                                                                .OrderByDescending(d => d.CreatedAt)
                                                                .ToListAsync();

                itemData = (from a in allItemData
                            select new ReportSKSPJtable
                            {
                                Id = a.Id,
                                tmtBerlaku = a.TmtApplies,
                                regulationCategoryId = (int)a.RegCategoryId,
                                regulationCategoryName = a.RegCategoryId.ToString(),
                                code = a.Code,
                                fileId = a.FileId,
                                fileSupportId = a.FileSupportId,
                                kbo = a.KBO,
                                kodeSimpan = a.SaveCode.Code,
                                kodeSimpanId = a.SaveCode.Id,
                                nomor = a.Number,
                                nomorSKSP = a.SKSPNumber,
                                perihal = a.Title,
                                positionNumber = a.PositionNumber,
                                produkHukumName = "Prin " + a.ProductOfLawSKSP.Description,
                                produkHukumId = a.ProductOfLawSKSP.Id,
                                status = a.Status,
                                statusDoc = a.StatusDocId.ToString(),
                                statusDocId = (int)a.StatusDocId,
                                type = a.TypeId.ToString(),
                                tahun = a.Year,
                                CreatedDate = a.CreatedAt,
                                CreatedBy = a.CreatedBy,
                                expiredDate = a.ExpiredDate,
                                jointReviewDate = a.JointReviewDate
                            }).ToList();

                BaseDTResponseModel<ReportSKSPJtable> result = new BaseDTResponseModel<ReportSKSPJtable>
                {
                    Data = itemData,
                    Draw = allItemData.Count(),
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

        public async Task<List<HistoryModel>> getRegulationSPHistory(int id)
        {
            try
            {
                ActivityLog logActivity = new ActivityLog();
                List<RegulationHistory> historyData = new List<RegulationHistory>();
                List<HistoryModel> retHistory = new List<HistoryModel>();

                historyData = await _proveContext.RegulationHistory.Include(a => a.RegulationSKSP)
                                                                   .Where(b => b.RegulationSKSP.Id == id)
                                                                   .ToListAsync();

                retHistory = (from a in historyData
                              select new HistoryModel
                              {
                                  Id = a.Id,
                                  Status = a.Status,
                                  Date = a.CreatedAt.ToString("dd MMMM yyyy"),
                                  Number = a.Number,
                                  Time = a.CreatedAt.ToString("HH : mm"),
                                  Title = a.Title,
                                  Notes = a.Notes,
                                  FileId = a.FileId,
                                  FileSupportId = a.FileSupportId,
                                  CreatedAt = a.CreatedAt,
                                  CreatedBy = a.CreatedBy,
                                  UpdatedAt = a.UpdatedAt,
                                  UpdatedBy = a.UpdatedBy
                              }).OrderByDescending(c => c.CreatedAt)
                                .ToList();

                #region log activity

                logActivity.Info = "Success Get Regulation History";
                logActivity.Method = "regulationHistory";
                logActivity.Remark = "PROVE BusinessLogic";
                logActivity.Status = "Success";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = "-";
                logActivity.CreatedAt = DateTime.Now;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                return retHistory;
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Get Regulation History";
                logActivity.Method = "regulationHistory";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                throw ex;
            }
        }

        public async Task<RegulationSKSP> detailSP(int id, CancellationToken cancellationToken)
            => await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                 .Include(b => b.SaveCode).FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        public async Task<ReturnValues> insertSP(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP spData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSKSP.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();

                    spData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    spData.Code = "Prin";
                    spData.FileId = fileID;
                    spData.FileSupportId = fileSupportID;
                    spData.KBO = param.kbo;
                    spData.Number = param.nomor;
                    spData.ProductOfLawSKSP = productOfLawData;
                    spData.SaveCode = saveCodeData;
                    spData.SKSPNumber = "Prin" + "-" + spData.Number + "/" + spData.KBO + "/" + spData.Year + "-" + saveCodeData.Code;
                    spData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    if (param.isAdmin == "true")
                    {
                        spData.Status = GeneralConstant.PROVE_UPLOADED; // Uploaded
                    }
                    else
                    {
                        spData.Status = GeneralConstant.PROVE_REQUEST; // New Request
                    }
                    spData.Title = param.perihal;
                    spData.Year = param.tahun;
                    spData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    spData.ExpiredDate = Convert.ToDateTime(param.expiredDate);
                    spData.TypeId = TypeSurat.Perintah;
                    spData.ConceptorId = param.conceptorId;
                    spData.ConceptorDirId = param.conceptorDirId;
                    spData.ConceptorDivId = param.conceptorDivId;
                    spData.CompanyCode = param.companyCode;
                    spData.CreatedBy = param.username;
                    spData.UpdatedBy = param.username;

                    await _proveContext.RegulationSKSP.AddAsync(spData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + saveCodeData.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.file,
                           Flag: "PROVE_SP",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + saveCodeData.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SP_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    spData.FileId = fileID;
                    spData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email
                    #region send email
                    var emailQM = await getListQM();

                    List<MailAddress> mailList = new List<MailAddress>();

                    foreach (var item in emailQM)
                    {
                        mailList.Add(new MailAddress(item.email));
                    }

                    var subject = "PROVE : Request - Pembuatan SPrin (" + spData.SKSPNumber + " - " + spData.Title + ")";

                    if (param.isAdmin == "true")
                    {
                        #region history
                        historyData.Number = spData.SKSPNumber;
                        historyData.Status = spData.Status;
                        historyData.Title = spData.Title;
                        historyData.RegulationSKSP = spData;
                        historyData.FileId = fileID;
                        historyData.Notes = param.notes;
                        historyData.CreatedBy = param.username;
                        historyData.UpdatedBy = param.username;

                        await _proveContext.RegulationHistory.AddAsync(historyData);
                        await _proveContext.SaveChangesAsync();
                        #endregion
                    }
                    else
                    {
                        var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim Document Control SPrin" + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Surat Perintah dengan nomor " + spData.SKSPNumber + " tentang " + spData.Title + " telah diajukan, " +
                        "mohon dapat di review. Catatan : </p>" +
                        "<p style='text-align:justify;'>" +
                            param.notes +
                        "</p>" +
                        "<br/>" +
                        "<p style='text-align:justify;'>Terimakasih.</p>";

                        try
                        {
                            await _mailService.Send(subject, BaseEmail.CreateMessage(body), mailList);
                        }
                        catch (Exception ex)
                        {
                            retVal.retEmail = ex.Message.ToString();

                            #region log activity

                            ActivityLog logActivityEmail = new ActivityLog();
                            logActivityEmail.Info = "Error Transaction Send Email";
                            logActivityEmail.Method = "insertSP";
                            logActivityEmail.Remark = "PROVE Business Logic";
                            logActivityEmail.Status = "Error";
                            logActivityEmail.UserName = param.username;
                            logActivityEmail.Controller = "PROVERegulationSPRepoImpl";
                            logActivityEmail.ErrorMessage = ex.Message;

                            await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                            await _proveContext.SaveChangesAsync();

                            #endregion
                        }

                        #region history
                        historyData.Number = spData.SKSPNumber;
                        historyData.Status = spData.Status;
                        historyData.Title = spData.Title;
                        historyData.RegulationSKSP = spData;
                        historyData.FileId = fileID;
                        historyData.Notes = param.notes;
                        historyData.CreatedBy = param.username;
                        historyData.UpdatedBy = param.username;

                        await _proveContext.RegulationHistory.AddAsync(historyData);
                        await _proveContext.SaveChangesAsync();
                        #endregion
                    }

                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Insert";
                    logActivity.Method = "insertSP";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSPRepoImpl";
                    logActivity.ErrorMessage = "-";

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();

                    retVal.ret = true;
                    retVal.retMessage = "Success";

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Insert";
                logActivity.Method = "insertSP";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> uploadedSP(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP spData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSKSP.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();

                    spData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    fileID = spData.FileId;
                    fileSupportID = spData.FileSupportId;

                    spData.Number = param.nomor;
                    spData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    spData.ProductOfLawSKSP = productOfLawData;
                    spData.Year = param.tahun;
                    spData.SaveCode = saveCodeData;
                    spData.SKSPNumber = "Prin" + "-" + spData.Number + "/" + spData.KBO + "/" + spData.Year + "-" + saveCodeData.Code;
                    spData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    spData.Status = GeneralConstant.PROVE_UPLOADED; // Uploaded
                    spData.Title = param.perihal;
                    spData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    spData.ExpiredDate = Convert.ToDateTime(param.expiredDate);
                    spData.UpdatedAt = DateTime.Now;
                    spData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + saveCodeData.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.file,
                           Flag: "PROVE_SP",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + saveCodeData.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SP_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    spData.FileId = fileID;
                    spData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    #region send email
                    if (spData.ConceptorDirId != param.conceptorDirId)
                    {
                        var listEmp = await getListUser(spData.ConceptorDirId);

                        List<MailAddress> mailListUser = new List<MailAddress>();

                        foreach (var item in listEmp.Select(a => a.email))
                        {
                            mailListUser.Add(new MailAddress(item));
                        }

                        var subject = "PROVE : Uploaded - Pembuatan SPrin (" + spData.SKSPNumber + " - " + spData.Title + ") Telah Selesai";

                        var body =
                            "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                            "<p style='text-align:justify;'>Surat Perintah dengan nomor " + spData.SKSPNumber + " tentang " + spData.Title + " sudah berhasil dibuat dan diunggah " +
                            "di aplikasi PROVE pada menu Final Document. Catatan : </p>" +
                            "<p style='text-align:justify;'>" +
                            param.notes +
                            "</p>" +
                            "<br/>" +
                            "<p style='text-align:justify;'>Terimakasih.</p>";

                        try
                        {
                            await _mailService.Send(subject, BaseEmail.CreateMessage(body), mailListUser);
                        }
                        catch (Exception ex)
                        {
                            retVal.retEmail = ex.Message.ToString();

                            #region log activity

                            ActivityLog logActivityEmail = new ActivityLog();
                            logActivityEmail.Info = "Error Transaction Send Email";
                            logActivityEmail.Method = "uploadedSP";
                            logActivityEmail.Remark = "PROVE Business Logic";
                            logActivityEmail.Status = "Error";
                            logActivityEmail.UserName = param.username;
                            logActivityEmail.Controller = "PROVERegulationSPRepoImpl";
                            logActivityEmail.ErrorMessage = ex.Message;

                            await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                            await _proveContext.SaveChangesAsync();

                            #endregion
                        }
                    }
                    #endregion

                    #region history
                    historyData.Number = spData.SKSPNumber;
                    historyData.Status = spData.Status;
                    historyData.Title = spData.Title;
                    historyData.RegulationSKSP = spData;
                    historyData.FileId = fileID;
                    historyData.Notes = param.notes;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion


                    #region log activity

                    logActivity.Info = "Success Transaction Uploaded";
                    logActivity.Method = "uploadedSP";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSPRepoImpl";
                    logActivity.ErrorMessage = "-";

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();

                    retVal.ret = true;
                    retVal.retMessage = "Success";

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Uploaded";
                logActivity.Method = "uploadedSP";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> revisedSP(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP spData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSKSP.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();

                    spData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    fileID = spData.FileId;
                    fileSupportID = spData.FileSupportId;

                    spData.Number = param.nomor;
                    spData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    spData.ProductOfLawSKSP = productOfLawData;
                    spData.Year = param.tahun;
                    spData.SaveCode = saveCodeData;
                    spData.SKSPNumber = "Prin" + "-" + spData.Number + "/" + spData.KBO + "/" + spData.Year + "-" + saveCodeData.Code;
                    spData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    spData.Status = GeneralConstant.PROVE_REVISED; // Revised
                    spData.Title = param.perihal;
                    spData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    spData.ExpiredDate = Convert.ToDateTime(param.expiredDate);
                    spData.UpdatedAt = DateTime.Now;
                    spData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + saveCodeData.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.file,
                           Flag: "PROVE_SP",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + saveCodeData.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SP_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    spData.FileId = fileID;
                    spData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email

                    #region send email
                    var emailQM = await getListQM();

                    List<MailAddress> mailListQM = new List<MailAddress>();

                    foreach (var item in emailQM)
                    {
                        mailListQM.Add(new MailAddress(item.email));
                    }

                    var subject = "PROVE : Revised - Pembuatan SPrin (" + spData.SKSPNumber + " - " + spData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim Document Control SPrin" + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Surat Perintah dengan nomor " + spData.SKSPNumber + " tentang " + spData.Title +
                        " telah di revisi, mohon dapat di review kembali. Catatan :</p>" +
                        "<p style='text-align:justify;'>" +
                        param.notes +
                        "</p>" +
                        "<br/>" +
                        "<p style='text-align:justify;'>Terimakasih.</p>";


                    try
                    {
                        await _mailService.Send(subject, BaseEmail.CreateMessage(body), mailListQM);
                    }
                    catch (Exception ex)
                    {
                        retVal.retEmail = ex.Message.ToString();

                        #region log activity

                        ActivityLog logActivityEmail = new ActivityLog();
                        logActivityEmail.Info = "Error Transaction Send Email";
                        logActivityEmail.Method = "revisedSP";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSPRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }

                    #endregion 

                    #region history
                    historyData.Number = spData.SKSPNumber;
                    historyData.Status = spData.Status;
                    historyData.Title = spData.Title;
                    historyData.RegulationSKSP = spData;
                    historyData.FileId = fileID;
                    historyData.Notes = param.notes;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Revised";
                    logActivity.Method = "revisedSP";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSPRepoImpl";
                    logActivity.ErrorMessage = "-";

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();

                    retVal.ret = true;
                    retVal.retMessage = "Success";

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Revised";
                logActivity.Method = "revisedSP";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> deleteSP(int id)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP spData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    spData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == id)
                                                               .FirstOrDefaultAsync();

                    spData.IsActive = GeneralConstant.NO;
                    spData.IsDeleted = GeneralConstant.YES;
                    spData.Status = GeneralConstant.PROVE_DELETED; //Deleted
                    spData.UpdatedAt = DateTime.Now;
                    spData.UpdatedBy = "SYSTEM";

                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    #region history
                    historyData.Number = spData.SKSPNumber;
                    historyData.Status = spData.Status;
                    historyData.Title = spData.Title;
                    historyData.RegulationSKSP = spData;
                    historyData.FileId = spData.FileId;
                    historyData.Notes = "Deleted";
                    historyData.CreatedBy = "SYSTEM";
                    historyData.UpdatedBy = "SYSTEM";

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Delete";
                    logActivity.Method = "deleteSP";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = "SYSTEM";
                    logActivity.Controller = "PROVERegulationSPRepoImpl";
                    logActivity.ErrorMessage = "-";

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();

                    retVal.ret = true;
                    retVal.retMessage = "Success";

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Delete";
                logActivity.Method = "deleteSP";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> approveSP(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP spData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    spData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = spData.FileId;
                    var fileSupportID = spData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + spData.SaveCode.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.file,
                           Flag: "PROVE_SP",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + spData.SaveCode.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SP_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    spData.FileId = fileID;
                    spData.FileSupportId = fileSupportID;
                    spData.Status = GeneralConstant.PROVE_REVIEWED; //reviewed Prod
                    spData.UpdatedAt = DateTime.Now;
                    spData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    #region history
                    historyData.Number = spData.SKSPNumber;
                    historyData.Status = spData.Status;
                    historyData.Title = spData.Title;
                    historyData.RegulationSKSP = spData;
                    historyData.Notes = param.notes;
                    historyData.FileId = spData.FileId;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Approve";
                    logActivity.Method = "approveSP";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSPRepoImpl";
                    logActivity.ErrorMessage = "-";

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();

                    retVal.ret = true;
                    retVal.retMessage = "Success";

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Approve";
                logActivity.Method = "approveSP";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> reviseSP(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP spData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    spData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = spData.FileId;
                    var fileSupportID = spData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + spData.SaveCode.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.file,
                           Flag: "SP_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "SP_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + spData.SaveCode.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.fileSupport,
                           Flag: "SP_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "SP_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    spData.FileId = fileID;
                    spData.FileSupportId = fileSupportID;
                    spData.Status = GeneralConstant.PROVE_REVISED; //revise
                    spData.UpdatedAt = DateTime.Now;
                    spData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email

                    #region send email
                    var listEmp = await getListUser(spData.ConceptorDirId);

                    List<MailAddress> mailListUser = new List<MailAddress>();

                    foreach (var item in listEmp.Select(a => a.email))
                    {
                        mailListUser.Add(new MailAddress(item));
                    }

                    var subject = "STK : Need Revise Surat Perintah (" + spData.SKSPNumber + " - " + spData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                        "<p style='text-align:justify;'>Pembuatan Surat Perintah dengan nomor " + spData.SKSPNumber +
                        ", perlu dilakukan perbaikan dengan catatan : " +
                        param.notes +
                        "</p>" +
                        "<br/>" +
                        "<p style='text-align:justify;'>Terimakasih.</p>";

                    try
                    {
                        await _mailService.Send(subject, BaseEmail.CreateMessage(body), mailListUser);
                    }
                    catch (Exception ex)
                    {
                        retVal.retEmail = ex.Message.ToString();

                        #region log activity

                        ActivityLog logActivityEmail = new ActivityLog();
                        logActivityEmail.Info = "Error Transaction Send Email";
                        logActivityEmail.Method = "rejectSP";
                        logActivityEmail.Remark = "STK Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "STKRegulationSPRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;
                        logActivityEmail.CreatedAt = DateTime.Now;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }

                    #endregion

                    #region history
                    historyData.Number = spData.SKSPNumber;
                    historyData.Status = spData.Status;
                    historyData.Title = spData.Title;
                    historyData.RegulationSKSP = spData;
                    historyData.FileId = spData.FileId;
                    historyData.Notes = param.notes;
                    historyData.IsActive = GeneralConstant.YES;
                    historyData.IsDeleted = GeneralConstant.NO;
                    historyData.CreatedAt = DateTime.Now;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedAt = DateTime.Now;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Reject";
                    logActivity.Method = "rejectSP";
                    logActivity.Remark = "STK BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "STKRegulationSPRepoImpl";
                    logActivity.ErrorMessage = "-";
                    logActivity.CreatedAt = DateTime.Now;

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();

                    retVal.ret = true;
                    retVal.retMessage = "Success";

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Reject";
                logActivity.Method = "rejectSP";
                logActivity.Remark = "STK Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "STKRegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;
                logActivity.CreatedAt = DateTime.Now;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> reviewSP(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP spData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    spData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = spData.FileId;
                    var fileSupportID = spData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + spData.SaveCode.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.file,
                           Flag: "PROVE_SP",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + spData.SaveCode.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SP_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    spData.FileId = fileID;
                    spData.FileSupportId = fileSupportID;
                    spData.JointReviewDate = Convert.ToDateTime(param.jointReviewDate);
                    spData.Status = GeneralConstant.PROVE_REVIEW; //on review
                    spData.UpdatedAt = DateTime.Now;
                    spData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email

                    #region send email
                    var listEmp = await getListUser(spData.ConceptorDirId);

                    List<MailAddress> mailListUser = new List<MailAddress>();

                    foreach (var item in listEmp.Select(a => a.email))
                    {
                        mailListUser.Add(new MailAddress(item));
                    }

                    var subject = "PROVE : On Review - Pembuatan SPrin (" + spData.SKSPNumber + " - " + spData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Surat Perintah dengan nomor " + spData.SKSPNumber + " tentang " + spData.Title +
                        " dalam proses review. Catatan : </p>" +
                        "<p style='text-align:justify;'>" +
                        param.notes +
                        "</p>" +
                        "<br/>" +
                        "<p style='text-align:justify;'>Terimakasih.</p>";

                    try
                    {
                        await _mailService.Send(subject, BaseEmail.CreateMessage(body), mailListUser);
                    }
                    catch (Exception ex)
                    {
                        retVal.retEmail = ex.Message.ToString();

                        #region log activity

                        ActivityLog logActivityEmail = new ActivityLog();
                        logActivityEmail.Info = "Error Transaction Send Email";
                        logActivityEmail.Method = "reviewSP";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSPRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }

                    #endregion

                    #region history
                    historyData.Number = spData.SKSPNumber;
                    historyData.Status = spData.Status;
                    historyData.Title = spData.Title;
                    historyData.RegulationSKSP = spData;
                    historyData.Notes = param.notes;
                    historyData.FileId = spData.FileId;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Review";
                    logActivity.Method = "reviewSP";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSPRepoImpl";
                    logActivity.ErrorMessage = "-";

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();

                    retVal.ret = true;
                    retVal.retMessage = "Success";

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Review";
                logActivity.Method = "reviewSP";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> needSignSP(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP spData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    spData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = spData.FileId;
                    var fileSupportID = spData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + spData.SaveCode.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.file,
                           Flag: "PROVE_SP",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + spData.SaveCode.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SP_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    spData.FileId = fileID;
                    spData.FileSupportId = fileSupportID;
                    spData.Status = GeneralConstant.PROVE_NEED_SIGN; //need sign
                    spData.UpdatedAt = DateTime.Now;
                    spData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email

                    #region send email
                    var listEmp = await getListUser(spData.ConceptorDirId);

                    List<MailAddress> mailListUser = new List<MailAddress>();

                    foreach (var item in listEmp.Select(a => a.email))
                    {
                        mailListUser.Add(new MailAddress(item));
                    }

                    var subject = "PROVE : Need Sign - Pembuatan SPrin (" + spData.SKSPNumber + " - " + spData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Surat Perintah dengan nomor " + spData.SKSPNumber + " tentang " + spData.Title +
                        " dapat dilakukan serkuler perstujuannya kepada Pihak/Pejabat terkait. Catatan : </p>" +
                        "<p style='text-align:justify;'>" +
                        param.notes +
                        "</p>" +
                        "<br/>" +
                        "<p style='text-align:justify;'>Terimakasih.</p>";

                    try
                    {
                        await _mailService.Send(subject, BaseEmail.CreateMessage(body), mailListUser);
                    }
                    catch (Exception ex)
                    {
                        retVal.retEmail = ex.Message.ToString();

                        #region log activity

                        ActivityLog logActivityEmail = new ActivityLog();
                        logActivityEmail.Info = "Error Transaction Send Email";
                        logActivityEmail.Method = "needSignSP";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSPRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }
                    #endregion

                    #region history
                    historyData.Number = spData.SKSPNumber;
                    historyData.Status = spData.Status;
                    historyData.Title = spData.Title;
                    historyData.RegulationSKSP = spData;
                    historyData.Notes = param.notes;
                    historyData.FileId = spData.FileId;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Need Sign";
                    logActivity.Method = "needSignSP";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSPRepoImpl";
                    logActivity.ErrorMessage = "-";

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();

                    retVal.ret = true;
                    retVal.retMessage = "Success";

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Need Sign";
                logActivity.Method = "needSignSP";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> signedSP(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP spData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSKSP.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();

                    spData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    fileID = spData.FileId;
                    fileSupportID = spData.FileSupportId;

                    spData.Number = param.nomor;
                    spData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    spData.ProductOfLawSKSP = productOfLawData;
                    spData.Year = param.tahun;
                    spData.SaveCode = saveCodeData;
                    spData.SKSPNumber = "Prin" + "-" + spData.Number + "/" + spData.KBO + "/" + spData.Year + "-" + saveCodeData.Code;
                    spData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    spData.Status = GeneralConstant.PROVE_SIGNED; // Revised
                    spData.Title = param.perihal;
                    spData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    spData.ExpiredDate = Convert.ToDateTime(param.expiredDate);
                    spData.UpdatedAt = DateTime.Now;
                    spData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + saveCodeData.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.file,
                           Flag: "PROVE_SP",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SP/PRTH/" + spData.KBO + "/" + saveCodeData.Code + "/" + spData.Number + "/" + spData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: spData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SP_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SP_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    spData.FileId = fileID;
                    spData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSKSP.Update(spData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email

                    #region send email
                    var emailQM = await getListQM();

                    List<MailAddress> mailListQM = new List<MailAddress>();

                    foreach (var item in emailQM)
                    {
                        mailListQM.Add(new MailAddress(item.email));
                    }

                    var subject = "PROVE : Signed - Pembuatan SPrin (" + spData.SKSPNumber + " - " + spData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim Document Control SPrin" + " </p><br/>" +
                        "<p style='text-align:justify;'>Draft Surat Perintah dengan nomor " + spData.SKSPNumber + " tentang " + spData.Title +
                        " telah di sahkan oleh Pihak/Pejabat terkait. Catatan :</p>" +
                        "<p style='text-align:justify;'>" +
                        param.notes +
                        "</p>" +
                        "<br/>" +
                        "<p style='text-align:justify;'>Terimakasih.</p>";


                    try
                    {
                        await _mailService.Send(subject, BaseEmail.CreateMessage(body), mailListQM);
                    }
                    catch (Exception ex)
                    {
                        retVal.retEmail = ex.Message.ToString();

                        #region log activity

                        ActivityLog logActivityEmail = new ActivityLog();
                        logActivityEmail.Info = "Error Transaction Send Email";
                        logActivityEmail.Method = "signedSP";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSPRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }

                    #endregion

                    #region history
                    historyData.Number = spData.SKSPNumber;
                    historyData.Status = spData.Status;
                    historyData.Title = spData.Title;
                    historyData.RegulationSKSP = spData;
                    historyData.FileId = fileID;
                    historyData.Notes = param.notes;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Signed";
                    logActivity.Method = "signedSP";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSPRepoImpl";
                    logActivity.ErrorMessage = "-";

                    await _proveContext.ActivityLog.AddAsync(logActivity);
                    await _proveContext.SaveChangesAsync();

                    #endregion

                    transaction.Commit();

                    retVal.ret = true;
                    retVal.retMessage = "Success";

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                #region log activity

                ActivityLog logActivity = new ActivityLog();
                logActivity.Info = "Error Transaction Signed";
                logActivity.Method = "signedSP";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSPRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        #region commend QM Email
        //public async Task<List<string>> getQMEmail()
        //{
        //    using (IServiceScope scope = _serviceProvider.CreateScope())
        //    {
        //        List<string> tempEmpEmail = new List<string>();

        //        var _userManager = (UserManager<ApplicationUser>)scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>));
        //        var _roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));

        //        var roleList = await _roleManager.Roles.Where(b => b.Name.Contains("STK_ADMIN")).ToListAsync();

        //        foreach (var item in roleList)
        //        {
        //            var userSTKList = await _userManager.GetUsersInRoleAsync(item.ToString());

        //            for (int i = 0; i < userSTKList.Count(); i++)
        //            {
        //                string email = userSTKList[i].Email;
        //                string name = userSTKList[i].UserName;
        //                tempEmpEmail.Add(email);
        //            }
        //        }

        //        return tempEmpEmail;
        //    }
        //}
        #endregion

        #region commend get emp div
        //public async Task<List<Employee>> getEmployeeDivision(int id)
        //{
        //    using (IServiceScope scope = _serviceProvider.CreateScope())
        //    {
        //        List<int> tempEmpId = new List<int>();
        //        List<string> tempEmpEmail = new List<string>();
        //        List<int> tempAstManId = new List<int>();
        //        List<int> tempAstManChildId = new List<int>();

        //        var abbrv = _employeePositionService.GetAllIncludingWithFilter(
        //                b => b.IsDeleted == PrideConstant.NO &&
        //                b.Id == Convert.ToInt32(id),
        //                b => b.EmployeePositionAbbrv).Select(b => b.EmployeePositionAbbrv.Id);

        //        if (abbrv.Count() > 0)
        //        {

        //            if (abbrv.FirstOrDefault() == 1) //BOD
        //            {
        //                List<int> tempBODId = new List<int> { Convert.ToInt32(id) };
        //                tempEmpId.AddRange(tempBODId);

        //                IEnumerable<int> EmployeeBOD = _employeeService.GetAllIncludingWithFilter(
        //                    b => b.IsDeleted == PrideConstant.NO &&
        //                    tempBODId.Any(d => d == b.EmployeePosition.Id),
        //                    b => b.EmployeePosition).Select(b => b.Id);

        //                tempEmpId.AddRange(EmployeeBOD);

        //            }
        //            else if (abbrv.FirstOrDefault() == 2)// Directorate
        //            {
        //                List<int> list = new List<int>();
        //                List<int> tempManagerId = new List<int>();
        //                List<int> tempDirectorateId = new List<int> { Convert.ToInt32(id) };
        //                list.AddRange(tempDirectorateId);

        //                tempManagerId.AddRange(_employeePositionStructsService
        //                    .GetAllIncludingWithFilter(b => b.IsDeleted == PrideConstant.NO &&
        //                    b.Leader.Id == Convert.ToInt32(id) &&
        //                    b.EmployeePosition.EmployeePositionAbbrv.Id != 7,
        //                    b => b.EmployeePosition,
        //                    b => b.EmployeePosition.EmployeePositionAbbrv)
        //                    .Select(b => b.EmployeePosition.Id));

        //                list.AddRange(tempManagerId);

        //                tempAstManId.AddRange(_employeePositionStructsService
        //                    .GetAllIncludingWithFilter(
        //                    b => b.IsDeleted == PrideConstant.NO &&
        //                    tempManagerId.Any(c => c == b.Leader.Id),
        //                    b => b.EmployeePosition).Select(b => b.EmployeePosition.Id));

        //                list.AddRange(tempAstManId);

        //                IEnumerable<int> AstManChile = _employeePositionStructsService
        //                    .GetAllIncludingWithFilter(
        //                    b => b.IsDeleted == PrideConstant.NO &&
        //                    tempAstManId.Any(c => c == b.Leader.Id),
        //                    b => b.EmployeePosition).Select(b => b.Id);

        //                list.AddRange(AstManChile);

        //                IEnumerable<int> EmployeeDirectorate = _employeeService.GetAllIncludingWithFilter(
        //                    b => b.IsDeleted == PrideConstant.NO &&
        //                    list.Any(d => d == b.EmployeePosition.Id),
        //                    b => b.EmployeePosition).Select(b => b.Id);

        //                tempEmpId.AddRange(EmployeeDirectorate);
        //            }
        //            else if (abbrv.FirstOrDefault() == 3) // Division
        //            {
        //                List<int> list = new List<int>();

        //                List<int> tempMgrId = new List<int> { Convert.ToInt32(id) };
        //                list.AddRange(tempMgrId);

        //                tempAstManId.AddRange(_employeePositionStructsService
        //                    .GetAllIncludingWithFilter(b => b.IsDeleted == PrideConstant.NO &&
        //                    b.Leader.Id == Convert.ToInt32(id),
        //                    b => b.EmployeePosition).Select(b => b.EmployeePosition.Id));
        //                list.AddRange(tempAstManId);

        //                IEnumerable<int> AstManChile = _employeePositionStructsService.GetAllIncludingWithFilter(
        //                    b => b.IsDeleted == PrideConstant.NO &&
        //                    tempAstManId.Any(c => c == b.Leader.Id),
        //                    b => b.EmployeePosition).Select(b => b.EmployeePosition.Id);
        //                //b => b.EmployeePosition).Select(b => b.Id);
        //                list.AddRange(AstManChile);

        //                IEnumerable<int> EmployeeDivision = _employeeService.GetAllIncludingWithFilter(
        //                    b => b.IsDeleted == PrideConstant.NO &&
        //                    list.Any(d => d == b.EmployeePosition.Id),
        //                    b => b.EmployeePosition).Select(b => b.Id);
        //                tempEmpId.AddRange(EmployeeDivision);

        //            }
        //            else if (abbrv.FirstOrDefault() == 4) // Division (Quality & Culture Management || Asset Management)
        //            {
        //                if (Convert.ToInt32(id) == 25 || Convert.ToInt32(id) == 610)
        //                {
        //                    List<int> list = new List<int>();

        //                    List<int> tempMgrId = new List<int> { Convert.ToInt32(id) };
        //                    list.AddRange(tempMgrId);

        //                    tempAstManId.AddRange(_employeePositionStructsService
        //                        .GetAllIncludingWithFilter(b => b.IsDeleted == PrideConstant.NO &&
        //                        b.Leader.Id == Convert.ToInt32(id),
        //                        b => b.EmployeePosition).Select(b => b.EmployeePosition.Id));
        //                    list.AddRange(tempAstManId);

        //                    IEnumerable<int> AstManChile = _employeePositionStructsService.GetAllIncludingWithFilter(
        //                        b => b.IsDeleted == PrideConstant.NO &&
        //                        tempAstManId.Any(c => c == b.Leader.Id),
        //                        b => b.EmployeePosition).Select(b => b.Id);
        //                    list.AddRange(AstManChile);

        //                    IEnumerable<int> EmployeeDivision = _employeeService.GetAllIncludingWithFilter(
        //                        b => b.IsDeleted == PrideConstant.NO &&
        //                        list.Any(d => d == b.EmployeePosition.Id),
        //                        b => b.EmployeePosition).Select(b => b.Id);
        //                    tempEmpId.AddRange(EmployeeDivision);
        //                }

        //            }
        //            else
        //            {
        //                tempEmpId.AddRange(_employeeService.GetAllWithFilter(b => b.IsDeleted == PrideConstant.NO).Select(b => b.Id));
        //            }
        //        }
        //        else
        //        {
        //            tempEmpId.AddRange(_employeeService.GetAllWithFilter(b => b.IsDeleted == PrideConstant.NO).Select(b => b.Id));
        //        }

        //        List<Employee> dataEmployee = await _employeeService.GetAllIncludingWithFilter(
        //            b => b.IsDeleted == PrideConstant.NO &&
        //            tempEmpId.Any(c => c == b.Id) &&
        //            b.EmployeePosition.EmployeePositionAbbrv.Id <= 7,
        //            b => b.EmployeePosition,
        //            b => b.Religion,
        //            b => b.EmployeePosition.EmployeePositionAbbrv
        //            )
        //            .ToListAsync();

        //        var _userManager = (UserManager<ApplicationUser>)scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>));
        //        var _roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));

        //        var roleList = await _roleManager.Roles.Where(b => b.Name.Contains("STK_USER")).ToListAsync();

        //        foreach (var item in roleList)
        //        {
        //            var userSTKList = await _userManager.GetUsersInRoleAsync(item.ToString());

        //            for (int i = 0; i < userSTKList.Count(); i++)
        //            {
        //                string email = userSTKList[i].Email;
        //                string name = userSTKList[i].UserName;
        //                tempEmpEmail.Add(email);
        //            }
        //        }

        //        List<Employee> filterdData = (from a in dataEmployee
        //                                      where tempEmpEmail.Contains(a.Email)
        //                                      select a).ToList();

        //        return filterdData;
        //    }
        //}
        #endregion

        public string NormalizeEmployeePositionName(string param) => param
                .Replace("Manager ", "")
                .Replace("Head of ", "")
                .Replace("Ast Manager ", "")
                .Replace("Sr. Manager ", "")
                .Replace("Ast ", "")
                .Replace("Sr. ", "")
                .Replace("VP ", "")
                .Replace("GM ", "")
                .Replace("Chief ", "")
                .Replace("Direktur ", "");

        public async Task<List<Models.User>> getListUser(string concepdirId)
        {
            HttpClient c = _httpClient.CreateClient("IDAMAN.API");

            try
            {
                var data = await _usmanContext.UserRoles.Include(a => a.PositionUser)
                                                        .Include(b => b.Role)
                                                        .ThenInclude(c => c.Application)
                                                        .Where(d => d.Role.Application.ApplicationName == "PROVE"
                                                                 && d.Role.RoleName == "Member"
                                                                 && d.PositionUser.PositionParentId == concepdirId
                                                                 && d.IsActive == GeneralConstant.YES
                                                                 && d.IsDeleted == GeneralConstant.NO)
                                                        .ToListAsync();

                List<Models.User> dataUser = new List<Models.User>();

                foreach (var item in data)
                {
                    var x = await c.GetAsync($"/v1/positions/user/{item.PositionUser.PositionId}");

                    if (x.IsSuccessStatusCode)
                    {

                        var dataReturn = await x.Content.ReadAsAsync<PosChildren>();

                        Models.User dataUserPriv = new Models.User
                        {
                            displayName = dataReturn.user.displayName,
                            division = item.PositionUser.OrganizationParentName,
                            email = dataReturn.user.email,
                            id = dataReturn.user.id
                        };

                        dataUser.Add(dataUserPriv);
                    }
                }
                return dataUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Models.User>> getListQM()
        {
            HttpClient c = _httpClient.CreateClient("IDAMAN.API");

            try
            {
                var data = await _usmanContext.UserRoles.Include(a => a.PositionUser)
                                                        .Include(b => b.Role)
                                                        .ThenInclude(c => c.Application)
                                                        .Where(d => d.Role.RoleName == "Administrator"
                                                                 && d.Role.Application.ApplicationName == "PROVE"
                                                                 && d.IsActive == GeneralConstant.YES
                                                                 && d.IsDeleted == GeneralConstant.NO)
                                                        .ToListAsync();

                List<Models.User> dataUser = new List<Models.User>();

                foreach (var item in data)
                {
                    var x = await c.GetAsync($"/v1/positions/user/{item.PositionUser.PositionId}");

                    if (x.IsSuccessStatusCode)
                    {
                        var dataReturn = await x.Content.ReadAsAsync<PosChildren>();

                        dataUser.Add(dataReturn.user);
                    }
                }
                return dataUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
