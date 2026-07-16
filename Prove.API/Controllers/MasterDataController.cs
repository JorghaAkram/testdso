using Microsoft.AspNetCore.Mvc;
using Prove.API.Attributes;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao.CorePTK;
using Prove.Data.Dao.Prove;
using Prove.Data.Dao.Usman;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class MasterDataController : Controller
    {
        private readonly IMasterDataRepo _proveMasterData;
        private readonly IFileUploadRepo _proveFileUpload;

        public MasterDataController(IMasterDataRepo proveMasterData, IFileUploadRepo proveFileUpload)
        {
            _proveMasterData = proveMasterData;
            _proveFileUpload = proveFileUpload;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving product of law SKpts and SPrin)")]
        [Route("GetProductOfLawSKSP/{DocType}/{CompanyCode}")]
        public async Task<List<ProductOfLawSKSP>> GetProductOfLawSKSP(string DocType, string CompanyCode)
        {
            var exec = await _proveMasterData.getProductOfLawSKSP(DocType, CompanyCode);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving data file upload)")]
        [Route("GetDataFileUpload/{id}")]
        public async Task<FileUpload> GetDataFileUpload(int id)
        {
            var exec = await _proveMasterData.getDataFileUpload(id);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving data file upload)")]
        [Route("ReadFile/{id}")]
        public async Task<IActionResult> ReadFile(int id)
        {
            try
            {
                ReadFileModel exec = await _proveFileUpload.GetFileData(id);

                return File(exec.FileContents, exec.ContentType, exec.FileName);

                //var exec = await _proveFileUpload.ReadFile(id);

                //return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving data file upload Desc)")]
        [Route("ReadFileDesc/{id}")]
        public async Task<ReadFileModel> ReadFileDesc(int id)
        {
            try
            {
                ReadFileModel exec = await _proveFileUpload.GetFileData(id);

                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving data user role)")]
        [Route("GetUserRole/{posId}/{companyCode}")]
        public async Task<Role> GetUserRole(string posID, string companyCode)
        {
            try
            {
                Role exec = await _proveMasterData.getRolebyPosId(posID, companyCode);

                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving data file upload)")]
        [Route("ReadFileWithDeletedFile/{id}/{isFileNameFromDb}")]
        public async Task<FileStreamResult> ReadFileWithDeletedFile(int id, bool isFileNameFromDb)
        {
            var exec = await _proveFileUpload.ReadFileWithDeletedFile(id, isFileNameFromDb);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving product of law STK")]
        [Route("GetProductOfLawSTK/{companyCode}")]
        public async Task<List<ProductOfLawSTK>> GetProductOfLawSTK(string companyCode)
        {
            var exec = await _proveMasterData.getProductOfLawSTK(companyCode);

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving save code data)")]
        [Route("GetSaveCode")]
        public async Task<List<SaveCode>> GetSaveCode()
        {
            var exec = await _proveMasterData.getSaveCode();

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving type of STK)")]
        [Route("GetSTKType")]
        public async Task<List<STKType>> GetSTKType()
        {
            var exec = await _proveMasterData.getSTKType();

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving KBO data)")]
        [Route("GetKBO")]
        public async Task<List<KBO>> GetKBO()
        {
            var exec = await _proveMasterData.getKBO();

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving division data)")]
        [Route("GetFungsi")]
        public async Task<List<EmployeePosition>> GetFungsi()
        {
            var exec = await _proveMasterData.getFungsi();

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving probis data)")]
        [Route("GetProbis")]
        public async Task<List<Probis>> GetProbis()
        {
            var exec = await _proveMasterData.getProbis();

            return exec;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "(This method is for retrieving type of template)")]
        [Route("GetTemplateType")]
        public async Task<List<TemplateType>> GetTemplateType()
        {
            var exec = await _proveMasterData.getTemplateType();

            return exec;
        }
    }
}
