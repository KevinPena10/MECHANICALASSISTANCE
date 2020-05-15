using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Data.Entities
{
    public class ProductEntity
    {
        [Display(Name = "#")]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Product name")]
        public string ProductName { get; set; }

        [MaxLength(150, ErrorMessage = "The field {0} can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        [Display(Name = "Price of the product")]
        public double Price { get; set; }

        [Display(Name = "Product Photo")]
        public string Photo { get; set; }

        public MechanicalServiceEntity Service { get; set; }
        public ProductBrandEntity ProductBrand { get; set; }


    }
}
