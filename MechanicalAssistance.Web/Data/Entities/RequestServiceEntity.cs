using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Data.Entities
{
    public class RequestServiceEntity
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date and Time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime DateAndTime { get; set; }

        [Display(Name = "Picture")]
        public string RequestPhoto { get; set; }

        [MaxLength(150, ErrorMessage = "The field {0} can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Observation of the Request")]
        public string Observation { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        public MechanicalServiceEntity Service { get; set; }
        public UserEntity User { get; set; }

    }
}
