using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.CorePTK.Models
{
    public class BirthDayEmployeeModel
    {
        public string NIP { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string BirthDate
        {
            get
            {
                return BirthDT.ToString("dd MMM yyyy");
            }
            set { }
        }
        public DateTime BirthDT { get; set; }
        public string Division { get; set; }
    }
}
