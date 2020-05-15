using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicalAssistance.Common.Models
{
    public class ProductResponse
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Photo { get; set; }

        public string LogoFullPath => string.IsNullOrEmpty(Photo) ? "https://mechanicalassistanceweb.azurewebsites.net/images/noimage.png" : $"https://mechanicalassistanceweb.azurewebsites.net{Photo.Substring(1)}";
        public ServiceResponse Service { get; set; }
        public ProductBrandResponse ProductBrand { get; set; }
    }
}
