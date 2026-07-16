using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prove.Data.Dao.CorePTK;
using Prove.Data.Services.CorePTK.Interface;
using Prove.Data.Services.FileUpload.Interface;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.FileUpload
{
    public class FileUploadService : IFileUpload
    {
        private readonly IUploadFile _uploadFileService;
        public FileUploadService(IUploadFile uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }
        public async Task<UploadFile> UploadFileWithReturn(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string Flag = null, bool autoRename = false, string remark = null)
        {
            if (file != null)
            {
                string filePath = GeneralConstant.CreateUploadPathNew(path);
                string filePathView = PrideConstant.CreateUploadPathView(path);
                string fileName = file.FileName.ToLower();

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                UploadFile uploadFile = new UploadFile
                {
                    FilePath = filePathView,
                    FileName = fileName,
                    ContentType = file.ContentType,
                    Length = file.Length,
                    TrxId = trxId,
                    CreatedBy = createBy,
                    Flag = Flag,
                    Remark = remark
                };

                // Delete File First while Update Action
                if (isUpdate)
                {
                    UploadFile oldFile = _uploadFileService
                        .Find(
                        b =>
                        b.TrxId == trxId &&
                        b.Flag == Flag &&
                        b.Remark == remark &&
                        b.IsDeleted != GeneralConstant.YES
                        );
                    await MoveFile(oldFile, createBy);
                }

                _uploadFileService.Add(uploadFile);

                if (autoRename)
                {
                    uploadFile.FileName = $"{uploadFile.Id}-{uploadFile.FileName}";
                    _uploadFileService.Update(uploadFile);
                }

                using (var stream = new FileStream(Path.Combine(filePath, uploadFile.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    stream.Flush();
                }
                return uploadFile;
            }
            return null;
        }

        public async Task UploadFile(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string Flag = null, string remark = null, bool autoRename = false)
        {
            if (file != null)
            {
                string filePath = GeneralConstant.CreateUploadPathNew(path);
                string filePathView = PrideConstant.CreateUploadPathView(path);
                string fileName = file.FileName.ToLower();

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                UploadFile uploadFile = new UploadFile
                {
                    FilePath = filePathView,
                    FileName = fileName,
                    ContentType = file.ContentType,
                    Length = file.Length,
                    TrxId = trxId,
                    CreatedBy = createBy,
                    Flag = Flag,
                    Remark = remark
                };

                // Delete File First while Update Action
                if (isUpdate)
                {
                    UploadFile oldFile = _uploadFileService
                        .Find(
                        b =>
                        b.TrxId == trxId &&
                        b.Flag == Flag &&
                        b.Remark == remark &&
                        b.IsDeleted != GeneralConstant.YES
                        );
                    await MoveFile(oldFile, createBy);
                }

                _uploadFileService.Add(uploadFile);

                if (autoRename)
                {
                    uploadFile.FileName = $"{uploadFile.Id}-{uploadFile.FileName}";
                    fileName = uploadFile.FileName;
                    _uploadFileService.Update(uploadFile);
                }

                using (var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }

        public Task MoveFile(UploadFile oldFile, string updateBy)
        {
            if (oldFile != null)
            {
                string deletePath = GeneralConstant.ReplaceDeletedPath(oldFile.FilePath);

                if (!Directory.Exists(GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + deletePath))
                {
                    Directory.CreateDirectory(GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + deletePath);
                };

                //UploadFile uploadFileOld = _uploadFileService.Find(b => b.IsDeleted == PrideConstant.NO && b.Id == DecryptionId(q));

                string fileFrom = GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + Path.Combine(oldFile.FilePath, oldFile.FileName);
                string fileTarget = GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + GeneralConstant.CreateDeletedPath(Path.Combine(deletePath, oldFile.FileName));

                if (File.Exists(fileFrom) && System.IO.File.Exists(fileTarget))
                {
                    File.Delete(fileTarget);
                }

                if (File.Exists(fileFrom))
                    File.Move(fileFrom, fileTarget);

                // Update old file data
                //uploadFileOld.FilePath = PrideConstant.URL_DOWNLOAD_UPLOAD + "/upload/" + path + "DeleteFiles/" + trxId + "/";
                oldFile.FilePath = deletePath;
                oldFile.IsDeleted = PrideConstant.YES;
                oldFile.UpdatedBy = updateBy;
                oldFile.UpdatedAt = DateTime.Now;

                _uploadFileService.Update(oldFile);
            }
            return Task.CompletedTask;
        }

        public async Task<FileStreamResult> ReadFile(int id, bool isFileNameFromDb = false, string fileName = null)
        {
            try
            {
                UploadFile file = _uploadFileService.Find(b => b.Id == id && b.IsDeleted == PrideConstant.NO);

                MemoryStream stream = new MemoryStream();

                string filePath = file != null ? (GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + file.FilePath + file.FileName) : GeneralConstant.IMG_PLACEHOLDER;
                string contentType = file != null ? file.ContentType : @"image/png";

                bool isFileExists = System.IO.File.Exists(filePath);

                byte[] byteArray = isFileExists ? await File.ReadAllBytesAsync(filePath) : await File.ReadAllBytesAsync(GeneralConstant.IMG_PLACEHOLDER);

                stream.Write(byteArray, 0, byteArray.Length);
                stream.Position = 0;
                FileStreamResult result = new FileStreamResult(stream, isFileExists && file != null ? file.ContentType : @"image/png");
                if (isFileNameFromDb)
                {
                    result.FileDownloadName = file?.FileName ?? "404";
                }
                else if (!string.IsNullOrWhiteSpace(fileName))
                {
                    result.FileDownloadName = fileName ?? "404";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<FileStreamResult> ReadFile(UploadFile file, bool isFileNameFromDb = false, string fileName = null)
        {

            MemoryStream stream = new MemoryStream();

            string filePath = file != null ? (GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + file.FilePath + file.FileName) : GeneralConstant.IMG_PLACEHOLDER;
            string contentType = file != null ? file.ContentType : @"image/png";

            bool isFileExists = System.IO.File.Exists(filePath);

            byte[] byteArray = isFileExists ? await File.ReadAllBytesAsync(filePath) : await File.ReadAllBytesAsync(GeneralConstant.IMG_PLACEHOLDER);

            stream.Write(byteArray, 0, byteArray.Length);
            stream.Position = 0;
            FileStreamResult result = new FileStreamResult(stream, isFileExists && file != null ? file.ContentType : @"image/png");
            if (isFileNameFromDb)
            {
                result.FileDownloadName = file?.FileName ?? "404";
            }
            else if (!string.IsNullOrWhiteSpace(fileName))
            {
                result.FileDownloadName = fileName ?? "404";
            }
            return result;
        }

        public async Task<FileStreamResult> ReadFileWithDeletedFile(int id, bool isFileNameFromDb = false, string fileName = null)
        {
            try
            {
                UploadFile file = _uploadFileService.Find(b => b.Id == id);

                MemoryStream stream = new MemoryStream();

                string filePath = file != null ? (GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + file.FilePath + file.FileName) : GeneralConstant.IMG_PLACEHOLDER;
                string contentType = file != null ? file.ContentType : @"image/png";

                bool isFileExists = System.IO.File.Exists(filePath);

                byte[] byteArray = isFileExists ? await File.ReadAllBytesAsync(filePath) : await File.ReadAllBytesAsync(GeneralConstant.IMG_PLACEHOLDER);

                stream.Write(byteArray, 0, byteArray.Length);
                stream.Position = 0;
                FileStreamResult result = new FileStreamResult(stream, isFileExists && file != null ? file.ContentType : @"image/png");
                if (isFileNameFromDb)
                {
                    result.FileDownloadName = file?.FileName ?? "404";
                }
                else if (!string.IsNullOrWhiteSpace(fileName))
                {
                    result.FileDownloadName = fileName ?? "404";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<string> GetFilePath(int id)
        {
            UploadFile file = await _uploadFileService.GetByIdAsync(id);
            string result = GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + file.FilePath + file.FileName;
            if (!File.Exists(result))
                throw new FileNotFoundException();
            return result;
        }

        public async Task<string> GetFilePath(UploadFile file)
        {
            string result = GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + file.FilePath + file.FileName;
            if (!File.Exists(result))
                throw new FileNotFoundException();
            return result;
        }

        public async Task<BaseReadFileModel> ReadFileByte(int id)
        {
            UploadFile file = await _uploadFileService.FindAsync(b => b.Id == id && b.IsDeleted != GeneralConstant.YES);
            return await ReadFileByte(file);
        }

        public async Task<BaseReadFileModel> ReadFileByte(UploadFile file)
        {

            string filePath = file != null ? (GeneralConstant.URL_DOWNLOAD_UPLOAD_NEW + file.FilePath + file.FileName) : GeneralConstant.IMG_PLACEHOLDER;

            bool isFileExists = File.Exists(filePath);

            byte[] byteArray = isFileExists ? await File.ReadAllBytesAsync(filePath) : await File.ReadAllBytesAsync(GeneralConstant.IMG_PLACEHOLDER);

            return new BaseReadFileModel
            {
                ContentType = file?.ContentType ?? @"image/png",
                FileContents = byteArray
            };
        }
    }
}
