using MechanicalAssistance.Common.Enums;
using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Common.Services;
using MechanicalAssistance.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ProductPageViewModel : ViewModelBase
    {
        public int serviceId;
        public string userEmail;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ServiceResponse _service;
        private List<ProductResponse> _products;
        private static ProductPageViewModel _instance;
        private UserResponse _user;
        private bool _isRunning;
        private bool _isVisible;
        private DelegateCommand _createCommand;


        public ProductPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.titleProducts;
            IsVisible = false;
            LoadProductsAsync();
          
        }

        public DelegateCommand CreateCommand => _createCommand ?? (_createCommand = new DelegateCommand(RedirectNewProductAsync));
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }


        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public List<ProductResponse> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public static ProductPageViewModel GetInstance()
        {
            return _instance;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _service = parameters.GetValue<ServiceResponse>("service");
            serviceId = _service.Id;
            userEmail = _service.User.Email;
            LoadUser();
        }

        public async void ReloadUser()
        {
            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                return;
            }

            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            EmailRequest emailRequest = new EmailRequest
            {
                Email = user.Email,
                CultureInfo = Languages.Culture

            };

            Response response = await _apiService.GetUserByEmail(url, "/api", "/Account/GetUserByEmail", "bearer", token.Token, emailRequest);
            UserResponse userResponse = (UserResponse)response.Result;
            Settings.User = JsonConvert.SerializeObject(userResponse);
            LoadUser();
        }

        public async void LoadProductsAsync()
        {
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsVisible = false;
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
                    if (item.Service.Id == serviceId)
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

        private async void RedirectNewProductAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "newProduct", serviceId }
            };

            await _navigationService.NavigateAsync("NewProductPage", parameters);
        }


        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                if (User.UserType == UserType.Mechanic || User.UserType == UserType.Admin)
                {
                    if (User.Email == userEmail)
                    {
                        IsVisible = true;
                    }
                    else
                    {
                        IsVisible = false;
                    }
                }
                else
                {

                    IsVisible = false;
                }

            }
            else
            {
                IsVisible = false;
            }
        }

    }
}
