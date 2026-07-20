using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Common
{
    public class IdentityUserResult(string id, string email, string displayName, string userName)
    {

        public string Id { get; set; } = id;
        public string Email { get; set; } = email;
        public string DisplayName { get; set; } = displayName;
        public string UserName { get; set; } = userName;
    }
}
