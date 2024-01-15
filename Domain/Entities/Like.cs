namespace Domain.Entities
{
    public class Like
    {
        public string Id { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual User User { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Image Image { get; set; }
    }
}
