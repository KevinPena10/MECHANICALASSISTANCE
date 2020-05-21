using System;

namespace MechanicalAssistance.Common.Models
{
    public class ServiceResponse
    {
        public int Id { get; set; }

        public string ServiceName { get; set; }
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string Address { get; set; }

        public string LogoPath { get; set; }

        public string LogoFullPath => string.IsNullOrEmpty(LogoPath) ? "https://mechanicalassistancewebk.azurewebsites.net/images/noimage.png" : $"https://mechanicalassistancewebk.azurewebsites.net{LogoPath.Substring(1)}";

        public float latitude { get; set; }

        public float length { get; set; }

        public UserResponse User { get; set; }

    }
}
