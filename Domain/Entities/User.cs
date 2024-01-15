using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public virtual ICollection<Image> Images { get; set; }
        public string CreatedAt { get; set; }
    }
}
