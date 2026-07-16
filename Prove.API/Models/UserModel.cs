using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Models
{
    public class Company
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class ExecutorType
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Organization
    {
        public string id { get; set; }
        public string name { get; set; }
        public string companyCode { get; set; }
    }

    public class Position
    {
        public string id { get; set; }
        public string name { get; set; }
        public string kbo { get; set; }
        public Organization organization { get; set; }
    }

    public class PosChildren
    {
        public string id { get; set; }
        public Company company { get; set; }
        public Position position { get; set; }
        public ExecutorType executorType { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
    }
}
