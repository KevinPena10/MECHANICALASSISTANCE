using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Common.Services;
using MechanicalAssistance.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ServiceRequestDetailsPageViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        public RequestServiceResponse _request;
        public RequestServiceResponse _requestR;
        private DelegateCommand _acceptCommand;
        private DelegateCommand _rejectCommand;
        private bool _isRunning;
        private bool _isEnabled;


        public ServiceRequestDetailsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.DetailsRequest;
            CurrentTime = DateTime.Now.TimeOfDay;
            Date = DateTime.Now.Date;
            LoadRequest();
        }
        public TimeSpan CurrentTime { get; set; }
        public DateTime Date { get; set; }
        public DelegateCommand Acceptcommand => _acceptCommand ?? (_acceptCommand = new DelegateCommand(AcceptRequestAsync));
        public DelegateCommand RejectCommand => _rejectCommand ?? (_rejectCommand = new DelegateCommand(RejectRequestAsync));

        public RequestServiceResponse RequestR
        {
            get => _requestR;
            set => SetProperty(ref _requestR, value);
        }


        private void LoadRequest()
        {
            _request = JsonConvert.DeserializeObject<RequestServiceResponse>(Settings.Request);
            RequestR = _request;
        }


        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        private async void AcceptRequestAsync()
        {
            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);

                return;
            }

            RequestServiceRequest Request = new RequestServiceRequest
            {
                Id = RequestR.Id,
                DateAndTime = Date + CurrentTime,
                Observation = RequestR.Observation,
                Status = Languages.statusAccept,
                ServiceId = RequestR.Service.Id,
                UserId = Guid.Parse(RequestR.User.Id),
                CultureInfo = Languages.Culture
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.UpdateRequestAsync(url, "/api", "/RequestService", Request, "bearer", token.Token);
            IsRunning = false;
           

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, response.Message, Languages.Accept);
            await _navigationService.NavigateAsync("/MechanicMasterDetailPage/NavigationPage/ServiceRequestsPage");
        }


        private async void RejectRequestAsync()
        {
            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);

                return;
            }

            RequestServiceRequest Request = new RequestServiceRequest
            {
                Id = RequestR.Id,
                DateAndTime = Date + CurrentTime,
                Observation = RequestR.Observation,
                Status = Languages.statusReject,
                ServiceId = RequestR.Service.Id,
                UserId = Guid.Parse(RequestR.User.Id),
                CultureInfo = Languages.Culture
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.UpdateRequestAsync(url, "/api", "/RequestService", Request, "bearer", token.Token);
            IsRunning = false;
        

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, response.Message, Languages.Accept);
            IsEnabled = true;
            await _navigationService.NavigateAsync("/MechanicMasterDetailPage/NavigationPage/ServiceRequestsPage");

        }






    }
}
