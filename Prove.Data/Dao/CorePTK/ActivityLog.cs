using Prove.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class ActivityLog
    {
        [Required]
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public int TrxId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(40)]
        public string Controller { get; set; }

        [StringLength(40)]
        public string Method { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(400)]
        public string Info { get; set; } = PrideConstant.GetLocalIPAddress();

        public string ErrorMessage { get; set; }

        [StringLength(400)]
        public string Remark { get; set; }
    }
}
