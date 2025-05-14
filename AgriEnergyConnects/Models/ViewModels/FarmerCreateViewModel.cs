// FarmerCreateViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;

public class FarmerCreateViewModel
{
    public string UserId { get; set; }  
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; } = "000-000-0000";
    public string Location { get; set; } = "Unknown Location";
    public List<SelectListItem> Users { get; set; }
}