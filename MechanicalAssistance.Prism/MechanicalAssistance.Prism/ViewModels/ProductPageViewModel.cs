using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Common.Services;
using MechanicalAssistance.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ProductPageViewModel : ViewModelBase
    {
        public int ServiceId;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ServiceResponse _service;
        private List<ProductResponse> _Products;
        private bool _isRunning;
    

        public ProductPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.titleProducts;
            LoadProductsAsync();
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public List<ProductResponse> Products
        {
            get => _Products;
            set => SetProperty(ref _Products, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _service = parameters.GetValue<ServiceResponse>("service");
            ServiceId = _service.Id;

        }

        private async void LoadProductsAsync()
        {
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
            }
            else
            {
                Response response = await _apiService.GetListAsync<ProductResponse>(url, "/api", "/Products");

                IsRunning = false;

                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                }


                List<ProductResponse> products = (List<ProductResponse>)response.Result;

                var list = new List<ProductResponse>();

                foreach (var item in products)
                {
                    if (item.Service.Id == ServiceId)
                    {

                        list.Add(new ProductResponse
                        {

                            Id = item.Id,
                            ProductName = item.ProductName,
                            Description = item.Description,
                            Price = item.Price,
                            Photo = item.Photo,
                            ProductBrand = item.ProductBrand
                        });
                    }

                }

                Products = list.Select(t => new ProductResponse
                {
                    Id = t.Id,
                    ProductName = t.ProductName,
                    Description = t.Description,
                    Price = t.Price,
                    Photo = t.Photo,
                    ProductBrand = t.ProductBrand
                }).ToList();

            }

        }


    }
}
