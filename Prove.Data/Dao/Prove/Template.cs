using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Prove
{
    public class Template : BaseDao
    {
        public virtual TemplateType Type { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int FileId { get; set; }
        public string CompanyCode { get; set; }
    }
}
