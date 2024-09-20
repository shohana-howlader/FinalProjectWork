using System.Diagnostics.Metrics;

namespace WebApplication1.Models
{
    public class State
    {
        public int StateID { get; set; }
        public string StateName { get; set; }

        // Foreign Key
        public int CountryID { get; set; }
        public Country Country { get; set; }

        // Navigation property
        public ICollection<Location> Locations { get; set; }
    }
}
