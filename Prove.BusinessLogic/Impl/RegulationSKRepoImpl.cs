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
    public class RegulationSKRepoImpl : IRegulationSKRepo
    {
        private readonly ProveExtContext _proveContext;
        private readonly UsmanContext _usmanContext;
        private readonly IFileUploadRepo _fileUploadService;
        private readonly ISendMail _mailService;
        private readonly IHttpClientFactory _httpClient;

        public RegulationSKRepoImpl(ProveExtContext proveContext, IFileUploadRepo fileUploadService, ISendMail mailService, IHttpClientFactory httpClient, UsmanContext usmanContext)
        {
            _proveContext = proveContext;
            _fileUploadService = fileUploadService;
            _mailService = mailService;
            _httpClient = httpClient;
            _usmanContext = usmanContext;
        }

        public async Task<BaseDTResponseModel<SKSPJtable>> getSKDatatable(SKSPJtableParam param)
        {
            try
            {
                IQueryable<SKSPJtable> itemData = null;
                List<SKSPJtable> filteredData = new List<SKSPJtable>();
                //List<EmployeePosition> positionData = new List<EmployeePosition>();
                IQueryable<RegulationSKSP> allItemData = null;

                if (param.isAdmin == "true")
                {
                    allItemData = _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                                    .Include(b => b.SaveCode)
                                                                    .Where(c => c.IsActive == GeneralConstant.YES
                                                                             && c.IsDeleted == GeneralConstant.NO
                                                                             && c.TypeId == TypeSurat.Keputusan
                                                                             && c.CompanyCode == param.companyCode)
                                                                    .OrderByDescending(d => d.CreatedAt);
                }
                else
                {
                    //if (param.conceptorDirId == param.conceptorId)
                    //{
                    //    allItemData = _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                    //                                                    .Include(b => b.SaveCode)
                    //                                                    .Where(c => c.IsActive == GeneralConstant.YES
                    //                                                             && c.IsDeleted == GeneralConstant.NO
                    //                                                             && c.TypeId == TypeSurat.Keputusan
                    //                                                             && (param.conceptorDivId == "0" ? c.ConceptorDirId == Convert.ToInt32(param.conceptorDirId) : c.ConceptorDivId == Convert.ToInt32(param.conceptorDivId)))
                    //                                                    .OrderByDescending(d => d.CreatedAt);
                    //}
                    //else
                    //{
                    allItemData = _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                                    .Include(b => b.SaveCode)
                                                                    .Where(c => c.IsActive == GeneralConstant.YES
                                                                             && c.IsDeleted == GeneralConstant.NO
                                                                             && c.TypeId == TypeSurat.Keputusan
                                                                             //&& (param.conceptorDivId == "0" ? c.ConceptorId == Convert.ToInt32(param.conceptorId) : c.ConceptorDivId == Convert.ToInt32(param.conceptorDivId)))
                                                                             && (c.ConceptorDirId == param.conceptorDirId)
                                                                             && c.CompanyCode == param.companyCode)
                                                                    .OrderByDescending(d => d.CreatedAt);
                    //}
                }

                //fileData = _proveContext.UploadFiles.ToList();

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
                               && (string.IsNullOrWhiteSpace(param.status) || (a.Status != null ? a.Status.ToLower().Contains(param.status.ToLower()) : a.Status == param.status))
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
                                produkHukumName = "Kpts " + a.ProductOfLawSKSP.Description,
                                produkHukumId = a.ProductOfLawSKSP.Id,
                                status = a.Status,
                                //statusDoc = a.StatusDocId.ToString(),
                                //statusDocId = (int)a.StatusDocId,
                                //type = a.TypeId.ToString(),
                                tahun = a.Year,
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

        public async Task<BaseDTResponseModel<ReportSKSPJtable>> getSKList(string companyCode)
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
                                                                         && c.TypeId == TypeSurat.Keputusan
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
                                produkHukumName = "Kpts " + a.ProductOfLawSKSP.Description,
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
                    Draw = itemData.Count(),
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

        public async Task<List<HistoryModel>> getRegulationSKHistory(int id)
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
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = "-";

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
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                throw ex;
            }
        }

        public async Task<RegulationSKSP> detailSK(int id, CancellationToken cancellationToken)
           => await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                .Include(b => b.SaveCode).FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        public async Task<ReturnValues> insertSK(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP skData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSKSP.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();

                    skData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    skData.Code = "Kpts";
                    skData.FileId = fileID;
                    skData.FileSupportId = fileSupportID;
                    skData.KBO = param.kbo;
                    skData.Number = param.nomor;
                    skData.ProductOfLawSKSP = productOfLawData;
                    skData.SaveCode = saveCodeData;
                    skData.Year = param.tahun;
                    skData.SKSPNumber = "Kpts" + "-" + skData.Number + "/" + skData.KBO + "/" + skData.Year + "-" + saveCodeData.Code;
                    skData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    if (param.isAdmin == "true")
                    {
                        skData.Status = GeneralConstant.PROVE_UPLOADED; //Uploaded
                    }
                    else
                    {
                        skData.Status = GeneralConstant.PROVE_REQUEST; // New Request
                    }
                    skData.Title = param.perihal;
                    
                    skData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    skData.ExpiredDate = Convert.ToDateTime(param.expiredDate);
                    skData.TypeId = TypeSurat.Keputusan;
                    skData.ConceptorId = param.conceptorId;
                    skData.ConceptorDirId = param.conceptorDirId;
                    skData.ConceptorDivId = param.conceptorDivId;
                    skData.CompanyCode = param.companyCode;
                    skData.CreatedBy = param.username;
                    skData.UpdatedBy = param.username;

                    await _proveContext.RegulationSKSP.AddAsync(skData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + saveCodeData.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.file,
                           Flag: "PROVE_SK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + saveCodeData.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    skData.FileId = fileID;
                    skData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email ke QM untuk di review
                    var emailQM = await getListQM();

                    List<MailAddress> mailList = new List<MailAddress>();

                    foreach (var item in emailQM.Select(a => a.email))
                    {
                        mailList.Add(new MailAddress(item));
                    }

                    var subject = "PROVE : Request - Pembuatan SKpts (" + skData.SKSPNumber + " - " + skData.Title + ")";

                    if (param.isAdmin == "true")
                    {
                        #region history
                        historyData.Number = skData.SKSPNumber;
                        historyData.Status = skData.Status;
                        historyData.Title = skData.Title;
                        historyData.RegulationSKSP = skData;
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
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim Document Control SKpts" + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Surat Keputusan dengan nomor " + skData.SKSPNumber + " tentang " + skData.Title + " telah diajukan, " +
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
                            logActivityEmail.Method = "insertSK";
                            logActivityEmail.Remark = "PROVE Business Logic";
                            logActivityEmail.Status = "Error";
                            logActivityEmail.UserName = param.username;
                            logActivityEmail.Controller = "PROVERegulationSKRepoImpl";
                            logActivityEmail.ErrorMessage = ex.Message;

                            await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                            await _proveContext.SaveChangesAsync();

                            #endregion
                        }

                        #region history
                        historyData.Number = skData.SKSPNumber;
                        historyData.Status = skData.Status;
                        historyData.Title = skData.Title;
                        historyData.RegulationSKSP = skData;
                        historyData.FileId = fileID;
                        historyData.Notes = param.notes;
                        historyData.CreatedBy = param.username;
                        historyData.UpdatedBy = param.username;

                        await _proveContext.RegulationHistory.AddAsync(historyData);
                        await _proveContext.SaveChangesAsync();
                        #endregion
                    }


                    #region log activity

                    logActivity.Info = "Success Transaction Insert";
                    logActivity.Method = "insertSK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSKRepoImpl";
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
                logActivity.Method = "insertSK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> uploadedSK(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP skData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSKSP.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();

                    skData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    fileID = skData.FileId;
                    fileSupportID = skData.FileSupportId;

                    skData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    skData.Number = param.nomor;
                    skData.ProductOfLawSKSP = productOfLawData;
                    skData.SaveCode = saveCodeData;
                    skData.Year = param.tahun;
                    skData.SKSPNumber = "Kpts" + "-" + skData.Number + "/" + skData.KBO + "/" + skData.Year + "-" + saveCodeData.Code;
                    skData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    skData.Status = GeneralConstant.PROVE_UPLOADED; //Uploaded
                    skData.Title = param.perihal;
                    skData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    skData.ExpiredDate = Convert.ToDateTime(param.expiredDate);
                    skData.UpdatedAt = DateTime.Now;
                    skData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + saveCodeData.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/file/",
                           //path: $"Profile/" + param.produkHukumId + "/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.file,
                           Flag: "PROVE_SK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + saveCodeData.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    skData.FileId = fileID;
                    skData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    #region send email
                    if (skData.ConceptorDirId != param.conceptorDirId)
                    {
                        var listEmp = await getListUser(skData.ConceptorDirId);

                        List<MailAddress> mailListUser = new List<MailAddress>();

                        foreach (var item in listEmp.Select(a => a.email))
                        {
                            mailListUser.Add(new MailAddress(item));
                        }

                        var subject = "PROVE : Uploaded - Pembuatan SKpts (" + skData.SKSPNumber + " - " + skData.Title + ") Telah Selesai";

                        var body =
                            "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                            "<p style='text-align:justify;'>Surat Keputusan dengan nomor " + skData.SKSPNumber + " tentang " + skData.Title + " sudah berhasil dibuat dan diunggah " +
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
                            logActivityEmail.Method = "uploadedSK";
                            logActivityEmail.Remark = "PROVE Business Logic";
                            logActivityEmail.Status = "Error";
                            logActivityEmail.UserName = param.username;
                            logActivityEmail.Controller = "PROVERegulationSKRepoImpl";
                            logActivityEmail.ErrorMessage = ex.Message;

                            await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                            await _proveContext.SaveChangesAsync();

                            #endregion
                        }
                    }
                    #endregion

                    #region history
                    historyData.Number = skData.SKSPNumber;
                    historyData.Status = skData.Status;
                    historyData.Title = skData.Title;
                    historyData.RegulationSKSP = skData;
                    historyData.FileId = fileID;
                    historyData.Notes = param.notes;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion


                    #region log activity

                    logActivity.Info = "Success Transaction Uploaded";
                    logActivity.Method = "uploadedSK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSKRepoImpl";
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
                logActivity.Method = "uploadedSK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> revisedSK(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP skData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSKSP.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();

                    skData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    fileID = skData.FileId;
                    fileSupportID = skData.FileSupportId;

                    skData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    skData.Number = param.nomor;
                    skData.ProductOfLawSKSP = productOfLawData;
                    skData.SaveCode = saveCodeData;
                    skData.Year = param.tahun;
                    skData.SKSPNumber = "Kpts" + "-" + skData.Number + "/" + skData.KBO + "/" + skData.Year + "-" + saveCodeData.Code;
                    skData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    skData.Status = GeneralConstant.PROVE_REVISED; // Revised
                    skData.Title = param.perihal;
                    skData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    skData.ExpiredDate = Convert.ToDateTime(param.expiredDate);
                    skData.UpdatedAt = DateTime.Now;
                    skData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + saveCodeData.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/file/",
                           //path: $"Profile/" + param.produkHukumId + "/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.file,
                           Flag: "PROVE_SK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + saveCodeData.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    skData.FileId = fileID;
                    skData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    //Kirim Email

                    #region send email
                    var emailQM = await getListQM();

                    List<MailAddress> mailListQM = new List<MailAddress>();

                    foreach (var item in emailQM)
                    {
                        mailListQM.Add(new MailAddress(item.email));
                    }

                    var subject = "PROVE : Revised - Pembuatan SKpts  (" + skData.SKSPNumber + " - " + skData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim Document Control SKpts" + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Surat Keputusan dengan nomor " + skData.SKSPNumber + " tentang " + skData.Title +
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
                        logActivityEmail.Method = "revisedSK";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }
                    #endregion

                    #region history
                    historyData.Number = skData.SKSPNumber;
                    historyData.Status = skData.Status;
                    historyData.Title = skData.Title;
                    historyData.RegulationSKSP = skData;
                    historyData.FileId = fileID;
                    historyData.Notes = param.notes;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Revised";
                    logActivity.Method = "revisedSK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSKRepoImpl";
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
                logActivity.Info = "Error Transaction Update";
                logActivity.Method = "revisedSK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> deleteSK(int id)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP skData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    skData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == id)
                                                               .FirstOrDefaultAsync();

                    skData.IsActive = GeneralConstant.NO;
                    skData.IsDeleted = GeneralConstant.YES;
                    skData.Status = GeneralConstant.PROVE_DELETED; //Deleted 
                    skData.UpdatedAt = DateTime.Now;
                    skData.UpdatedBy = "SYSTEM";

                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    #region history
                    historyData.Number = skData.SKSPNumber;
                    historyData.Status = skData.Status;
                    historyData.Title = skData.Title;
                    historyData.RegulationSKSP = skData;
                    historyData.FileId = skData.FileId;
                    historyData.Notes = "Deleted";
                    historyData.CreatedBy = "SYSTEM";
                    historyData.UpdatedBy = "SYSTEM";

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion


                    #region log activity

                    logActivity.Info = "Success Transaction Delete";
                    logActivity.Method = "deleteSK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = "SYSTEM";
                    logActivity.Controller = "PROVERegulationSKRepoImpl";
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
                logActivity.Method = "deleteSK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> approveSK(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP skData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    skData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = skData.FileId;
                    var fileSupportID = skData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + skData.SaveCode.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.file,
                           Flag: "PROVE_SK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + skData.SaveCode.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    skData.FileId = fileID;
                    skData.FileSupportId = fileSupportID;
                    skData.Status = GeneralConstant.PROVE_REVIEWED; //reviewed
                    skData.UpdatedAt = DateTime.Now;
                    skData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    #region history
                    historyData.Number = skData.SKSPNumber;
                    historyData.Status = skData.Status;
                    historyData.Title = skData.Title;
                    historyData.RegulationSKSP = skData;
                    historyData.Notes = param.notes;
                    historyData.FileId = skData.FileId;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Approve";
                    logActivity.Method = "approveSK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSKRepoImpl";
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
                logActivity.Method = "approveSK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> reviseSK(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP skData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    skData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = skData.FileId;
                    var fileSupportID = skData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + skData.SaveCode.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.file,
                           Flag: "SK_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "SK_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + skData.SaveCode.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.fileSupport,
                           Flag: "SK_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "SK_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    skData.FileId = fileID;
                    skData.FileSupportId = fileSupportID;
                    skData.Status = GeneralConstant.PROVE_REVISED; //revise
                    skData.UpdatedAt = DateTime.Now;
                    skData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email

                    //List<Employee> listEmp = new List<Employee>();
                    //string nameDirectorate = string.Empty;

                    //if (skData.ConceptorId == skData.ConceptorDirId)
                    //{
                    //    listEmp = await getEmployeeDivision(Convert.ToInt32(skData.ConceptorDivId == "0" ? skData.ConceptorDirId : skData.ConceptorDivId));
                    //    nameDirectorate = ""; //NormalizeEmployeePositionName(_proveContext.EmployeePositions.Where(a => a.Id == (skData.ConceptorDivId == 0 ? skData.ConceptorDirId : skData.ConceptorDivId)).Select(a => a.PositionName).FirstOrDefault());
                    //}
                    //else
                    //{
                    //    listEmp = await getEmployeeDivision(Convert.ToInt32(skData.ConceptorDivId == "0" ? skData.ConceptorId : skData.ConceptorDivId));
                    //    nameDirectorate = ""; //NormalizeEmployeePositionName(_proveContext.EmployeePositions.Where(a => a.Id == (skData.ConceptorDivId == 0 ? skData.ConceptorId : skData.ConceptorDivId)).Select(a => a.PositionName).FirstOrDefault());
                    //}

                    #region send email
                    var listEmp = await getListUser(skData.ConceptorDirId);

                    List<MailAddress> mailListUser = new List<MailAddress>();

                    foreach (var item in listEmp.Select(a => a.email))
                    {
                        mailListUser.Add(new MailAddress(item));
                    }

                    var subject = "STK : Need Revise Surat Keputusan (" + skData.SKSPNumber + " - " + skData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                        "<p style='text-align:justify;'>Pembuatan Surat Keputusan dengan nomor " + skData.SKSPNumber +
                        ", perlu dilakukan perbaikan dengan catatan : " +
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
                        logActivityEmail.Method = "rejectSK";
                        logActivityEmail.Remark = "STK Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "STKRegulationSKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;
                        logActivityEmail.CreatedAt = DateTime.Now;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }
                    #endregion

                    #region history
                    historyData.Number = skData.SKSPNumber;
                    historyData.Status = skData.Status;
                    historyData.Title = skData.Title;
                    historyData.RegulationSKSP = skData;
                    historyData.Notes = param.notes;
                    historyData.FileId = skData.FileId;
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
                    logActivity.Method = "rejectSK";
                    logActivity.Remark = "STK BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "STKRegulationSKRepoImpl";
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
                logActivity.Method = "rejectSK";
                logActivity.Remark = "STK Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "STKRegulationSKRepoImpl";
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

        public async Task<ReturnValues> reviewSK(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP skData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    skData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = skData.FileId;
                    var fileSupportID = skData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + skData.SaveCode.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.file,
                           Flag: "PROVE_SK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + skData.SaveCode.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    skData.FileId = fileID;
                    skData.FileSupportId = fileSupportID;
                    skData.JointReviewDate = Convert.ToDateTime(param.jointReviewDate);
                    skData.Status = GeneralConstant.PROVE_REVIEW; //on review
                    skData.UpdatedAt = DateTime.Now;
                    skData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email

                    //List<Employee> listEmp = new List<Employee>();
                    //string nameDirectorate = string.Empty;

                    //if (skData.ConceptorId == skData.ConceptorDirId)
                    //{
                    //    listEmp = await getEmployeeDivision(Convert.ToInt32(skData.ConceptorDivId == "0" ? skData.ConceptorDirId : skData.ConceptorDivId));
                    //    nameDirectorate = ""; //NormalizeEmployeePositionName(_proveContext.EmployeePositions.Where(a => a.Id == (skData.ConceptorDivId == 0 ? skData.ConceptorDirId : skData.ConceptorDivId)).Select(a => a.PositionName).FirstOrDefault());
                    //}
                    //else
                    //{
                    //    listEmp = await getEmployeeDivision(Convert.ToInt32(skData.ConceptorDivId == "0" ? skData.ConceptorId : skData.ConceptorDivId));
                    //    nameDirectorate = ""; //NormalizeEmployeePositionName(_proveContext.EmployeePositions.Where(a => a.Id == (skData.ConceptorDivId == 0 ? skData.ConceptorId : skData.ConceptorDivId)).Select(a => a.PositionName).FirstOrDefault());
                    //}

                    #region send email
                    var listEmp = await getListUser(skData.ConceptorDirId);

                    List<MailAddress> mailListUser = new List<MailAddress>();

                    foreach (var item in listEmp.Select(a => a.email))
                    {
                        mailListUser.Add(new MailAddress(item));
                    }

                    var subject = "PROVE : On Review - Pembuatan SKpts (" + skData.SKSPNumber + " - " + skData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Surat Keputusan dengan nomor " + skData.SKSPNumber + " tentang " + skData.Title +
                        " dalam proses review. Catatan : </p>" +
                        "<p style='text-align:justify;'>" +
                        param.notes +
                        //"<a href='https://shipchandler.ptk-shipping.com/Order/DetailsNew?id=" + orderData.Id + "'>Klik</a> disini untuk menuju detail order." +
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
                        logActivityEmail.Method = "reviewSK";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }
                    #endregion

                    #region history
                    historyData.Number = skData.SKSPNumber;
                    historyData.Status = skData.Status;
                    historyData.Title = skData.Title;
                    historyData.RegulationSKSP = skData;
                    historyData.Notes = param.notes;
                    historyData.FileId = skData.FileId;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Review";
                    logActivity.Method = "reviewSK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSKRepoImpl";
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
                logActivity.Method = "reviewSK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> needSignSK(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP skData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    skData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = skData.FileId;
                    var fileSupportID = skData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + skData.SaveCode.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.file,
                           Flag: "PROVE_SK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + skData.SaveCode.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    skData.FileId = fileID;
                    skData.FileSupportId = fileSupportID;
                    skData.Status = GeneralConstant.PROVE_NEED_SIGN; //need sign
                    skData.UpdatedAt = DateTime.Now;
                    skData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email

                    //List<Employee> listEmp = new List<Employee>();
                    //string nameDirectorate = string.Empty;

                    //if (skData.ConceptorId == skData.ConceptorDirId)
                    //{
                    //    listEmp = await getEmployeeDivision(Convert.ToInt32(skData.ConceptorDivId == "0" ? skData.ConceptorDirId : skData.ConceptorDivId));
                    //    nameDirectorate = ""; //NormalizeEmployeePositionName(_proveContext.EmployeePositions.Where(a => a.Id == (skData.ConceptorDivId == 0 ? skData.ConceptorDirId : skData.ConceptorDivId)).Select(a => a.PositionName).FirstOrDefault());
                    //}
                    //else
                    //{
                    //    listEmp = await getEmployeeDivision(Convert.ToInt32(skData.ConceptorDivId == "0" ? skData.ConceptorId : skData.ConceptorDivId));
                    //    nameDirectorate = ""; //NormalizeEmployeePositionName(_proveContext.EmployeePositions.Where(a => a.Id == (skData.ConceptorDivId == 0 ? skData.ConceptorId : skData.ConceptorDivId)).Select(a => a.PositionName).FirstOrDefault());
                    //}

                    #region send email
                    var listEmp = await getListUser(skData.ConceptorDirId);

                    List<MailAddress> mailListUser = new List<MailAddress>();

                    foreach (var item in listEmp.Select(a => a.email))
                    {
                        mailListUser.Add(new MailAddress(item));
                    }

                    var subject = "PROVE : Need Sign - Pembuatan SKpts (" + skData.SKSPNumber + " - " + skData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].email + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Surat Keputusan dengan nomor " + skData.SKSPNumber + " tentang " + skData.Title +
                        " dapat dilakukan serkuler perstujuannya kepada Pihak/Pejabat terkait. Catatan : </p>" +
                        "<p style='text-align:justify;'>" +
                        param.notes +
                        //"<a href='https://shipchandler.ptk-shipping.com/Order/DetailsNew?id=" + orderData.Id + "'>Klik</a> disini untuk menuju detail order." +
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
                        logActivityEmail.Method = "needSignSK";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }
                    #endregion

                    #region history
                    historyData.Number = skData.SKSPNumber;
                    historyData.Status = skData.Status;
                    historyData.Title = skData.Title;
                    historyData.RegulationSKSP = skData;
                    historyData.Notes = param.notes;
                    historyData.FileId = skData.FileId;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Need Sign";
                    logActivity.Method = "needSignSK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "ProveRegulationSKRepoImpl";
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
                logActivity.Method = "needSignSK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> signedSK(SKSPPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSKSP skData = new RegulationSKSP();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSKSP.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();

                    skData = await _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    fileID = skData.FileId;
                    fileSupportID = skData.FileSupportId;

                    skData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    skData.Number = param.nomor;
                    skData.ProductOfLawSKSP = productOfLawData;
                    skData.SaveCode = saveCodeData;
                    skData.Year = param.tahun;
                    skData.SKSPNumber = "Kpts" + "-" + skData.Number + "/" + skData.KBO + "/" + skData.Year + "-" + saveCodeData.Code;
                    skData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    skData.Status = GeneralConstant.PROVE_SIGNED; // Signed
                    skData.Title = param.perihal;
                    skData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    skData.ExpiredDate = Convert.ToDateTime(param.expiredDate);
                    skData.UpdatedAt = DateTime.Now;
                    skData.UpdatedBy = param.username;

                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + saveCodeData.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.file,
                           Flag: "PROVE_SK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"SK/KPTS/" + skData.KBO + "/" + saveCodeData.Code + "/" + skData.Number + "/" + skData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: skData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_SK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_SK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    skData.FileId = fileID;
                    skData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSKSP.Update(skData);
                    await _proveContext.SaveChangesAsync();

                    //Kirim Email

                    #region send email
                    var emailQM = await getListQM();

                    List<MailAddress> mailListQM = new List<MailAddress>();

                    foreach (var item in emailQM)
                    {
                        mailListQM.Add(new MailAddress(item.email));
                    }

                    var subject = "PROVE : Signed - Pembuatan SKpts  (" + skData.SKSPNumber + " - " + skData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim Document Control SKpts" + "</p><br/>" +
                        "<p style='text-align:justify;'>Surat Keputusan dengan nomor " + skData.SKSPNumber + " tentang " + skData.Title +
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
                        logActivityEmail.Method = "signedSK";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }
                    #endregion

                    #region history
                    historyData.Number = skData.SKSPNumber;
                    historyData.Status = skData.Status;
                    historyData.Title = skData.Title;
                    historyData.RegulationSKSP = skData;
                    historyData.FileId = fileID;
                    historyData.Notes = param.notes;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Signed";
                    logActivity.Method = "signedSK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSKRepoImpl";
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
                logActivity.Method = "signedSK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        #region commend qm
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
