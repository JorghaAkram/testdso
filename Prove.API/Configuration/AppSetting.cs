using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Configuration
{
    [UsedImplicitly]
    public class AppSetting
    {
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public IDictionary<string, string> DataBase { get; set; }
        public bool IsProduction { get; set; }
        public bool IsLocalDevelopment { get; set; }
        public IDictionary<string, string> Secret { get; set; }
        public IDictionary<string, string> BaseURL { get; set; }
    }
}
