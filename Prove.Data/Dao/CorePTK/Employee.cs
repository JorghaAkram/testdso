using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class Employee : BaseDao
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Nip { get; set; }

        [StringLength(50)]
        public string FamilyCardNo { get; set; }

        [StringLength(50)]
        public string IndetityCardNo { get; set; }

        [StringLength(400)]
        public string IdentityCardAddress { get; set; }

        [StringLength(400)]
        public string CurrentAddress { get; set; }

        [Required]
        public char Gender { get; set; }

        public DateTime Birthdate { get; set; }

        [StringLength(50)]
        public string BirthPlace { get; set; }

        [StringLength(50)]
        public string Npwp { get; set; }

        [StringLength(50)]
        public string PhoneNo { get; set; }

        [StringLength(50)]
        public string PolisMaps { get; set; }

        public virtual Religion Religion { get; set; }

        public virtual MaritalStatus MaritalStatus { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public virtual EmployeePosition EmployeePosition { get; set; }

        public virtual ICollection<ShipEmployee> ShipEmployees { get; set; }

        public virtual ICollection<PortEmployee> PortEmployees { get; set; }

        public DateTime? TmtJoin { get; set; }
        public DateTime? TmtOrganik { get; set; }
        public DateTime? TmtPensiun { get; set; }
        public DateTime? TmtMppk { get; set; }
        public string GolonganDarah { get; set; }
        public string WearPackSize { get; set; }
        public string ShoesSize { get; set; }
        public string EmergencyPhone { get; set; }
        public string BankAccount { get; set; }
        public virtual Bank Bank { get; set; }
    }
}
