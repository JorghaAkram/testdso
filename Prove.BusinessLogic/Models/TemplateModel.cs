using Microsoft.AspNetCore.Http;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.BusinessLogic.Models
{
    public class TemplateJtable : BaseJtable
    {
        public string namaFile { get; set; }
        public string tipeDokumen { get; set; }
        public int tipeDokumenId { get; set; }
        public string pengunggah { get; set; }
        public DateTime tglUnggah { get; set; }
        public int fileID { get; set; }
        public string status { get; set; }
    }

    public class TemplateJtableParam : BaseDTParam
    {
        public string namaFile { get; set; }
        public int tipeDokumen { get; set; }
        public string pengunggah { get; set; }
        public int status { get; set; }
        public DateTime? tglUnggah { get; set; }
        public string companyCode { get; set; }
    }

    public class TemplatePost
    {
        public string id { get; set; }
        public string typeDoc { get; set; }
        public string docName { get; set; }
        public IFormFile file { get; set; }
        public string username { get; set; }
        public string companyCode { get; set; }
    }
}
