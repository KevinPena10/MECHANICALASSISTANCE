using MechanicalAssistance.Web.Data.Entities;
using MechanicalAssistance.Web.Models;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<ProductEntity>ToProductEntityAsync(ProductViewModel model, string path, bool isNew);
        ProductViewModel ToProductViewModel(ProductEntity productEntity);
    }
}
