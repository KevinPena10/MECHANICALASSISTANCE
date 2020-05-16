using System.ComponentModel.DataAnnotations;

namespace MechanicalAssistance.Common.Models
{
    public class ProductRequest
    {
        [Required]
        public string ProductName { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public double Price { get; set; }
        public byte[] Photo { get; set; }

        [Required]
        public int ServiceId { get; set; }
        
        [Required]
        public int ProductBrandId { get; set; }

        [Required]
        public string CultureInfo { get; set; }

    }
}
