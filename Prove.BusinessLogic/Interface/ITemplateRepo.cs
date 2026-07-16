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
    public interface ITemplateRepo
    {
        Task<BaseDTResponseModel<TemplateJtable>> getTemplateDatatable(TemplateJtableParam param);
        Task<Template> detailTemplate(int id, CancellationToken cancellationToken);
        Task<bool> insertTemplate(TemplatePost param);
        Task<bool> updateTemplate(TemplatePost param);
        Task<bool> deleteTemplate(int id);
    }
}
