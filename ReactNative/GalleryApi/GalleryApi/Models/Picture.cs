namespace GalleryApi.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Tags { get; set; }
        public string Data { get; set; }
        public string SentBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
