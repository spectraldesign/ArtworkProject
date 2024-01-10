namespace Domain.Entities
{
    public class Comment
    {
        public string Id { get; set; }
        public string Content { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual User Creator { get; set; }
    }
}
