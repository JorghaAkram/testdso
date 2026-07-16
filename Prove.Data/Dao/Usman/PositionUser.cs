using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.Usman
{
    public class PositionUser : BaseDao
    {
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationParentId { get; set; }
        public string OrganizationParentName { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionParentId { get; set; }
        public string PositionParentName { get; set; }
    }
}
