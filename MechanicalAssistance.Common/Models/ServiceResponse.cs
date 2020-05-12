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

        public string LogoFullPath => string.IsNullOrEmpty(LogoPath) ? "https://mechanicalassistanceweb.azurewebsites.net/images/noimage.png" : $"https://mechanicalassistanceweb.azurewebsites.net{LogoPath.Substring(1)}";

        public UserResponse User { get; set; }

    }
}
