using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Common.Services;
using MechanicalAssistance.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ServicePageViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<ServiceItemViewModel> _services;
        private UserResponse _user;
        private bool _isRunning;

        public ServicePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.ServiceMenu;

            LoadUser();
            LoadServiceAsync();
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }


        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public List<ServiceItemViewModel> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private async void LoadServiceAsync()
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
       
                Response response = await _apiService.GetListAsync<ServiceResponse>(url, "/api", "/Service");

                IsRunning = false;

                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                }


                List<ServiceResponse> services = (List<ServiceResponse>)response.Result;



                Services = services.Select(s => new ServiceItemViewModel(_navigationService)
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName,
                    Description = s.Description,
                    Date = s.Date,
                    Address = s.Address,
                    LogoPath = s.LogoPath,
                    User = s.User
                }).ToList();


            }

        }


    }
}
