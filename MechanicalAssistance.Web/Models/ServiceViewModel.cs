using MechanicalAssistance.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MechanicalAssistance.Web.Models
{
    public class ServiceViewModel : MechanicalServiceEntity
    {
        [Display(Name = "Service Photo")]
        public IFormFile ServiceFile { get; set; }
    }
}
