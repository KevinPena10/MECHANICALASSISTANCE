using MechanicalAssistance.Prism.Helpers;
using Prism.Navigation;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ServiceDetailsPageViewModel : ViewModelBase
    {
        public ServiceDetailsPageViewModel(INavigationService navigationService) : base(navigationService) {

            Title = Languages.titleServiceDetails;
        }
    }
}
