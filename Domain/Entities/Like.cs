using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Like
    {
        public string Id { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual User User { get; set; }
    }
}
