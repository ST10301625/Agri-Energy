namespace AgriEnergyConnects.Models.ViewModels
{
    public class FarmerProfileViewModel
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }  // Allow them to update the location
    }
}
