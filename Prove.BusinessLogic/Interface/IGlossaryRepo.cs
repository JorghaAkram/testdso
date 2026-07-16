using Prove.BusinessLogic.Models;
using Prove.Data.Dao.Prove;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prove.BusinessLogic.Interface
{
    public interface IGlossaryRepo
    {
        Task<BaseDTResponseModel<GlossaryJtable>> getGlossaryDatatable(GlossaryJtableParam param);
        Task<Glossary> detailGlossary(int id, CancellationToken cancellationToken);
        Task<bool> insertGlossary(GlossaryPost param);
        Task<bool> updateGlossary(GlossaryPost param);
        Task<bool> deleteGlossary(int id);
    }
}
