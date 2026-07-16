using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.BusinessLogic.Models
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
        public string division { get; set; }
    }

    public class Users
    {
        public string name { get; set; }
        public string email { get; set; }
        public string division { get; set; }
    }

    public class UserPost
    {
        public string IdamanId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
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
        public string Role { get; set; }
    }

    public class OrganizationDT
    {
        public string next { get; set; }
        public int start { get; set; }
        public int total { get; set; }
        public List<Organizations> value { get; set; }
    }

    public class Organizations
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string name { get; set; }
        public string companyCode { get; set; }
        public string lastModified { get; set; }
        public bool isPublished { get; set; }
    }

    public class RootValueOrg
    {
        public string next { get; set; }
        public int page { get; set; }
        public int total { get; set; }
        public List<ValueOrg> value { get; set; }
    }

    public class ValueOrg
    {
        public ExecutorType executorType { get; set; }
        public string id { get; set; }
        public Company company { get; set; }
        public Position position { get; set; }
        public User user { get; set; }
    }
}
