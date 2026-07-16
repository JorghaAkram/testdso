using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using Prove.BusinessLogic.Interface;
using Prove.BusinessLogic.Models;
using Prove.Data.Dao.CorePTK;
using Prove.Data.Dao.Prove;
using Prove.Data.Data;
using Prove.Data.Services.CorePTK.Interface;
using Prove.Utilities.Base;
using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Impl
{
    public class FileUploadRepoImpl : IFileUploadRepo
    {
        private readonly MinioClient _mc;
        private readonly ProveExtContext _proveContext;

        public FileUploadRepoImpl(MinioClient mc, ProveExtContext proveContext)
        {
            _mc = mc;
            _proveContext = proveContext;
        }

        private byte[] CompressImage(IFormFile img)
        {

            byte[] result = null;

            using (MagickImage image = new MagickImage(img.OpenReadStream()))
            {
                //image.Format = image.Format; // Get or Set the format of the image.
                //image.Resize(40, 40); // fit the image into the requested width and height. 
                image.Quality = 50; // This is the Compression level.
                result = image.ToByteArray();
            }
            return result;
            //using (var stream = new FileStream(Path.Combine($"D:/TesUpload/", images.FileName), FileMode.Create))
            //{
            //    await stream.WriteAsync(result, 0, result.Length);
            //}
        }

        private byte[] CompressImage(byte[] img)
        {

            byte[] result = null;

            using (MagickImage image = new MagickImage(img))
            {
                //image.Format = image.Format; // Get or Set the format of the image.
                //image.Resize(40, 40); // fit the image into the requested width and height. 
                image.Quality = 50; // This is the Compression level.
                result = image.ToByteArray();
            }
            return result;
            //using (var stream = new FileStream(Path.Combine($"D:/TesUpload/", images.FileName), FileMode.Create))
            //{
            //    await stream.WriteAsync(result, 0, result.Length);
            //}
        }

        private string CreatePath(string path) => Path.Combine(path, DateTime.UtcNow.ToString("yyyyMMdd/"));

        public async Task DeleteFile(FileUpload oldFile, string updateBy)
        {
            if (oldFile != null)
            {
                string fileFrom = Path.Combine(oldFile.FilePath, oldFile.FileName);
                bool isFileFromExist = false;
                try
                {
                    ObjectStat stat = await _mc.StatObjectAsync("proveuploaded", fileFrom);
                    isFileFromExist = true;
                }
                catch (ObjectNotFoundException)
                {

                }

                if (isFileFromExist)
                {
                    if (!await _mc.BucketExistsAsync("provedeleted"))
                        await _mc.MakeBucketAsync("provedeleted");
                    try
                    {
                        await _mc.CopyObjectAsync("proveuploaded", fileFrom, "provedeleted", destObjectName: fileFrom, copyConditions: new CopyConditions());
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    await _mc.RemoveObjectAsync("proveuploaded", fileFrom);
                }

                oldFile.IsDeleted = GeneralConstant.YES;
                oldFile.UpdatedBy = updateBy;
                oldFile.BucketName = "provedeleted";
                oldFile.UpdatedAt = DateTime.Now;

                _proveContext.Set<FileUpload>().Update(oldFile);
                await _proveContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetFilePath(int id)
        {
            FileUpload file = await _proveContext.Set<FileUpload>().SingleOrDefaultAsync(b => b.Id == id);
            string result = Path.Combine(file.FilePath + file.FileName);
            try
            {
                ObjectStat stat = await _mc.StatObjectAsync("proveuploaded", result);
            }
            catch (ObjectNotFoundException)
            {
                throw new FileNotFoundException();
            }
            return result;
        }

        public async Task<string> GetFilePath(FileUpload file)
        {
            string result = Path.Combine(file.FilePath + file.FileName);
            try
            {
                ObjectStat stat = await _mc.StatObjectAsync("proveuploaded", result);
            }
            catch (ObjectNotFoundException)
            {
                throw new FileNotFoundException();
            }
            return result;
        }

        public async Task<FileStreamResult> ReadFile(int id, bool isFileNameFromDb = false, string fileName = null)
        {
            try
            {
                FileUpload file = await _proveContext.Set<FileUpload>().SingleOrDefaultAsync(b => b.Id == id && b.IsDeleted == GeneralConstant.NO);
                MemoryStream stream = new MemoryStream();
                string filePath = file != null ? (Path.Combine(file.FilePath + file.FileName)) : GeneralConstant.IMG_PLACEHOLDER;

                bool isFileExists = false;
                if (file != null)
                {
                    try
                    {
                        ObjectStat stat = await _mc.StatObjectAsync("proveuploaded", filePath);
                        isFileExists = true;
                    }
                    catch (ObjectNotFoundException)
                    {

                    }
                }
                if (isFileExists)
                    using (MemoryStream ms = new MemoryStream())
                        await _mc.GetObjectAsync("proveuploaded", filePath, b => b.CopyTo(stream));

                else
                {
                    byte[] byteArray = await File.ReadAllBytesAsync(GeneralConstant.IMG_PLACEHOLDER);
                    stream.Write(byteArray, 0, byteArray.Length);
                }
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

        public async Task<FileStreamResult> ReadFile(FileUpload file, bool isFileNameFromDb = false, string fileName = null)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                string filePath = file != null ? (Path.Combine(file.FilePath + file.FileName)) : GeneralConstant.IMG_PLACEHOLDER;

                bool isFileExists = false;
                if (file != null)
                {
                    try
                    {
                        ObjectStat stat = await _mc.StatObjectAsync("proveuploaded", filePath);
                        isFileExists = true;
                    }
                    catch (ObjectNotFoundException)
                    {

                    }
                }
                if (isFileExists)
                    using (MemoryStream ms = new MemoryStream())
                        await _mc.GetObjectAsync("proveuploaded", filePath, b => b.CopyTo(stream));

                else
                {
                    byte[] byteArray = await File.ReadAllBytesAsync(GeneralConstant.IMG_PLACEHOLDER);
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Position = 0;
                }
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

        public async Task<FileStreamResult> ReadFileWithDeletedFile(int id, bool isFileNameFromDb = false, string fileName = null)
        {
            try
            {
                FileUpload file = await _proveContext.Set<FileUpload>().SingleOrDefaultAsync(b => b.Id == id);
                MemoryStream stream = new MemoryStream();
                string filePath = file != null ? (Path.Combine(file.FilePath + file.FileName)) : GeneralConstant.IMG_PLACEHOLDER;

                bool isFileExists = false;
                if (file != null)
                {
                    try
                    {
                        ObjectStat stat = await _mc.StatObjectAsync("proveuploaded", filePath);
                        isFileExists = true;
                    }
                    catch (ObjectNotFoundException)
                    {

                    }
                }
                if (isFileExists)
                    using (MemoryStream ms = new MemoryStream())
                        await _mc.GetObjectAsync("proveuploaded", filePath, b => b.CopyTo(stream));

                else
                {
                    byte[] byteArray = await File.ReadAllBytesAsync(GeneralConstant.IMG_PLACEHOLDER);
                    stream.Write(byteArray, 0, byteArray.Length);
                }
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

        public async Task<ReadFileModel> ReadFileByte(int id)
        {
            FileUpload file = await _proveContext.Set<FileUpload>().SingleOrDefaultAsync(b => b.Id == id && b.IsDeleted != GeneralConstant.YES);
            return await ReadFileByte(file);
        }

        public async Task<ReadFileModel> ReadFileByte(FileUpload file)
        {
            string filePath = file != null ? (Path.Combine(file.FilePath + file.FileName)) : GeneralConstant.IMG_PLACEHOLDER;

            bool isFileExists = false;
            if (file != null)
            {
                try
                {
                    ObjectStat stat = await _mc.StatObjectAsync("proveuploaded", filePath);
                    isFileExists = true;
                }
                catch (ObjectNotFoundException)
                {

                }
            }

            byte[] byteArray = null;
            if (isFileExists)
                using (MemoryStream ms = new MemoryStream())
                {
                    await _mc.GetObjectAsync("proveuploaded", filePath, b => b.CopyTo(ms));
                    byteArray = ms.ToArray();
                }
            else
                byteArray = await File.ReadAllBytesAsync(GeneralConstant.IMG_PLACEHOLDER);

            return new ReadFileModel
            {
                ContentType = isFileExists ? file.ContentType : @"image/png",
                FileContents = byteArray,
                FileName = isFileExists ? file.FileName : "404.jpg"
            };
        }

        public async Task UploadFile(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string Flag = null, string remark = null, bool autoRename = false, bool isCompress = false)
        {
            if (file != null)
            {
                path = path.EndsWith("/") ? path : path + "/";
                string filePathView = CreatePath(path);
                string fileName = file.FileName.ToLower();

                byte[] compresed = null;
                if (isCompress)
                    compresed = CompressImage(file);

                FileUpload uploadFile = new FileUpload
                {
                    FilePath = filePathView,
                    FileName = fileName,
                    ContentType = file.ContentType,
                    Length = isCompress ? compresed.Length : file.Length,
                    TrxId = trxId,
                    CreatedBy = createBy,
                    Flag = Flag,
                    Remark = remark
                };

                // Delete File First while Update Action
                if (isUpdate)
                {
                    FileUpload oldFile = await _proveContext.Set<FileUpload>().SingleOrDefaultAsync(
                        b =>
                        b.TrxId == trxId &&
                        b.Flag == Flag &&
                        b.Remark == remark &&
                        b.IsDeleted != GeneralConstant.YES);
                    await DeleteFile(oldFile, createBy);
                }

                await _proveContext.Set<FileUpload>().AddAsync(uploadFile);
                await _proveContext.SaveChangesAsync();

                if (autoRename)
                {
                    uploadFile.FileName = $"{uploadFile.Id}-{uploadFile.FileName}";
                    fileName = uploadFile.FileName;
                    _proveContext.Set<FileUpload>().Update(uploadFile);
                }

                if (!await _mc.BucketExistsAsync("proveuploaded"))
                    await _mc.MakeBucketAsync("proveuploaded");


                using MemoryStream ms = new MemoryStream();
                if (isCompress)
                    await ms.WriteAsync(compresed, 0, compresed.Length);
                else
                    await file.CopyToAsync(ms);

                ms.Position = 0;
                await _mc.PutObjectAsync(
                    bucketName: uploadFile.BucketName,
                    objectName: Path.Combine(filePathView, uploadFile.FileName),
                    data: ms,
                    size: ms.Length,
                    contentType: file.ContentType);
                await _proveContext.SaveChangesAsync();
            }
        }

        public async Task<FileUpload> UploadFileWithReturn(string path, string createBy, int trxId, IFormFile file, bool isUpdate = false, string Flag = null, bool autoRename = false, string remark = null, bool isCompress = false)
        {
            if (file != null)
            {
                path = path.EndsWith("/") ? path : path + "/";
                string filePathView = CreatePath(path);
                string fileName = file.FileName.ToLower();

                byte[] compresed = null;
                if (isCompress)
                    compresed = CompressImage(file);

                FileUpload uploadFile = new FileUpload
                {
                    BucketName = "proveuploaded",
                    FilePath = filePathView,
                    FileName = fileName,
                    ContentType = file.ContentType,
                    Length = isCompress ? compresed.Length : file.Length,
                    TrxId = trxId,
                    CreatedBy = createBy,
                    Flag = Flag,
                    Remark = remark
                };
                // Delete File First while Update Action
                if (isUpdate)
                {
                    FileUpload oldFile = await _proveContext.Set<FileUpload>().SingleOrDefaultAsync(
                        b =>
                        b.TrxId == trxId &&
                        b.Flag == Flag &&
                        b.Remark == remark &&
                        b.IsDeleted != GeneralConstant.YES);
                    await DeleteFile(oldFile, createBy);
                }

                await _proveContext.Set<FileUpload>().AddAsync(uploadFile);
                await _proveContext.SaveChangesAsync();

                if (autoRename)
                {
                    uploadFile.FileName = $"{uploadFile.Id}-{uploadFile.FileName}";
                    fileName = uploadFile.FileName;
                    _proveContext.Set<FileUpload>().Update(uploadFile);
                }

                if (!await _mc.BucketExistsAsync("proveuploaded"))
                    await _mc.MakeBucketAsync("proveuploaded");

                using (var ms = new MemoryStream())
                {
                    if (isCompress)
                        await ms.WriteAsync(compresed, 0, compresed.Length);
                    else
                        await file.CopyToAsync(ms);

                    ms.Position = 0;
                    await _mc.PutObjectAsync(bucketName: "proveuploaded", objectName: Path.Combine(filePathView, uploadFile.FileName), data: ms, size: ms.Length, contentType: file.ContentType);
                }
                await _proveContext.SaveChangesAsync();
                return uploadFile;
            }
            return null;
        }

        public async Task<FileUpload> UploadFileWithReturn(string path, string createBy, int trxId, ReadFileModel file, bool isUpdate = false, string Flag = null, bool autoRename = false, string remark = null, bool isCompress = false)
        {
            if (file != null)
            {
                path = path.EndsWith("/") ? path : path + "/";
                string filePathView = CreatePath(path);
                string fileName = file.FileName.ToLower();
                FileUpload uploadFile = new FileUpload
                {
                    BucketName = "proveuploaded",
                    FilePath = filePathView,
                    FileName = fileName,
                    ContentType = file.ContentType,
                    Length = file.FileContents.Length,
                    TrxId = trxId,
                    CreatedBy = createBy,
                    Flag = Flag,
                    Remark = remark
                };
                // Delete File First while Update Action
                if (isUpdate)
                {
                    FileUpload oldFile = await _proveContext.Set<FileUpload>().SingleOrDefaultAsync(
                        b =>
                        b.TrxId == trxId &&
                        b.Flag == Flag &&
                        b.Remark == remark &&
                        b.IsDeleted != GeneralConstant.YES);
                    await DeleteFile(oldFile, createBy);
                }

                await _proveContext.Set<FileUpload>().AddAsync(uploadFile);
                await _proveContext.SaveChangesAsync();

                if (autoRename)
                {
                    uploadFile.FileName = $"{uploadFile.Id}-{uploadFile.FileName}";
                    fileName = uploadFile.FileName;
                    _proveContext.Set<FileUpload>().Update(uploadFile);
                }


                if (!await _mc.BucketExistsAsync("proveuploaded"))
                    await _mc.MakeBucketAsync("proveuploaded");


                using (var ms = new MemoryStream())
                {
                    if (isCompress)
                    {
                        byte[] compresed = CompressImage(file.FileContents);
                        await ms.WriteAsync(compresed, 0, compresed.Length);
                    }
                    else
                        await ms.WriteAsync(file.FileContents, 0, file.FileContents.Length);

                    ms.Position = 0;
                    await _mc.PutObjectAsync(bucketName: "proveuploaded", objectName: Path.Combine(filePathView, uploadFile.FileName), data: ms, size: ms.Length, contentType: file.ContentType);
                }
                await _proveContext.SaveChangesAsync();
                return uploadFile;
            }
            return null;
        }

        public async Task<ReadFileModel> GetFileData(int id)
        {
            var res = await _proveContext.FileUpload.FirstOrDefaultAsync(a => a.Id == id);

            return await ReadFileByte(res);
        }
    }
}
