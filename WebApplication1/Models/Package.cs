namespace WebApplication1.Models
{
    public class Package
    {
        public int PackageID { get; set; }
        public string PackageTitle { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}