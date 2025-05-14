using System;
using System.Collections.Generic;

namespace AgriEnergyConnects.Models.ViewModels
{
    public class ProductFilterViewModel
    {
        // Fields to filter farmers products
        public string? Category { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<string> Categories { get; set; } = new();
        public List<Product> Products { get; set; } = new();
    }
}
