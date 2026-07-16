using Prove.Utilities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Dao.CorePTK
{
    public class Ship : BaseDao
    {
        [Required]
        [StringLength(200, ErrorMessage = "Limit ship name to 30 characters.")]
        public string Name { get; set; }

        [StringLength(15, ErrorMessage = "Limit ship code to 15 characters.")]
        public string ShipCode { get; set; }

        [StringLength(200, ErrorMessage = "Limit ship alias to 15 characters.")]
        public string ShipAlias { get; set; }

        [StringLength(10, ErrorMessage = "Limit callsign to 10 characters.")]
        public string Callsign { get; set; }

        [StringLength(10, ErrorMessage = "Limit grt to 10 characters.")]
        public string Grt { get; set; }

        [StringLength(4, ErrorMessage = "Limit build year to 30 characters.")]
        public string BuildYear { get; set; }

        [StringLength(15, ErrorMessage = "Limit imo to 15 characters.")]
        public string Imo { get; set; }

        [StringLength(15, ErrorMessage = "Limit mmsi to 15 characters.")]
        public string Mmsi { get; set; }

        [StringLength(15, ErrorMessage = "Limit phone to 15 characters.")]
        public string Phone { get; set; }

        [StringLength(50, ErrorMessage = "Limit email to 50 characters.")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "Limit email2 to 50 characters.")]
        public string Email2 { get; set; }

        public string Detail { get; set; }
        public virtual ShipCategory ShipCategory { get; set; }
        public virtual ShipStatus ShipStatus { get; set; }
        public virtual ShipOwner ShipOwner { get; set; }
        public virtual ShipAuthor ShipAuthor { get; set; }

        [StringLength(10, ErrorMessage = "Limit dwt to 10 characters.")]
        public string Dwt { get; set; }

        [StringLength(10, ErrorMessage = "Limit yob to 10 characters.")]
        public string Yob { get; set; }

        [StringLength(10, ErrorMessage = "Limit loa to 10 characters.")]
        public string Loa { get; set; }

        [StringLength(10, ErrorMessage = "Limit nrt to 10 characters.")]
        public string Nrt { get; set; }

        public int SequenceNumber { get; set; }

        [StringLength(20)]
        public string HullNo { get; set; }

        public string HP { get; set; }

        public string PropulsionEngine { get; set; }

        public string PortOfRegistry { get; set; }
        public string Depth { get; set; }
        public string MainEngine { get; set; }
        public virtual ICollection<ShipEmployee> ShipEmployees { get; set; }
        public virtual ICollection<ShipArea> ShipAreas { get; set; }
        public string Capacity { get; set; }
        public string Draft { get; set; }
        public string Breadth { get; set; }
        public string Keterangan { get; set; }
        public string CharterParty { get; set; }
        public string Builder { get; set; }
    }
}
