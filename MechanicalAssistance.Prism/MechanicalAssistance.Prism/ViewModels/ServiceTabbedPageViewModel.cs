using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Prism.Helpers;
using Prism.Navigation;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ServiceTabbedPageViewModel : ViewModelBase
    {
        private ServiceResponse _service;
        public ServiceTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _service = parameters.GetValue<ServiceResponse>("service");
            Title = _service.ServiceName;

        }

    }
}

