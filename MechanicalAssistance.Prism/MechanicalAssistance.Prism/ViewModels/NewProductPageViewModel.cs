using MechanicalAssistance.Prism.Helpers;
using Prism.Navigation;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class NewProductPageViewModel : ViewModelBase
    {
        public NewProductPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.titleNewProduct;
        }
    }
}
