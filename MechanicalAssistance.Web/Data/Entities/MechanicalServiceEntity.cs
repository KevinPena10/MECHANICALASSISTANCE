using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Data.Entities
{
    public class MechanicalServiceEntity
    {
        [Display(Name = "#")]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Name")]
        public string ServiceName { get; set; }

        [MaxLength(200, ErrorMessage = "The field {0} can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Service description")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Publication Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        
        public DateTime Date { get; set; }

        [MaxLength(500, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Address { get; set; }

        [Display(Name = "Logo")]
        public string LogoPath { get; set; }

        public UserEntity User { get; set; }

        public ICollection<ProductEntity> Products { get; set; }

        public ICollection<RequestServiceEntity> RequestServices { get; set; }

    }
}
