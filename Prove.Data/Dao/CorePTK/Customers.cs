using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class Customers : BaseDao
    {
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Website { get; set; }

        public string Owner { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [DataType(DataType.Text)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string Province { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(100)]
        public string Industry { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string Flag { get; set; }
        public string Alias { get; set; }
        public string Code { get; set; }

        // CRM
        [StringLength(50)]
        public string TelephoneExt { get; set; }
        public string AddressOther { get; set; }
        public int AreaId { get; set; } //{sumbagut,sumbangsel,jawa1,jawa2}
        public string Email { get; set; }
        public string BusinessType { get; set; } //{Oil & Gas,Shipping Business,Marine Business,Logistic Business,Lain-lain}
        public string BusinessActivities { get; set; }
        public string CustomerType { get; set; } // {Active,Non Active,Potensial}
        public string Note { get; set; }
        public string Category { get; set; } // {Pertamina Group,Non Pertamina}
        public string Feedback { get; set; } // diambil dari we care
        public string FinancialInfo { get; set; } // {Lancar,Bad Debt}
        public string FinancialInfoNominal { get; set; } // nilai hutang saat ini 
    }
}
