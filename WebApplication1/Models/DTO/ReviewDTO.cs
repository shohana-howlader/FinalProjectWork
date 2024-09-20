namespace WebApplication1.Models.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; } 
        public DateTime DatePosted { get; set; }
    }
}
