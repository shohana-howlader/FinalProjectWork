namespace WebApplication1.Models
{
    public class PackageFacility
    {
        public int PackageFacilityID { get; set; }
        public int PackageID { get; set; }
        public Package Package { get; set; }

        public int FacilityID { get; set; }
        public Facility Facility { get; set; }
    }
}