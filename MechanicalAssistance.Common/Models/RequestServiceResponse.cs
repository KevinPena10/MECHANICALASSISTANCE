using System;

namespace MechanicalAssistance.Common.Models
{
    public class RequestServiceResponse
    {

        public int Id { get; set; }

        public DateTime DateAndTime { get; set; }

        public string Photo { get; set; }

        public string Observation { get; set; }

  
        public string Status { get; set; }

        public ServiceResponse Service { get; set; }

        public UserResponse User { get; set; }

        public string photoPath => string.IsNullOrEmpty(Photo) ? "https://mechanicalassistancewebk.azurewebsites.net/images/noimage.png" : $"https://mechanicalassistancewebk.azurewebsites.net{Photo.Substring(1)}";

    }
}
