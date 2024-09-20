namespace WebApplication1.Models
{
    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        // Navigation property
        public ICollection<State> States { get; set; }
    }
}