using Microsoft.AspNetCore.Http;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.BusinessLogic.Models
{
    public class SKSPJtable : BaseJtable
    {
        public string produkHukumName { get; set; }
        public int produkHukumId { get; set; }
        public int regulationCategoryId { get; set; }
        public string regulationCategoryName { get; set; }
        public string nomorSKSP { get; set; }
        public string perihal { get; set; }
        public DateTime tmtBerlaku { get; set; }
        public DateTime? expiredDate { get; set; }
        public DateTime? jointReviewDate { get; set; }
        public string code { get; set; }
        public string catatan { get; set; }
        public string nomor { get; set; }
        public string positionNumber { get; set; }
        public string kbo { get; set; }
        public string kodeSimpan { get; set; }
        public int kodeSimpanId { get; set; }
        public string statusDoc { get; set; }
        public int statusDocId { get; set; }
        public string status { get; set; }
        public int fileId { get; set; }
        public int fileSupportId { get; set; }
        public string fileName { get; set; }
        public string fileSupportName { get; set; }
        public string type { get; set; }
        public string tahun { get; set; }
    }

    public class SKSPJtableParam : BaseDTParam
    {
        public int produkHukum { get; set; }
        public string status { get; set; }
        public string nomor { get; set; }
        public string perihal { get; set; }
        public DateTime? tmtBerlaku { get; set; }
        public DateTime? expiredDate { get; set; }
        public string conceptorDirId { get; set; }
        public string conceptorId { get; set; }
        public string conceptorDivId { get; set; }
        public string isAdmin { get; set; }
        public string companyCode { get; set; }
    }

    public class SKSPPost
    {
        public string id { get; set; }
        public string categoryId { get; set; }
        public string produkHukumId { get; set; }
        public string perihal { get; set; }
        public string nomor { get; set; }
        public string kbo { get; set; }
        public string positionNumber { get; set; }
        public string tahun { get; set; }
        public string kodeSimpan { get; set; }
        public string tmtBerlaku { get; set; }
        public string jointReviewDate { get; set; }
        public string expiredDate { get; set; }
        public string statusDoc { get; set; }
        public IFormFile file { get; set; }
        public IFormFile fileSupport { get; set; }
        public string username { get; set; }
        public string isAdmin { get; set; }
        public string notes { get; set; }
        public string conceptorId { get; set; }
        public string conceptorDivId { get; set; }
        public string conceptorDirId { get; set; }
        public string companyCode { get; set; }
    }
}
