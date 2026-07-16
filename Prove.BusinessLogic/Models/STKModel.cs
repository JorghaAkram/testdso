using Microsoft.AspNetCore.Http;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.BusinessLogic.Models
{
    public class STKJtable : BaseJtable
    {
        public string produkHukumName { get; set; }
        public string produkHukumType { get; set; }
        public int produkHukumId { get; set; }
        public int regulationCategoryId { get; set; }
        public string regulationCategoryName { get; set; }
        public string jenis { get; set; }
        public string nomor { get; set; }
        public string nomorSerial { get; set; }
        public string nomorSTK { get; set; }
        public string refNumber { get; set; }
        public string positionNumber { get; set; }
        public string positionName { get; set; }
        public string kbo { get; set; }
        public string year { get; set; }
        public string kodeSimpan { get; set; }
        public int kodeSimpanId { get; set; }
        public int revisedFlag { get; set; }
        public string statusDoc { get; set; }
        public int statusDocId { get; set; }
        public string perihal { get; set; }
        public string positionCode { get; set; }
        public DateTime tmtBerlaku { get; set; }
        public string probis { get; set; }
        public int probisId { get; set; }
        public string probisNumber { get; set; }
        public string status { get; set; }
        public int fileId { get; set; }
        public int fileSupportId { get; set; }
        public string fileName { get; set; }
        public string fileSupportName { get; set; }
    }

    public class STKJtableParam : BaseDTParam
    {
        public int jenis { get; set; }
        public int rev { get; set; }
        public string status { get; set; }
        public string fungsi { get; set; }
        public string nomor { get; set; }
        public string perihal { get; set; }
        public DateTime? tmtBerlaku { get; set; }
        public string catatan { get; set; }
        public string conceptorDirId { get; set; }
        public string conceptorDivId { get; set; }
        public string conceptorId { get; set; }
        public string isAdmin { get; set; }
        public string companyCode { get; set; }
    }

    public class STKPost
    {
        public string id { get; set; }
        public string categoryId { get; set; }
        public string produkHukumId { get; set; }
        public string perihal { get; set; }
        public string nomor { get; set; }
        public string nomorSerial { get; set; }
        public string kbo { get; set; }
        public string positionNumber { get; set; }
        public string tahun { get; set; }
        public string kodeSimpan { get; set; }
        public string nomorRef { get; set; }
        public string revisiKe { get; set; }
        public string fungsi { get; set; }
        public string probisId { get; set; }
        public string statusId { get; set; }
        public string tmtBerlaku { get; set; }
        public string jointReviewDate { get; set; }
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
