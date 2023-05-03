namespace main_backend.Models{
    public class NewPostModel{
        public decimal Limit { get; set; }

        public string Hour { get; set; } = null!;

        public string Minute { get; set; } = null!;
    }
}