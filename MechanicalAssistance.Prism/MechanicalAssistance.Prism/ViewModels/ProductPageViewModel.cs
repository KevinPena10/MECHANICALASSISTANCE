using MechanicalAssistance.Prism.Helpers;
using Prism.Navigation;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class ProductPageViewModel : ViewModelBase
    {

        public ProductPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.titleProducts;
        }


    }
}
