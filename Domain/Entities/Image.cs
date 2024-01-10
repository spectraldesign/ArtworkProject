namespace Domain.Entities
{
    public class Image
    {
        public string Id { get; set; }

        public string FileData { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public User Creator { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Comment> Comments { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Like> Likes { get; set; }

        public int Views { get; set; }
    }
}
