namespace MechanicalAssistance.Common.Models
{
    public class ServiceDetailResponse
    {
        public string LogoPath { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public float latitude { get; set; }

        public float length { get; set; }
    }
}
