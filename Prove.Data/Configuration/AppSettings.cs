using JetBrains.Annotations;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Configuration
{
    [UsedImplicitly]
    public class AppSettings : BaseAppSettings
    {
        public IDictionary<string, string> SmsSettingStrings { get; set; }
        public IDictionary<string, string> TelkomSatelitSettings { get; set; }
        public IDictionary<string, string> EmailSetting { get; set; }
    }
}
