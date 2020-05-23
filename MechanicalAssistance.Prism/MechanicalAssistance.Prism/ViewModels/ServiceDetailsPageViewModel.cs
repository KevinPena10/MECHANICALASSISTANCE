using MechanicalAssistance.Common.Enums;
using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Prism.Helpers;
using MechanicalAssistance.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ServiceDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ServiceResponse _service;
        private ServiceDetailResponse _detail;
        private DelegateCommand _requestPasswordCommand;
        private UserResponse _user;
        private bool _isVisible;

        public ServiceDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = Languages.titleServiceDetails;
            Detail = new ServiceDetailResponse();
            LoadDetails();
        }
        public DelegateCommand RequestCommand => _requestPasswordCommand ?? (_requestPasswordCommand = new DelegateCommand(RequestServiceAsync));


        public ServiceDetailResponse Detail
        {
            get => _detail;
            set => SetProperty(ref _detail, value);
        }

        public bool IsEnabled
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private async void RequestServiceAsync()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                if (User.UserType == UserType.User || User.UserType == UserType.Admin)
                {
                    IsEnabled = true;
                    await _navigationService.NavigateAsync(nameof(NewRequestServicePage));
                }
                else
                {
                    IsEnabled = false;
                    await App.Current.MainPage.DisplayAlert(Languages.info, Languages.rol, Languages.Accept);
                }
            }
            else
            {
                IsEnabled = false;
                await NavigationService.NavigateAsync("/MechanicMasterDetailPage/NavigationPage/LoginPage");

            }

        }


        private void LoadDetails()
        {
            _service = JsonConvert.DeserializeObject<ServiceResponse>(Settings.Service);
            Detail.LogoPath = _service.LogoFullPath;
            Detail.ServiceName = _service.ServiceName;
            Detail.Description = _service.Description;
            Detail.UserName = _service.User.FullName;
            Detail.Address = _service.Address;
            Detail.PhoneNumber = _service.User.PhoneNumber;
            Detail.latitude = _service.latitude;
            Detail.length = _service.length;

            ServiceDetailsPage.GetInstance();
        }


    }
}
