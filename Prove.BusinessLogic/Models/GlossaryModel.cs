using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.BusinessLogic.Models
{
    public class GlossaryJtable : BaseJtable
    {
        public string istilah { get; set; }
        public string pengertian { get; set; }
        public string referensi { get; set; }
    }

    public class GlossaryJtableParam : BaseDTParam
    {
        public string istilah { get; set; }
        public string pengertian { get; set; }
        public string referensi { get; set; }
        public string companyCode { get; set; }
    }

    public class GlossaryPost
    {
        public int id { get; set; }
        public string istilah { get; set; }
        public string desc { get; set; }
        public string referensi { get; set; }
        public string username { get; set; }
        public string companyCode { get; set; }
    }
}
