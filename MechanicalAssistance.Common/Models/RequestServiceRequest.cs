using System;
using System.ComponentModel.DataAnnotations;

namespace MechanicalAssistance.Common.Models
{
    public class RequestServiceRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime DateAndTime { get; set; }

        public byte[] Photo { get; set; }

        [Required]
        public string Observation { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public Guid UserId { get; set; }


        [Required]
        public string CultureInfo { get; set; }
    }
}
