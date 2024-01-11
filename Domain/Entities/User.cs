using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string Username { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Image> Images { get; set; }
    }
}
