using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.Utilities.Base
{
    public abstract class BaseAppSettings
    {
        public IDictionary<string, string> Default { get; set; }
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public IDictionary<string, string> DataBase { get; set; }
        public bool IsProduction { get; set; }
    }
}
