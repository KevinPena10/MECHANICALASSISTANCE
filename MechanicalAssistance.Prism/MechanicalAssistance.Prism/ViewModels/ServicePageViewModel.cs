using MechanicalAssistance.Common.Services;
using Prism.Navigation;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ServicePageViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        public ServicePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Service quemado";
        }
    }
}
