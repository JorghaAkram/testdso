using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prove.Data.Dao.CorePTK;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.FileUpload.Interface
{
    public interface IFileUpload
    {
        Task<UploadFile> UploadFileWithReturn(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string Flag = null, bool autoRename = false, string remark = null);
        Task<string> GetFilePath(int id);
        Task<string> GetFilePath(UploadFile file);
        Task UploadFile(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string Flag = null, string remark = null, bool autoRename = false);
        Task MoveFile(UploadFile oldFile, string updateBy);
        Task<FileStreamResult> ReadFile(int id, bool isFileNameFromDb = false, string fileName = null);
        Task<FileStreamResult> ReadFile(UploadFile file, bool isFileNameFromDb = false, string fileName = null);
        Task<FileStreamResult> ReadFileWithDeletedFile(int id, bool isFileNameFromDb = false, string fileName = null);
        Task<BaseReadFileModel> ReadFileByte(int id);
        Task<BaseReadFileModel> ReadFileByte(UploadFile file);
    }
}
