using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.Utilities.Base
{
    public class BaseDTParam
    {
        public int Skip { get; set; }
        public int PageSize { get; set; }
        public int Draw { get; set; }
        public string ColumnIndex { get; set; }
        public string SortDirection { get; set; }
    }
}
