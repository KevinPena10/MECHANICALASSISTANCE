using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class RequestServiceitemViewModel : RequestServiceResponse
    {

        private readonly INavigationService _navigationService;
        private DelegateCommand _selectCommand;

        public RequestServiceitemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectCommand => _selectCommand ??
          (_selectCommand = new DelegateCommand(SelectAsync));

        private async void SelectAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "request", this }
            };

            Settings.Request = JsonConvert.SerializeObject(this);

            await _navigationService.NavigateAsync(nameof(ServiceRequestDetailsPage), parameters);
        }
    }
}
