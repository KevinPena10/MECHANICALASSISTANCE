using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Prism.Helpers;
using MechanicalAssistance.Prism.Views;
using Newtonsoft.Json;
using Prism.Navigation;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ServiceDetailsPageViewModel : ViewModelBase
    {
        private ServiceResponse _service;
        private ServiceDetailResponse _detail;

        public ServiceDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {

            Title = Languages.titleServiceDetails;
            Detail = new ServiceDetailResponse();
            LoadDetails();
        }

        public ServiceDetailResponse Detail
        {
            get => _detail;
            set => SetProperty(ref _detail, value);
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
