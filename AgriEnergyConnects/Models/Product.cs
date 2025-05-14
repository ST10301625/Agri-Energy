using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AgriEnergyConnects.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        public DateTime ProductionDate { get; set; }

        // 🔗 Link to Farmer

        public int FarmerId { get; set; }

        [ForeignKey("FarmerId")]
        [ValidateNever] // 🔒 Tell model binder to skip validation
        public Farmer Farmer { get; set; }
    }
}
