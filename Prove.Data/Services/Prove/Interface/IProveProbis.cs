using Prove.Data.Dao.Prove;
using Prove.Data.Services.Prove.Models;
using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.Prove.Interface
{
    public interface IProveProbis : IBaseCrud<Probis>
    {
        Task<List<ProbisHierarchy>> GetProbisHierarchy();
    }
}
