using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.Utilities.Base
{
    public enum Category
    {
        Perihal,
        Number
    }

    public enum RegulationCategory
    {
        Eksternal,
        Internal
    }

    public enum StatusDoc
    {
        TidakBerlaku,
        Berlaku
    }

    public enum TypeSurat
    {
        Perintah,
        Keputusan
    }

    public enum DatabaseEnums
    {
        AppUserDb,
        CorePTKDb,
        ProveExtDb,
        CronJobDb,
        UsmanDb
    }
}
