using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao.Prove;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Interface
{
    public interface IFileUploadRepo
    {
        Task<FileUpload> UploadFileWithReturn(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string Flag = null, bool autoRename = false, string remark = null, bool isCompress = false);
        Task<FileUpload> UploadFileWithReturn(string path, string createBy, int trxId, ReadFileModel file, bool isUpdate = false, string Flag = null, bool autoRename = false, string remark = null, bool isCompress = false);
        Task<string> GetFilePath(int id);
        Task<string> GetFilePath(FileUpload file);
        Task UploadFile(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string Flag = null, string remark = null, bool autoRename = false, bool isCompress = false);
        Task DeleteFile(FileUpload oldFile, string updateBy);
        Task<FileStreamResult> ReadFile(int id, bool isFileNameFromDb = false, string fileName = null);
        Task<FileStreamResult> ReadFileWithDeletedFile(int id, bool isFileNameFromDb = false, string fileName = null);
        Task<FileStreamResult> ReadFile(FileUpload file, bool isFileNameFromDb = false, string fileName = null);
        Task<ReadFileModel> ReadFileByte(int id);
        Task<ReadFileModel> ReadFileByte(FileUpload file);
        Task<ReadFileModel> GetFileData(int id);
    }
}
