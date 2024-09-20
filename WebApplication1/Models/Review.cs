namespace WebApplication1.Models
{
    public class Review
    {
        public int Id { get; set; } // Primary Key

        // Foreign Key to the Package
        public int PackageId { get; set; }
        public Package Package { get; set; } // Navigation property to Package

        // Foreign Key to the User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } // Navigation property to User

        public int Rating { get; set; } // Rating for the package
        public string Comment { get; set; } // User's comment
        public DateTime DatePosted { get; set; } // Date of the review
    }
}
