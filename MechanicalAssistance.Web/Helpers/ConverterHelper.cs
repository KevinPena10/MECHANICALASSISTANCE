using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Web.Data;
using MechanicalAssistance.Web.Data.Entities;
using MechanicalAssistance.Web.Models;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {

        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        public ConverterHelper(ICombosHelper combosHelper,
          DataContext context)
        { 
            _context = context;
            _combosHelper = combosHelper;
        }

        public async Task<ProductEntity> ToProductEntityAsync(ProductViewModel model, string path, bool isNew)
        {
            return new ProductEntity
            {
                Id = isNew ? 0 : model.Id,
                ProductName = model.ProductName,
                Description = model.Description,
                Price = model.Price,
                Photo = path,
                ProductBrand = await _context.ProductBrands.FindAsync(model.ProductBrandId),
                Service = await _context.Services.FindAsync(model.ServiceId)

            };
        }

        public ProductViewModel ToProductViewModel(ProductEntity productEntity)
        {
            return new ProductViewModel
            {
                Id = productEntity.Id,
                ProductName = productEntity.ProductName,
                Description = productEntity.Description,
                Price = productEntity.Price,
                Photo = productEntity.Photo,
                ProductBrandId = productEntity.ProductBrand.Id,
                ProductBrands = _combosHelper.GetComboProductsBrand(),
                ServiceId = productEntity.Service.Id

            };
        }

        public MechanicalServiceEntity ToServiceEntity(ServiceViewModel model, string path, bool isNew)
        {
            return new MechanicalServiceEntity
            {
                Id = isNew ? 0 : model.Id,
                ServiceName = model.ServiceName,
                Description = model.Description,
                Date = model.Date,
                Address = model.Address,
                LogoPath = path,
                User = model.User

            };
        }

        public ServiceViewModel ToServiceViewModel(MechanicalServiceEntity serviceEntity)
        {
            return new ServiceViewModel
            {
                Id = serviceEntity.Id,
                ServiceName = serviceEntity.ServiceName,
                Description = serviceEntity.Description,
                Date = serviceEntity.Date,
                Address = serviceEntity.Address,
                LogoPath = serviceEntity.LogoPath
            };
        }

        public UserResponse ToUserResponse(UserEntity user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserResponse
            {
                Address = user.Address,
                Document = user.Document,
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PicturePath = user.PicturePath,
                UserType = user.UserType
            };
        }
    }


}

