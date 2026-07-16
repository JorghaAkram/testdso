using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao;
using Prove.Data.Dao.Prove;
using Prove.Data.Data;
using Prove.Data.Services.FileUpload.Interface;
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
    public class RegulationSTKRepoImpl : IRegulationSTKRepo
    {
        private readonly ProveExtContext _proveContext;
        private readonly UsmanContext _usmanContext;
        private readonly IFileUploadRepo _fileUploadService;
        private readonly ISendMail _mailService;
        private readonly IHttpClientFactory _httpClient;

        public RegulationSTKRepoImpl(ProveExtContext proveContext, IFileUploadRepo fileUploadService, ISendMail mailService, IHttpClientFactory httpClient, UsmanContext usmanContext)
        {
            _proveContext = proveContext;
            _fileUploadService = fileUploadService;
            _mailService = mailService;
            _httpClient = httpClient;
            _usmanContext = usmanContext;
        }

        public async Task<BaseDTResponseModel<STKJtable>> getSTKDatatable(STKJtableParam param)
        {
            try
            {
                IQueryable<STKJtable> itemData = null;
                List<STKJtable> filteredData = new List<STKJtable>();
                //List<EmployeePosition> positionData = new List<EmployeePosition>();
                IQueryable<RegulationSTK> allItemData = null;

                if (param.isAdmin == "true")
                {
                    allItemData = _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                                   .ThenInclude(a => a.Type)
                                                                   .Include(a => a.Type)
                                                                   .Include(a => a.SaveCode)
                                                                   .Include(a => a.Probis)
                                                                   .Where(c => c.IsActive == GeneralConstant.YES
                                                                            && c.IsDeleted == GeneralConstant.NO
                                                                            && c.CompanyCode == param.companyCode)
                                                                   .OrderByDescending(d => d.CreatedAt);
                }
                else
                {
                    //if (param.conceptorDirId == param.conceptorId)
                    //{
                    //    allItemData = _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                    //                                                   .ThenInclude(a => a.Type)
                    //                                                   .Include(a => a.Type)
                    //                                                   .Include(a => a.SaveCode)
                    //                                                   .Include(a => a.Probis)
                    //                                                   .Where(c => c.IsActive == GeneralConstant.YES
                    //                                                            && c.IsDeleted == GeneralConstant.NO
                    //                                                            && (param.conceptorDivId == "0" ? c.ConceptorDirId == param.conceptorDirId : c.ConceptorDivId == param.conceptorDivId)
                    //                                                            && c.CompanyCode == param.companyCode)
                    //                                                   .OrderByDescending(d => d.CreatedAt);
                    //}
                    //else
                    //{
                    //    allItemData = _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                    //                                                   .ThenInclude(a => a.Type)
                    //                                                   .Include(a => a.Type)
                    //                                                   .Include(a => a.SaveCode)
                    //                                                   .Include(a => a.Probis)
                    //                                                   .Where(c => c.IsActive == GeneralConstant.YES
                    //                                                            && c.IsDeleted == GeneralConstant.NO
                    //                                                            && (param.conceptorDivId == "0" ? c.ConceptorId == param.conceptorId : c.ConceptorDivId == param.conceptorDivId)
                    //                                                            && c.CompanyCode == param.companyCode)
                    //                                                   .OrderByDescending(d => d.CreatedAt);
                    //}

                    allItemData = _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                                       .ThenInclude(a => a.Type)
                                                                       .Include(a => a.Type)
                                                                       .Include(a => a.SaveCode)
                                                                       .Include(a => a.Probis)
                                                                       .Where(c => c.IsActive == GeneralConstant.YES
                                                                                && c.IsDeleted == GeneralConstant.NO
                                                                                && c.ConceptorDirId == param.conceptorDirId)
                                                                       .OrderByDescending(d => d.CreatedAt);

                }

                if (param.PageSize < 1)
                    return new BaseDTResponseModel<STKJtable>
                    {
                        Data = new List<STKJtable>(),
                        Draw = param.Draw,
                        RecordsFiltered = 0,
                        RecordsTotal = allItemData.Count()
                    };

                itemData = (from a in allItemData
                            where (string.IsNullOrWhiteSpace(param.nomor) || a.STKNumber.ToLower().Contains(param.nomor.ToLower()))
                               && (string.IsNullOrWhiteSpace(param.perihal) || (a.Title != null ? a.Title.ToLower().Contains(param.perihal.ToLower()) : a.Title == param.perihal))
                               && (param.jenis == 0 || a.ProductOfLawSTK.Id == param.jenis)
                               && (param.rev == 0 || a.RevisedFlag == param.rev)
                               && (string.IsNullOrWhiteSpace(param.status) || (a.Status != null ? a.Status.ToLower().Contains(param.status.ToLower()) : a.Status == param.status))
                               && (string.IsNullOrWhiteSpace(param.fungsi) || a.PositionId == param.fungsi)
                               && (param.tmtBerlaku == null || a.TmtApplies == param.tmtBerlaku)
                            select new STKJtable
                            {
                                Id = a.Id,
                                tmtBerlaku = a.TmtApplies,
                                //regulationCategoryId = (int)a.RegCategoryId,
                                //regulationCategoryName = a.RegCategoryId.ToString(),
                                fileId = a.FileId,
                                fileSupportId = a.FileSupportId,
                                //kbo = a.KBO,
                                //kodeSimpan = a.SaveCode.Code,
                                //kodeSimpanId = a.SaveCode.Id,
                                //nomor = a.Number,
                                //nomorSerial = a.SerialNumberSTK,
                                nomorSTK = a.STKNumber,
                                positionName = a.PositionId,
                                perihal = a.Title,
                                //positionNumber = a.PositionId,
                                produkHukumName = a.ProductOfLawSTK.Description,
                                //produkHukumType = a.ProductOfLawSTK.Type.Code,
                                produkHukumId = a.ProductOfLawSTK.Id,
                                //refNumber = a.RefNumber,
                                status = a.Status,
                                //statusDoc = a.StatusDocId.ToString(),
                                //statusDocId = (int)a.StatusDocId,
                                //jenis = a.Type.Code,
                                //positionCode = a.PositionCode,
                                //probis = a.Probis.Description,
                                //probisId = a.Probis.Id,
                                //probisNumber = a.ProbisNumber,
                                revisedFlag = a.RevisedFlag,
                                year = a.Year,
                                CreatedDate = a.CreatedAt,
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

                //positionData = await _proveContext.EmployeePositions.Include(a => a.Organization)
                //                                                   .Include(b => b.EmployeePositionAbbrv)
                //                                                   .Include(c => c.PAreaSub)
                //                                                   .Where(d => d.IsActive == GeneralConstant.YES
                //                                                            && d.IsDeleted == GeneralConstant.NO && filteredData.Select(c => int.Parse(c.positionName)).Any(c => c == d.Id))
                //                                                   .ToListAsync();

                

                var posData = await GetOrganization(param.companyCode);

                filteredData.ForEach(b =>
                {
                    b.positionName = posData.FirstOrDefault(c => c.id == b.positionName).name;
                });

                BaseDTResponseModel<STKJtable> result = new BaseDTResponseModel<STKJtable>
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

        public async Task<BaseDTResponseModel<ReportSTKJtable>> getSTKList(string CompanyCode)
        {
            try
            {
                List<ReportSTKJtable> itemData = new List<ReportSTKJtable>();
                List<ReportSTKJtable> filteredData = new List<ReportSTKJtable>();
                //List<EmployeePosition> positionData = new List<EmployeePosition>();
                List<RegulationSTK> allItemData = new List<RegulationSTK>();

                allItemData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(a => a.Type)
                                                               .Include(a => a.SaveCode)
                                                               .Include(a => a.Probis)
                                                               .Where(c => c.IsActive == GeneralConstant.YES
                                                                        && c.IsDeleted == GeneralConstant.NO
                                                                        && c.CompanyCode == CompanyCode)
                                                               .OrderByDescending(d => d.CreatedAt)
                                                               .ToListAsync();

                //positionData = await _proveContext.EmployeePositions.Include(a => a.Organization)
                //                                                   .Include(b => b.EmployeePositionAbbrv)
                //                                                   .Include(c => c.PAreaSub)
                //                                                   .Where(d => d.IsActive == GeneralConstant.YES
                //                                                            && d.IsDeleted == GeneralConstant.NO)
                //                                                   .ToListAsync();

                itemData = (from a in allItemData
                            select new ReportSTKJtable
                            {
                                Id = a.Id,
                                tmtBerlaku = a.TmtApplies,
                                regulationCategoryId = (int)a.RegCategoryId,
                                regulationCategoryName = a.RegCategoryId.ToString(),
                                fileId = a.FileId,
                                fileSupportId = a.FileSupportId,
                                kbo = a.KBO,
                                kodeSimpan = a.SaveCode.Code,
                                kodeSimpanId = a.SaveCode.Id,
                                nomor = a.Number,
                                nomorSerial = a.SerialNumberSTK,
                                nomorSTK = a.STKNumber,
                                perihal = a.Title,
                                positionNumber = a.PositionId,
                                //positionName = cf == null ? string.Empty : cf.PositionName,
                                produkHukumName = a.ProductOfLawSTK.Description,
                                produkHukumId = a.ProductOfLawSTK.Id,
                                status = a.Status,
                                statusDoc = a.StatusDocId.ToString(),
                                statusDocId = (int)a.StatusDocId,
                                jenis = a.Type.Code,
                                positionCode = a.PositionCode,
                                probis = a.Probis.Description,
                                probisId = a.Probis.Id,
                                probisNumber = a.ProbisNumber,
                                revisedFlag = a.RevisedFlag,
                                year = a.Year,
                                CreatedDate = a.CreatedAt,
                                CreatedBy = a.CreatedBy
                            }).ToList();


                //var posData = await GetOrganization(param.companyCode);

                //filteredData.ForEach(b =>
                //{
                //    b.positionName = posData.FirstOrDefault(c => c.id == b.positionName).name;
                //});

                BaseDTResponseModel<ReportSTKJtable> result = new BaseDTResponseModel<ReportSTKJtable>
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

        public async Task<List<HistoryModel>> getRegulationSTKHistory(int id)
        {
            try
            {
                ActivityLog logActivity = new ActivityLog();
                List<RegulationHistory> historyData = new List<RegulationHistory>();
                List<HistoryModel> retHistory = new List<HistoryModel>();

                historyData = await _proveContext.RegulationHistory.Include(a => a.RegulationSTK)
                                                                   .Where(b => b.RegulationSTK.Id == id)
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
                logActivity.Remark = "STK BusinessLogic";
                logActivity.Status = "Success";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "STKRegulationSKRepoImpl";
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

        public async Task<RegulationSTK> detailSTK(int id, CancellationToken cancellationToken)
            => await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                .ThenInclude(a => a.Type)
                                                .Include(a => a.Type)
                                                .Include(a => a.SaveCode)
                                                .Include(a => a.Probis).FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        public async Task<ReturnValues> insertSTK(STKPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSTK stkData = new RegulationSTK();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSTK.Include(a => a.Type).Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();
                    var typeData = await _proveContext.STKType.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var probisData = await _proveContext.Probis.Where(a => a.Id == Convert.ToInt32(param.probisId)).FirstOrDefaultAsync();

                    stkData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    stkData.FileId = fileID;
                    stkData.FileSupportId = fileSupportID;
                    stkData.KBO = param.kbo;
                    stkData.Number = param.nomor;
                    stkData.SerialNumberSTK = param.nomorSerial;
                    stkData.PositionId = param.fungsi;
                    stkData.ProductOfLawSTK = productOfLawData;
                    stkData.RefNumber = param.nomorRef;
                    stkData.SaveCode = saveCodeData;
                    stkData.Year = param.tahun;
                    stkData.STKNumber = productOfLawData.Type.Code + stkData.Number + "-" + stkData.SerialNumberSTK + "/" + stkData.KBO + "/" + stkData.Year + "-" + saveCodeData.Code;
                    stkData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    if (param.isAdmin == "true")
                    {
                        stkData.Status = GeneralConstant.PROVE_UPLOADED; // Uploaded
                    }
                    else
                    {
                        stkData.Status = GeneralConstant.PROVE_REQUEST; // New Request
                    }
                    stkData.Title = param.perihal;
                    stkData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    stkData.Type = typeData;
                    stkData.RevisedFlag = Convert.ToInt32(param.revisiKe);
                    stkData.PositionCode = param.fungsi;
                    stkData.Probis = probisData;
                    stkData.ProbisNumber = probisData.ChildProbisNumber.ToString();
                    stkData.ConceptorId = param.conceptorId;
                    stkData.ConceptorDirId = param.conceptorDirId;
                    stkData.ConceptorDivId = param.conceptorDivId;
                    stkData.CompanyCode = param.companyCode;
                    stkData.CreatedBy = param.username;
                    stkData.UpdatedBy = param.username;

                    await _proveContext.RegulationSTK.AddAsync(stkData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + saveCodeData.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.file,
                           Flag: "PROVE_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + saveCodeData.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    stkData.FileId = fileID;
                    stkData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    //kirim email ke QM untuk di review

                    #region send email
                    var emailQM = await getListQM();

                    List<MailAddress> mailList = new List<MailAddress>();

                    foreach (var item in emailQM)
                    {
                        mailList.Add(new MailAddress(item.email));
                    }

                    var subject = "PROVE : Request - Pembuatan STK (" + stkData.STKNumber + " - " + stkData.Title + ")";

                    if (param.isAdmin == "true")
                    {
                        #region history
                        historyData.Number = stkData.STKNumber;
                        historyData.Status = stkData.Status;
                        historyData.Title = stkData.Title;
                        historyData.RegulationSTK = stkData;
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
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim Document Control STK" + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Sistem Tata Kerja dengan nomor " + stkData.STKNumber + " tentang " + stkData.Title + " telah diajukan, " +
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
                            logActivityEmail.Method = "insertSTK";
                            logActivityEmail.Remark = "PROVE Business Logic";
                            logActivityEmail.Status = "Error";
                            logActivityEmail.UserName = param.username;
                            logActivityEmail.Controller = "PROVERegulationSTKRepoImpl";
                            logActivityEmail.ErrorMessage = ex.Message;

                            await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                            await _proveContext.SaveChangesAsync();

                            #endregion
                        }

                        #region history
                        historyData.Number = stkData.STKNumber;
                        historyData.Status = stkData.Status;
                        historyData.Title = stkData.Title;
                        historyData.RegulationSTK = stkData;
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
                    logActivity.Method = "insertSTK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSTKRepoImpl";
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
                logActivity.Method = "insertSTK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSTKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> uploadedSTK(STKPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSTK stkData = new RegulationSTK();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSTK.Include(a => a.Type).Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();
                    var typeData = await _proveContext.STKType.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var probisData = await _proveContext.Probis.Where(a => a.Id == Convert.ToInt32(param.probisId)).FirstOrDefaultAsync();

                    stkData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    fileID = stkData.FileId;
                    fileSupportID = stkData.FileSupportId;

                    stkData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    stkData.Number = param.nomor;
                    stkData.SerialNumberSTK = param.nomorSerial;
                    stkData.PositionId = param.fungsi;
                    stkData.ProductOfLawSTK = productOfLawData;
                    stkData.RefNumber = param.nomorRef;
                    stkData.SaveCode = saveCodeData;
                    stkData.Year = param.tahun;
                    stkData.STKNumber = productOfLawData.Type.Code + stkData.Number + "-" + stkData.SerialNumberSTK + "/" + stkData.KBO + "/" + stkData.Year + "-" + saveCodeData.Code;
                    stkData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    stkData.Status = GeneralConstant.PROVE_UPLOADED; // Uploaded
                    stkData.Title = param.perihal;
                    stkData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    stkData.Type = typeData;
                    stkData.RevisedFlag = Convert.ToInt32(param.revisiKe);
                    stkData.PositionCode = param.fungsi;
                    stkData.Probis = probisData;
                    stkData.ProbisNumber = probisData.ChildProbisNumber.ToString();
                    stkData.UpdatedAt = DateTime.Now;
                    stkData.UpdatedBy = param.username;

                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + saveCodeData.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.file,
                           Flag: "PROVE_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + saveCodeData.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    stkData.FileId = fileID;
                    stkData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    #region send email
                    if (stkData.ConceptorDirId != param.conceptorDirId)
                    {
                        var listEmp = await getListUser(stkData.ConceptorDirId);

                        List<MailAddress> mailListUser = new List<MailAddress>();

                        foreach (var item in listEmp.Select(a => a.email))
                        {
                            mailListUser.Add(new MailAddress(item));
                        }

                        var subject = "PROVE : Uploaded - Pembuatan STK (" + stkData.STKNumber + " - " + stkData.Title + ")";

                        var body =
                            "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                            "<p style='text-align:justify;'>Sistem Tata Kerja dengan nomor " + stkData.STKNumber + " tentang " + stkData.Title + " sudah berhasil dibuat dan diunggah " +
                            "di Aplikasi PROVE pada menu Final Document. Catatan :</p>" +
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
                            logActivityEmail.Method = "uploadedSTK";
                            logActivityEmail.Remark = "PROVE Business Logic";
                            logActivityEmail.Status = "Error";
                            logActivityEmail.UserName = param.username;
                            logActivityEmail.Controller = "PROVERegulationSTKRepoImpl";
                            logActivityEmail.ErrorMessage = ex.Message;

                            await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                            await _proveContext.SaveChangesAsync();

                            #endregion
                        }
                    }
                    #endregion

                    #region history
                    historyData.Number = stkData.STKNumber;
                    historyData.Status = stkData.Status;
                    historyData.Title = stkData.Title;
                    historyData.RegulationSTK = stkData;
                    historyData.FileId = fileID;
                    historyData.Notes = param.notes;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion


                    #region log activity

                    logActivity.Info = "Success Transaction Upload";
                    logActivity.Method = "uploadedSTK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSTKRepoImpl";
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
                logActivity.Info = "Error Transaction Upload";
                logActivity.Method = "uploadSTK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSTKRepoImpl";
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

        public async Task<ReturnValues> revisedSTK(STKPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSTK stkData = new RegulationSTK();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSTK.Include(a => a.Type).Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();
                    var typeData = await _proveContext.STKType.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var probisData = await _proveContext.Probis.Where(a => a.Id == Convert.ToInt32(param.probisId)).FirstOrDefaultAsync();

                    stkData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    fileID = stkData.FileId;
                    fileSupportID = stkData.FileSupportId;

                    stkData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    stkData.Number = param.nomor;
                    stkData.SerialNumberSTK = param.nomorSerial;
                    stkData.PositionId = param.fungsi;
                    stkData.ProductOfLawSTK = productOfLawData;
                    stkData.RefNumber = param.nomorRef;
                    stkData.SaveCode = saveCodeData;
                    stkData.Year = param.tahun;
                    stkData.STKNumber = productOfLawData.Type.Code + stkData.Number + "-" + stkData.SerialNumberSTK + "/" + stkData.KBO + "/" + stkData.Year + "-" + saveCodeData.Code;
                    stkData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    stkData.Status = GeneralConstant.PROVE_REVISED; // Revised
                    stkData.Title = param.perihal;
                    stkData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    stkData.Type = typeData;
                    stkData.RevisedFlag = Convert.ToInt32(param.revisiKe);
                    stkData.PositionCode = param.fungsi;
                    stkData.Probis = probisData;
                    stkData.ProbisNumber = probisData.ChildProbisNumber.ToString();
                    stkData.UpdatedAt = DateTime.Now;
                    stkData.UpdatedBy = param.username;

                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + saveCodeData.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.file,
                           Flag: "PROVE_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + saveCodeData.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    stkData.FileId = fileID;
                    stkData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    //Kirim Email
                    #region send email
                    var emailQM = await getListQM();

                    List<MailAddress> mailListQM = new List<MailAddress>();

                    foreach (var item in emailQM)
                    {
                        mailListQM.Add(new MailAddress(item.email));
                    }

                    var subject = "PROVE : Revised - Pembuatan STK (" + stkData.STKNumber + " - " + stkData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim Document Control STK" + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Sistem Tata Kerja dengan nomor " + stkData.STKNumber + " tentang " + stkData.Title +
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
                        logActivityEmail.Method = "revisedSTK";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSTKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }

                    #endregion

                    #region history
                    historyData.Number = stkData.STKNumber;
                    historyData.Status = stkData.Status;
                    historyData.Title = stkData.Title;
                    historyData.RegulationSTK = stkData;
                    historyData.FileId = fileID;
                    historyData.Notes = param.notes;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Revised";
                    logActivity.Method = "revisedSTK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSTKRepoImpl";
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
                logActivity.Info = "Error Transaction Revised";
                logActivity.Method = "revisedSTK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSTKRepoImpl";
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

        public async Task<ReturnValues> deleteSTK(int id)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSTK stkData = new RegulationSTK();
                    RegulationHistory historyData = new RegulationHistory();

                    stkData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == id)
                                                               .FirstOrDefaultAsync();

                    stkData.IsActive = GeneralConstant.NO;
                    stkData.IsDeleted = GeneralConstant.YES;
                    stkData.Status = GeneralConstant.PROVE_DELETED; //Deleted
                    stkData.UpdatedAt = DateTime.Now;
                    stkData.UpdatedBy = "SYSTEM";

                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    #region history
                    historyData.Number = stkData.STKNumber;
                    historyData.Status = stkData.Status;
                    historyData.Title = stkData.Title;
                    historyData.RegulationSTK = stkData;
                    historyData.FileId = stkData.FileId;
                    historyData.Notes = "Deleted";
                    historyData.CreatedBy = "SYSTEM";
                    historyData.UpdatedBy = "SYSTEM";

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Delete";
                    logActivity.Method = "deleteSTK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = "SYSTEM";
                    logActivity.Controller = "PROVERegulationSTKRepoImpl";
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
                logActivity.Method = "deleteSTK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = "SYSTEM";
                logActivity.Controller = "PROVERegulationSTKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> approveSTK(STKPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSTK stkData = new RegulationSTK();
                    RegulationHistory historyData = new RegulationHistory();

                    stkData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = stkData.FileId;
                    var fileSupportID = stkData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + stkData.SaveCode.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.file,
                           Flag: "PROVE_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + stkData.SaveCode.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    stkData.FileId = fileID;
                    stkData.FileSupportId = fileSupportID;
                    stkData.Status = GeneralConstant.PROVE_REVIEWED; //reviewed Prod
                    stkData.UpdatedAt = DateTime.Now;
                    stkData.UpdatedBy = param.username;

                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    #region history
                    historyData.Number = stkData.STKNumber;
                    historyData.Status = stkData.Status;
                    historyData.Title = stkData.Title;
                    historyData.RegulationSTK = stkData;
                    historyData.Notes = param.notes;
                    historyData.FileId = stkData.FileId;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Approve";
                    logActivity.Method = "approveSTK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSTKRepoImpl";
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
                logActivity.Method = "approveSTK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSTKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> reviseSTK(STKPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = _proveContext.Database.BeginTransaction())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSTK stkData = new RegulationSTK();
                    RegulationHistory historyData = new RegulationHistory();

                    stkData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    var fileID = stkData.FileId;
                    var fileSupportID = stkData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + stkData.SaveCode.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.file,
                           Flag: "STK_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "STK_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + stkData.SaveCode.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.fileSupport,
                           Flag: "STK_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "STK_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    stkData.FileId = fileID;
                    stkData.FileSupportId = fileSupportID;
                    stkData.Status = GeneralConstant.PROVE_REVISED; //revise
                    stkData.UpdatedAt = DateTime.Now;
                    stkData.UpdatedBy = param.username;

                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    #region send email
                    var listEmp = await getListUser(stkData.ConceptorDirId);

                    List<MailAddress> mailListUser = new List<MailAddress>();

                    foreach (var item in listEmp.Select(a => a.email))
                    {
                        mailListUser.Add(new MailAddress(item));
                    }

                    var subject = "STK : Need Revise Sistem Tata Kerja (" + stkData.STKNumber + " - " + stkData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                        "<p style='text-align:justify;'>Pembuatan Sistem Tata Kerja dengan nomor " + stkData.STKNumber +
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
                        logActivityEmail.Method = "rejectSTK";
                        logActivityEmail.Remark = "STK Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "STKRegulationSTKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;
                        logActivityEmail.CreatedAt = DateTime.Now;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }

                    #endregion

                    #region history
                    historyData.Number = stkData.STKNumber;
                    historyData.Status = stkData.Status;
                    historyData.Title = stkData.Title;
                    historyData.RegulationSTK = stkData;
                    historyData.Notes = param.notes;
                    historyData.FileId = stkData.FileId;
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
                    logActivity.Method = "rejectSTK";
                    logActivity.Remark = "STK BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "STKRegulationSTKRepoImpl";
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
                logActivity.Method = "rejectSTK";
                logActivity.Remark = "STK Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "STKRegulationSTKRepoImpl";
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

        public async Task<ReturnValues> reviewSTK(STKPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSTK stkData = new RegulationSTK();
                    RegulationHistory historyData = new RegulationHistory();



                    stkData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();
                    var fileID = stkData.FileId;
                    var fileSupportID = stkData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + stkData.SaveCode.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.file,
                           Flag: "PROVE_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + stkData.SaveCode.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    stkData.FileId = fileID;
                    stkData.FileSupportId = fileSupportID;
                    stkData.JointReviewDate = Convert.ToDateTime(param.jointReviewDate);
                    stkData.Status = GeneralConstant.PROVE_REVIEW; //review
                    stkData.UpdatedAt = DateTime.Now;
                    stkData.UpdatedBy = param.username;

                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    #region send email
                    var listEmp = await getListUser(stkData.ConceptorDirId);

                    List<MailAddress> mailListUser = new List<MailAddress>();

                    foreach (var item in listEmp.Select(a => a.email))
                    {
                        mailListUser.Add(new MailAddress(item));
                    }

                    var subject = "PROVE : On Review - Pembuatan STK (" + stkData.STKNumber + " - " + stkData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Sistem Tata Kerja dengan nomor " + stkData.STKNumber + " tentang " + stkData.Title +
                        " dalam proses review. Catatan : </p> " +
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
                        logActivityEmail.Method = "reviewSTK";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSTKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }

                    #endregion

                    #region history
                    historyData.Number = stkData.STKNumber;
                    historyData.Status = stkData.Status;
                    historyData.Title = stkData.Title;
                    historyData.RegulationSTK = stkData;
                    historyData.Notes = param.notes;
                    historyData.FileId = stkData.FileId;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Review";
                    logActivity.Method = "reviewSTK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSTKRepoImpl";
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
                logActivity.Method = "reviewSTK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSTKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> needSignSTK(STKPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSTK stkData = new RegulationSTK();
                    RegulationHistory historyData = new RegulationHistory();

                    stkData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();
                    var fileID = stkData.FileId;
                    var fileSupportID = stkData.FileSupportId;

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + stkData.SaveCode.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.file,
                           Flag: "PROVE_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + stkData.SaveCode.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    stkData.FileId = fileID;
                    stkData.FileSupportId = fileSupportID;
                    stkData.Status = GeneralConstant.PROVE_NEED_SIGN; //need sign
                    stkData.UpdatedAt = DateTime.Now;
                    stkData.UpdatedBy = param.username;

                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    #region send email
                    var listEmp = await getListUser(stkData.ConceptorDirId);

                    List<MailAddress> mailListUser = new List<MailAddress>();

                    foreach (var item in listEmp.Select(a => a.email))
                    {
                        mailListUser.Add(new MailAddress(item));
                    }

                    var subject = "PROVE : Need Sign - Pembuatan STK (" + stkData.STKNumber + " - " + stkData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim " + listEmp[0].division + "</p><br/>" +
                        "<p style='text-align:justify;'>Draft Sistem Tata Kerja dengan nomor " + stkData.STKNumber + " tentang " + stkData.Title +
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
                        logActivityEmail.Method = "needSignSTK";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSTKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }

                    #endregion

                    #region history
                    historyData.Number = stkData.STKNumber;
                    historyData.Status = stkData.Status;
                    historyData.Title = stkData.Title;
                    historyData.RegulationSTK = stkData;
                    historyData.Notes = param.notes;
                    historyData.FileId = stkData.FileId;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Need Sign";
                    logActivity.Method = "needSignSTK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSTKRepoImpl";
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
                logActivity.Method = "needSignSTK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSTKRepoImpl";
                logActivity.ErrorMessage = ex.Message;

                await _proveContext.ActivityLog.AddAsync(logActivity);
                await _proveContext.SaveChangesAsync();

                #endregion

                retVal.ret = false;
                retVal.retMessage = ex.Message.ToString();

                return retVal;
            }
        }

        public async Task<ReturnValues> signedSTK(STKPost param)
        {
            ReturnValues retVal = new ReturnValues();
            try
            {
                using (var transaction = await _proveContext.Database.BeginTransactionAsync())
                {
                    ActivityLog logActivity = new ActivityLog();
                    RegulationSTK stkData = new RegulationSTK();
                    RegulationHistory historyData = new RegulationHistory();

                    var fileID = 0;
                    var fileSupportID = 0;

                    var productOfLawData = await _proveContext.ProductOfLawSTK.Include(a => a.Type).Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var saveCodeData = await _proveContext.SaveCode.Where(a => a.Id == Convert.ToInt32(param.kodeSimpan)).FirstOrDefaultAsync();
                    var typeData = await _proveContext.STKType.Where(a => a.Id == Convert.ToInt32(param.produkHukumId)).FirstOrDefaultAsync();
                    var probisData = await _proveContext.Probis.Where(a => a.Id == Convert.ToInt32(param.probisId)).FirstOrDefaultAsync();

                    stkData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(b => b.SaveCode)
                                                               .Where(c => c.Id == Convert.ToInt32(param.id))
                                                               .FirstOrDefaultAsync();

                    fileID = stkData.FileId;
                    fileSupportID = stkData.FileSupportId;

                    stkData.RegCategoryId = (RegulationCategory)Convert.ToInt32(param.categoryId);
                    stkData.Number = param.nomor;
                    stkData.SerialNumberSTK = param.nomorSerial;
                    stkData.PositionId = param.fungsi;
                    stkData.ProductOfLawSTK = productOfLawData;
                    stkData.RefNumber = param.nomorRef;
                    stkData.SaveCode = saveCodeData;
                    stkData.Year = param.tahun;
                    stkData.STKNumber = productOfLawData.Type.Code + stkData.Number + "-" + stkData.SerialNumberSTK + "/" + stkData.KBO + "/" + stkData.Year + "-" + saveCodeData.Code;
                    stkData.StatusDocId = (StatusDoc)Convert.ToInt32(param.statusDoc);
                    stkData.Status = GeneralConstant.PROVE_SIGNED; // Revised
                    stkData.Title = param.perihal;
                    stkData.TmtApplies = Convert.ToDateTime(param.tmtBerlaku);
                    stkData.Type = typeData;
                    stkData.RevisedFlag = Convert.ToInt32(param.revisiKe);
                    stkData.PositionCode = param.fungsi;
                    stkData.Probis = probisData;
                    stkData.ProbisNumber = probisData.ChildProbisNumber.ToString();
                    stkData.UpdatedAt = DateTime.Now;
                    stkData.UpdatedBy = param.username;

                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    if (param.file != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + saveCodeData.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.file,
                           Flag: "PROVE_STK",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK");
                        FileUpload data = FileData;
                        fileID = FileData.Id;
                    }

                    if (param.fileSupport != null)
                    {
                        FileUpload FileData = await _fileUploadService.UploadFileWithReturn(
                           path: $"STK/" + stkData.KBO + "/" + saveCodeData.Code + "/" + stkData.Number + "/" + stkData.ConceptorId + "/support/file/",
                           createBy: param.username,
                           trxId: stkData.Id,
                           file: param.fileSupport,
                           Flag: "PROVE_STK_Support",
                           autoRename: true,
                           isUpdate: true,
                           remark: "PROVE_STK_Support");
                        FileUpload data = FileData;
                        fileSupportID = FileData.Id;
                    }

                    stkData.FileId = fileID;
                    stkData.FileSupportId = fileSupportID;
                    _proveContext.RegulationSTK.Update(stkData);
                    await _proveContext.SaveChangesAsync();

                    //Kirim Email
                    #region send email
                    var emailQM = await getListQM();

                    List<MailAddress> mailListQM = new List<MailAddress>();

                    foreach (var item in emailQM)
                    {
                        mailListQM.Add(new MailAddress(item.email));
                    }

                    var subject = "PROVE : Signed - Pembuatan STK (" + stkData.STKNumber + " - " + stkData.Title + ")";

                    var body =
                        "<p style='text-align:justify;'>Kepada Yth, <br>" + "Tim Document Control STK" + "</p><br/>" +
                        "<p style='text-align:justify;'>Sistem Tata Kerja dengan nomor " + stkData.STKNumber + " tentang " + stkData.Title +
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
                        logActivityEmail.Method = "signedSTK";
                        logActivityEmail.Remark = "PROVE Business Logic";
                        logActivityEmail.Status = "Error";
                        logActivityEmail.UserName = param.username;
                        logActivityEmail.Controller = "PROVERegulationSTKRepoImpl";
                        logActivityEmail.ErrorMessage = ex.Message;

                        await _proveContext.ActivityLog.AddAsync(logActivityEmail);
                        await _proveContext.SaveChangesAsync();

                        #endregion
                    }

                    #endregion

                    #region history
                    historyData.Number = stkData.STKNumber;
                    historyData.Status = stkData.Status;
                    historyData.Title = stkData.Title;
                    historyData.RegulationSTK = stkData;
                    historyData.FileId = fileID;
                    historyData.Notes = param.notes;
                    historyData.CreatedBy = param.username;
                    historyData.UpdatedBy = param.username;

                    await _proveContext.RegulationHistory.AddAsync(historyData);
                    await _proveContext.SaveChangesAsync();
                    #endregion

                    #region log activity

                    logActivity.Info = "Success Transaction Revised";
                    logActivity.Method = "signedSTK";
                    logActivity.Remark = "PROVE BusinessLogic";
                    logActivity.Status = "Success";
                    logActivity.UserName = param.username;
                    logActivity.Controller = "PROVERegulationSTKRepoImpl";
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
                logActivity.Method = "signedSTK";
                logActivity.Remark = "PROVE Business Logic";
                logActivity.Status = "Error";
                logActivity.UserName = param.username;
                logActivity.Controller = "PROVERegulationSTKRepoImpl";
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

        //        List<Employee> dataEmployee = _employeeService.GetAllIncludingWithFilter(
        //            b => b.IsDeleted == PrideConstant.NO &&
        //            tempEmpId.Any(c => c == b.Id) &&
        //            b.EmployeePosition.EmployeePositionAbbrv.Id <= 7,
        //            b => b.EmployeePosition,
        //            b => b.Religion,
        //            b => b.EmployeePosition.EmployeePositionAbbrv
        //            )
        //            .ToList();

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

        #region commend qm email
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

        //public async Task<List<Organizations>> GetOrganization(string companyCode)
        //{
        //    HttpClient c = _httpClient.CreateClient("IDAMAN.API");

        //    try
        //    {
        //        var r = await c.GetAsync($"/v1/organizations/company/{companyCode}?take=10000");

        //        if (r.IsSuccessStatusCode)
        //        {
        //            var data = await r.Content.ReadAsAsync<OrganizationDT>();
        //            return data.value;
        //        }

        //        throw new Exception();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<List<Organization>> GetOrganization(string companyCode)
        {
            HttpClient c = _httpClient.CreateClient("IDAMAN.API");

            try
            {

                var r = await c.GetAsync($"/v1/positions/company/{companyCode}/false/true?page=1&take=1000");
                var s = await c.GetAsync($"/v1/positions/company/{companyCode}/true/false?page=1&take=1000");

                List<ValueOrg> DataIsWorker = new List<ValueOrg>();
                List<ValueOrg> DataNotWorker = new List<ValueOrg>();

                if (r.IsSuccessStatusCode)
                {
                    var data = await r.Content.ReadAsAsync<RootValueOrg>();
                    DataNotWorker = data.value;
                }

                if (s.IsSuccessStatusCode)
                {
                    var data = await s.Content.ReadAsAsync<RootValueOrg>();
                    DataIsWorker = data.value;
                }


                //var dataFinal = DataIsWorker.Concat(DataNotWorker).Select(a => a.position.organization).ToHashSet();
                var dataFinal = DataIsWorker.Concat(DataNotWorker).Select(a => a.position.organization).GroupBy(a => a.id).Select(a => a.First()).ToList();

                return dataFinal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
