using System.Collections.Generic;

namespace MechanicalAssistance.Common.Models
{
    public class ProductBrandResponse
    {
        public int Id { get; set; }

        public string BrandName { get; set; }

        public ICollection<ProductResponse> Products { get; set; }
    }
}
