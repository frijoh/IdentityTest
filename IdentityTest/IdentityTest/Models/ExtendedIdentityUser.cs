
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IdentityTest.Models
{
    public class ExtendedIdentityUser : IdentityUser
    {
        [JsonProperty(PropertyName = "id")]
        public string DocumentId { get; set; }

        public IList<string> Roles { get; set; }

        public ExtendedIdentityUser(): base()
        {
            Roles = new List<string>();
        }
    }
}
