namespace WebApplication1.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}