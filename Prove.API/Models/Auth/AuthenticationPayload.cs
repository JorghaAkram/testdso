using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prove.API.Models.Auth
{
    public class Response
    {
        public string Token { get; set; }
        public object FirstName { get; set; }
        public object LastName { get; set; }
        public string FullName { get; set; }
        public string Nip { get; set; }
        public string Username { get; set; }
        public string AspUserId { get; set; }
        public string Email { get; set; }
    }
    public class AuthenticationPayload
    {
        public bool Successfull { get; set; }
        public object ErrorMessage { get; set; }
        public Response Response { get; set; }
    }

    public class AuthIdaman
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }

    public class UserResponse
    {
        public string next { get; set; }
        public int page { get; set; }
        public int total { get; set; }
        public List<value> value { get; set; }
    }

    public class value
    {
        public string id { get; set; }
        public string userId { get; set; }
        public position position { get; set; }
        public string companyCode { get; set; }
        public string companyName { get; set; }
        public bool isActive { get; set; } = true;
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string displayName { get; set; }
        public string employeeId { get; set; }
        public string lastName { get; set; }
        public string jobTitle { get; set; }
        public string email { get; set; }
        public string mobilePhone { get; set; }
        public string aboutMe { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string address { get; set; }
        public string photo { get; set; }
        public extensionAttributes extensionAttributes { get; set; }
        public string idp { get; set; }
        public string directoryId { get; set; }
        public string lastModified { get; set; }
        public string employeeNumber { get; set; }
        public string employeeType { get; set; }
        public string cultureInfo { get; set; }
        public string language { get; set; }
        public string dateFormat { get; set; }
        public string timeFormat { get; set; }
        public applicationParams applicationParams { get; set; }
    }

    public class applicationParams
    {
        public application application { get; set; }
        public customParameters customParameters { get; set; }
    }

    public class application
    {
        public string id { get; set; }
        public string clientId { get; set; }
        public string name { get; set; }
        public createdBy createdBy { get; set; }
        public string lastModified { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string photo { get; set; }
        public string clientSecret { get; set; }
        public string logoutUrl { get; set; }
        public string homePageUrl { get; set; }
        public string termofServiceUrl { get; set; }
        public string privacyStatementUrl { get; set; }
        public string publisherDomain { get; set; }
        public string redirectUrl { get; set; }
        public bool isPublished { get; set; } = true;
        public int rattings { get; set; }
        public string technologies { get; set; }
        public string databases { get; set; }
        public string programs { get; set; }
        public string requirements { get; set; }
    }

    public class customParameters
    {
        public List<additionalProp> additionalProp1 { get; set; } = null;
        public List<additionalProp> additionalProp2 { get; set; } = null;
        public List<additionalProp> additionalProp3 { get; set; } = null;
    }

    public class extensionAttributes
    {
        public List<additionalProp> additionalProp1 { get; set; } = null;
        public List<additionalProp> additionalProp2 { get; set; } = null;
        public List<additionalProp> additionalProp3 { get; set; } = null;
    }

    public class position
    {
        public string id { get; set; }
        public string name { get; set; }
        public organization organization { get; set; }
        public string kbo { get; set; }
        public createdBy createdBy { get; set; }
        public string lastModified { get; set; }
        public bool isPublished { get; set; } = true;
        public bool isHead { get; set; } = true;
        public bool isOwner { get; set; } = true;
        public bool isChief { get; set; } = true;
    }

    public class organization
    {
        public string id { get; set; }
        public string name { get; set; }
        public string companyCode { get; set; }
    }

    public class createdBy
    {
        public List<additionalProp> additionalProp1 { get; set; } = null;
        public List<additionalProp> additionalProp2 { get; set; } = null;
        public List<additionalProp> additionalProp3 { get; set; } = null;
    }

    public class additionalProp{ }

    public class Application
    {
        public string id { get; set; }
        public string clientId { get; set; }
        public CreatedBy createdBy { get; set; }
        public string lastModified { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string photo { get; set; }
        public string clientSecret { get; set; }
        public string logoutUrl { get; set; }
        public string homePageUrl { get; set; }
        public string termofServiceUrl { get; set; }
        public string privacyStatementUrl { get; set; }
        public string publisherDomain { get; set; }
        public string redirectUrl { get; set; }
        public bool isPublished { get; set; }
        public decimal rattings { get; set; }
        public string technologies { get; set; }
        public string databases { get; set; }
        public string programs { get; set; }
        public string requirements { get; set; }
    }

    public class CreatedBy
    {
        public List<object> additionalProp1 { get; set; }
        public List<object> additionalProp2 { get; set; }
        public List<object> additionalProp3 { get; set; }
    }

    public class Organization
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Position
    {
        public string id { get; set; }
        public string name { get; set; }
        public string kbo { get; set; }
        public Organization organization { get; set; }
    }

    public class Role
    {
        public string id { get; set; }
        public string name { get; set; }
        public Position position { get; set; }
        public Application application { get; set; }
    }

    public class Roles
    {
        public string type { get; set; }
        public List<Role> roles { get; set; }
    }
}
