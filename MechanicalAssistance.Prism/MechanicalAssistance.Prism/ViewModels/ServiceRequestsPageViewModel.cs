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
    public class ServiceRequestsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<RequestServiceitemViewModel> _requests;
        private bool _isRunning;
        private bool _isVisible;


        public ServiceRequestsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.titleRequest;
            LoadRequestAsync();
        }


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



        public List<RequestServiceitemViewModel> Requests
        {
            get => _requests;
            set => SetProperty(ref _requests, value);
        }


        private async void LoadRequestAsync()
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
                UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                Response response = await _apiService.GetListAsync<RequestServiceResponse>(url, "/api", "/RequestService", "bearer", token.Token);

                IsRunning = false;

                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                }


                List<RequestServiceResponse> requests = (List<RequestServiceResponse>)response.Result;

                var list = new List<RequestServiceResponse>();

                foreach (var item in requests)
                {
                    if (item.Service.User.Id == user.Id)
                    {
                        if (item.Status == Languages.statusFirst)
                        {
                            list.Add(new RequestServiceResponse
                            {
                                Id = item.Id,
                                DateAndTime = item.DateAndTime,
                                Observation = item.Observation,
                                Status = item.Status,
                                Photo = item.Photo,
                                Service = item.Service,
                                User = item.User
                            }); ;
                        }
                    }
                }
                if (list.Count != 0)
                {
                 IsVisible = false;
                Requests = list.Select(t => new RequestServiceitemViewModel(_navigationService)
                {
                    Id = t.Id,
                    DateAndTime = t.DateAndTime.ToLocalTime(),
                    Observation = t.Observation,
                    Status = t.Status,
                    Photo = t.Photo,
                    Service = t.Service,
                    User = t.User
                }).ToList();

                } else {
                    IsVisible = true;
                }

            }

        }
    }
}
