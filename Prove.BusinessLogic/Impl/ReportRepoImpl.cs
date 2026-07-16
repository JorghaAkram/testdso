using Microsoft.EntityFrameworkCore;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao.CorePTK;
using Prove.Data.Dao.Prove;
using Prove.Data.Data;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Impl
{
    public class ReportRepoImpl : IReportRepo
    {
        private readonly CorePTKContext _coreContext;
        private readonly ProveExtContext _proveContext;
        private readonly IHttpClientFactory _httpClient;

        public ReportRepoImpl(ProveExtContext proveContext, CorePTKContext coreContext, IHttpClientFactory httpClient)
        {
            _proveContext = proveContext;
            _coreContext = coreContext;
            _httpClient = httpClient;
        }

        public async Task<BaseDTResponseModel<ReportSKSPJtable>> getSKSPDatatable(ReportSKSPJtableParam param)
        {
            try
            {
                IQueryable<ReportSKSPJtable> itemData = null;
                List<ReportSKSPJtable> filteredData = new List<ReportSKSPJtable>();
                IQueryable<RegulationSKSP> allItemData = null;

                allItemData = _proveContext.RegulationSKSP.Include(a => a.ProductOfLawSKSP)
                                                                .Include(b => b.SaveCode)
                                                                .Where(c => c.IsActive == GeneralConstant.YES
                                                                         && c.IsDeleted == GeneralConstant.NO
                                                                         && c.StatusDocId == (param.status == "1" ? StatusDoc.Berlaku : StatusDoc.TidakBerlaku)
                                                                         && c.TypeId == (param.documentType == "2" ? TypeSurat.Keputusan : TypeSurat.Perintah)
                                                                         && c.Status == GeneralConstant.PROVE_UPLOADED
                                                                         && c.CompanyCode == param.companyCode) //dev
                                                                .OrderByDescending(d => d.CreatedAt);

                if (param.PageSize < 1)
                    return new BaseDTResponseModel<ReportSKSPJtable>
                    {
                        Data = new List<ReportSKSPJtable>(),
                        Draw = param.Draw,
                        RecordsFiltered = 0,
                        RecordsTotal = allItemData.Count()
                    };

                itemData = (from a in allItemData
                            where (string.IsNullOrWhiteSpace(param.nomor) || a.SKSPNumber.ToLower().Contains(param.nomor.ToLower()))
                               && (string.IsNullOrWhiteSpace(param.perihal) || (a.Title != null ? a.Title.ToLower().Contains(param.perihal.ToLower()) : a.Title == param.perihal))
                               && (param.produkHukum == 0 || a.ProductOfLawSKSP.Id == param.produkHukum)
                               && (param.tmtBerlaku == null || a.TmtApplies == param.tmtBerlaku)
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
                                //nomor = a.Number,
                                nomorSKSP = a.SKSPNumber,
                                perihal = a.Title,
                                //positionNumber = a.PositionNumber,
                                produkHukumName = param.documentType == "2" ? "Kpts " + a.ProductOfLawSKSP.Description : "Prin " + a.ProductOfLawSKSP.Description,
                                //produkHukumId = a.ProductOfLawSKSP.Id,
                                status = a.Status,
                                //statusDoc = a.StatusDocId.ToString(),
                                //statusDocId = (int)a.StatusDocId,
                                //type = a.TypeId.ToString(),
                                //tahun = a.Year,
                                CreatedDate = a.CreatedAt,
                                expiredDate = a.ExpiredDate
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

                BaseDTResponseModel<ReportSKSPJtable> result = new BaseDTResponseModel<ReportSKSPJtable>
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

        public async Task<BaseDTResponseModel<ReportSTKJtable>> getSTKDatatable(ReportSTKJtableParam param)
        {
            try
            {
                IQueryable<ReportSTKJtable> itemData = null;
                List<ReportSTKJtable> filteredData = new List<ReportSTKJtable>();
                List<EmployeePosition> positionData = new List<EmployeePosition>();
                IQueryable<RegulationSTK> allItemData = null;

                allItemData = _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .ThenInclude(a => a.Type)
                                                               .Include(a => a.Type)
                                                               .Include(a => a.SaveCode)
                                                               .Include(a => a.Probis)
                                                               .Where(c => c.IsActive == GeneralConstant.YES
                                                                        && c.IsDeleted == GeneralConstant.NO
                                                                        && c.StatusDocId == (param.status == "1" ? StatusDoc.Berlaku : StatusDoc.TidakBerlaku)
                                                                        && c.Status == GeneralConstant.PROVE_UPLOADED
                                                                        && c.CompanyCode == param.companyCode)//dev
                                                               .OrderByDescending(d => d.CreatedAt);

                if (param.PageSize < 1)
                    return new BaseDTResponseModel<ReportSTKJtable>
                    {
                        Data = new List<ReportSTKJtable>(),
                        Draw = param.Draw,
                        RecordsFiltered = 0,
                        RecordsTotal = allItemData.Count()
                    };

                itemData = (from a in allItemData
                            where (string.IsNullOrWhiteSpace(param.nomor) || a.STKNumber.ToLower().Contains(param.nomor.ToLower()))
                               && (string.IsNullOrWhiteSpace(param.perihal) || (a.Title != null ? a.Title.ToLower().Contains(param.perihal.ToLower()) : a.Title == param.perihal))
                               && (param.jenis == 0 || a.ProductOfLawSTK.Id == param.jenis)
                               && (param.revisiKe == 0 || a.RevisedFlag == param.revisiKe)
                               && (string.IsNullOrWhiteSpace(param.fungsi) || a.PositionId == param.fungsi)
                               && (param.tmtBerlaku == null || a.TmtApplies == param.tmtBerlaku)
                            select new ReportSTKJtable
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
                                perihal = a.Title,
                                //positionNumber = a.PositionId,
                                positionName = a.PositionId,
                                produkHukumName = a.ProductOfLawSTK.Description,
                                produkHukumId = a.ProductOfLawSTK.Id,
                                status = a.Status,
                                //statusDoc = a.StatusDocId.ToString(),
                                //statusDocId = (int)a.StatusDocId,
                                //jenis = a.Type.Code,
                                //positionCode = a.PositionCode,
                                //probis = a.Probis.Description,
                                //probisId = a.Probis.Id,
                                //probisNumber = a.ProbisNumber,
                                revisedFlag = a.RevisedFlag,
                                //year = a.Year,
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

                var posData = await GetOrganization(param.companyCode);

                filteredData.ForEach(b =>
                {
                    b.positionName = posData.FirstOrDefault(c => c.id == b.positionName).name;
                });

                BaseDTResponseModel<ReportSTKJtable> result = new BaseDTResponseModel<ReportSTKJtable>
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

        public async Task<BaseDTResponseModel<ReportSTKJtable>> getSTKList(ReportSTKJtableParam param)
        {
            try
            {
                List<ReportSTKJtable> itemData = new List<ReportSTKJtable>();
                List<ReportSTKJtable> filteredData = new List<ReportSTKJtable>();
                List<EmployeePosition> positionData = new List<EmployeePosition>();
                List<RegulationSTK> allItemData = new List<RegulationSTK>();

                allItemData = await _proveContext.RegulationSTK.Include(a => a.ProductOfLawSTK)
                                                               .Include(a => a.Type)
                                                               .Include(a => a.SaveCode)
                                                               .Include(a => a.Probis)
                                                               .Where(c => c.IsActive == GeneralConstant.YES
                                                                        && c.IsDeleted == GeneralConstant.NO
                                                                        && c.StatusDocId == (param.status == "1" ? StatusDoc.Berlaku : StatusDoc.TidakBerlaku)
                                                                        && c.CompanyCode == param.companyCode)
                                                               .OrderByDescending(d => d.CreatedAt)
                                                               .ToListAsync();

                positionData = await _coreContext.EmployeePositions.Include(a => a.Organization)
                                                                   .Include(b => b.EmployeePositionAbbrv)
                                                                   .Include(c => c.PAreaSub)
                                                                   .Where(d => d.IsActive == GeneralConstant.YES
                                                                            && d.IsDeleted == GeneralConstant.NO)
                                                                   .ToListAsync();

                itemData = (from a in allItemData
                            join c in positionData on Convert.ToInt32(a.PositionId) equals c.Id into f
                            from cf in f.DefaultIfEmpty()
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
                                positionName = cf == null ? string.Empty : cf.PositionName,
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


                BaseDTResponseModel<ReportSTKJtable> result = new BaseDTResponseModel<ReportSTKJtable>
                {
                    Data = itemData,
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

        public async Task<BaseDTResponseModel<ReportSKSPJtable>> getSKSPList(ReportSKSPJtableParam param)
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
                                                                         && c.StatusDocId == (param.status == "1" ? StatusDoc.Berlaku : StatusDoc.TidakBerlaku)
                                                                         && c.TypeId == (param.documentType == "2" ? TypeSurat.Keputusan : TypeSurat.Perintah)
                                                                         && c.CompanyCode == param.companyCode)
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
                                produkHukumName = a.ProductOfLawSKSP.Description,
                                produkHukumId = a.ProductOfLawSKSP.Id,
                                status = a.Status,
                                statusDoc = a.StatusDocId.ToString(),
                                statusDocId = (int)a.StatusDocId,
                                type = a.TypeId.ToString(),
                                tahun = a.Year,
                                CreatedDate = a.CreatedAt,
                                CreatedBy = a.CreatedBy,
                                expiredDate = a.ExpiredDate
                            }).ToList();

                BaseDTResponseModel<ReportSKSPJtable> result = new BaseDTResponseModel<ReportSKSPJtable>
                {
                    Data = itemData,
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

        public async Task<List<BusinessLogic.Models.Organization>> GetOrganization(string companyCode)
        {
            HttpClient c = _httpClient.CreateClient("IDAMAN.API");

            try
            {

                var r = await c.GetAsync($"/v1/positions/company/{companyCode}/false/true?page=1&take=10000");
                var s = await c.GetAsync($"/v1/positions/company/{companyCode}/true/false?page=1&take=10000");

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
