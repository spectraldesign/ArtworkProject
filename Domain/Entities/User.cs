using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string Username { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual User Creator { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
        public int Views { get; set; }
    }
}
