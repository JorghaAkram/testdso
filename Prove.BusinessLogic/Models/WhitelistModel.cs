using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.BusinessLogic.Models
{
    public class WhitelistModel
    {
        public class Application
        {
            public string id { get; set; }
            public string applicationName { get; set; }
        }

        public class Role
        {
            public string id { get; set; }
            public string roleName { get; set; }
            public Application application { get; set; }
        }

        public class Root
        {
            public object next { get; set; }
            public int start { get; set; }
            public int total { get; set; }
            public List<Value> value { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public string displayName { get; set; }
            public string email { get; set; }
        }

        public class Value
        {
            public string id { get; set; }
            public User user { get; set; }
            public List<Role> role { get; set; }
            public bool isDeleted { get; set; }
        }

    }
}
