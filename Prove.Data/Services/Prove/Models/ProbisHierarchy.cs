using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.Data.Services.Prove.Models
{
    public class ProbisHierarchy
    {
        public string ParentProbisNumber { get; set; }
        public string ChildProbisNumber { get; set; }
        public string Description { get; set; }
        public string Probis { get; set; }
        public string Id_String { get; set; }
        public int Id { get; set; }
        public string ProbisNumber { get; set; }
        public string ParentId { get; set; }
        public int UnderParent { get; set; }
        public string UnderParent_String { get; set; }
        public int LEVEL_DEPTH { get; set; }
    }
}
