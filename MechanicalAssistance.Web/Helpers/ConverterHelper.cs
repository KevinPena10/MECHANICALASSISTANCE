﻿using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Web.Data;
using MechanicalAssistance.Web.Data.Entities;
using MechanicalAssistance.Web.Models;
using System.Collections.Generic;
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
                latitude = model.latitude,
                length = model.length,
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
                latitude = serviceEntity.latitude,
                length = serviceEntity.length,
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
                UserType = user.UserType,

                LoginType = user.LoginType

            };
        }

        public ServiceResponse ToServiceResponse(MechanicalServiceEntity serviceEntity)
        {
            return new ServiceResponse
            {
                Id = serviceEntity.Id,
                ServiceName = serviceEntity.ServiceName,
                Description = serviceEntity.Description,
                Date = serviceEntity.Date,
                Address = serviceEntity.Address,
                LogoPath = serviceEntity.LogoPath,
                latitude = serviceEntity.latitude,
                length = serviceEntity.length,
                User = ToUserResponse(serviceEntity.User)
            };
        }

        public List<ServiceResponse> ToServiceResponse(List<MechanicalServiceEntity> servicesEntities)
        {
            List<ServiceResponse> list = new List<ServiceResponse>();
            foreach (MechanicalServiceEntity serviceEntity in servicesEntities)
            {
                list.Add(ToServiceResponse(serviceEntity));
            }

            return list;
        }


        public RequestServiceResponse ToRequestServiceResponse(RequestServiceEntity requestServiceEntity)
        {
            return new RequestServiceResponse
            {
                Id = requestServiceEntity.Id,
                DateAndTime = requestServiceEntity.DateAndTime,
                Photo = requestServiceEntity.RequestPhoto,
                Observation = requestServiceEntity.Observation,
                Status = requestServiceEntity.Status,
                Service = ToServiceResponseTwo(requestServiceEntity.Service),
                User = ToUserResponse(requestServiceEntity.User)
            };
        }



        public List<RequestServiceResponse> ToRequestServiceResponse(List<RequestServiceEntity> requestServiceEntities)
        {
            List<RequestServiceResponse> list = new List<RequestServiceResponse>();
            foreach (RequestServiceEntity requestServiceEntity in requestServiceEntities)
            {
                list.Add(ToRequestServiceResponse(requestServiceEntity));
            }

            return list;
        }



        public ProductResponse ToProductsResponse(ProductEntity productEntity)
        {
            return new ProductResponse
            {
                Id = productEntity.Id,
                ProductName = productEntity.ProductName,
                Description = productEntity.Description,
                Price = productEntity.Price,
                Photo = productEntity.Photo,
                Service = ToServiceResponseTwo(productEntity.Service),
                ProductBrand = ToProductBrandResponse(productEntity.ProductBrand)
            };
        }

        public List<ProductResponse> ToProductsResponse(List<ProductEntity> productEntities)
        {
            List<ProductResponse> list = new List<ProductResponse>();
            foreach (ProductEntity ProductEntity in productEntities)
            {
                list.Add(ToProductsResponse(ProductEntity));
            }

            return list;
        }

        private ServiceResponse ToServiceResponseTwo(MechanicalServiceEntity Service)
        {
            if (Service == null)
            {
                return null;
            }

            return new ServiceResponse
            {
                Id = Service.Id,
                ServiceName = Service.ServiceName,
                Description = Service.Description,
                Date = Service.Date,
                Address = Service.Address,
                LogoPath = Service.LogoPath,
                latitude = Service.latitude,
                length = Service.length,
                User = ToUserResponse(Service.User)
            };
        }

        private ProductBrandResponse ToProductBrandResponse(ProductBrandEntity ProductBrand)
        {
            if (ProductBrand == null)
            {
                return null;
            }

            return new ProductBrandResponse
            {
                Id = ProductBrand.Id,
                BrandName = ProductBrand.BrandName

            };
        }
    }


}

