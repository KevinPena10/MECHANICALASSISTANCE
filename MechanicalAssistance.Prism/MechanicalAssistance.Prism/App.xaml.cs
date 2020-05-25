using Prism;
using Prism.Ioc;
using MechanicalAssistance.Prism.ViewModels;
using MechanicalAssistance.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.Licensing;
using MechanicalAssistance.Common.Services;
using MechanicalAssistance.Common.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MechanicalAssistance.Prism
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MjUzMjg2QDMxMzgyZTMxMmUzMEw5LzRzTVV3VmNWWlc3SlJHWmpIWTdDTFk4dDRzUFRJeDN2VUFHZ0wvQnc9");

            InitializeComponent();

            await NavigationService.NavigateAsync("/MechanicMasterDetailPage/NavigationPage/ServicePage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IGeolocatorService, GeolocatorService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.Register<IFilesHelper, FilesHelper>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RememberPasswordPage, RememberPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<MechanicMasterDetailPage, MechanicMasterDetailPageViewModel > ();
            containerRegistry.RegisterForNavigation<ServicePage, ServicePageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ServiceTabbedPage, ServiceTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductPage, ProductPageViewModel>();
            containerRegistry.RegisterForNavigation<NewProductPage, NewProductPageViewModel>();
            containerRegistry.RegisterForNavigation<ServiceDetailsPage, ServiceDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<NewRequestServicePage, NewRequestServiceViewModel>();
            containerRegistry.RegisterForNavigation<ServiceRequestsPage, ServiceRequestsPageViewModel>();
            containerRegistry.RegisterForNavigation<ServiceRequestDetailsPage, ServiceRequestDetailsPageViewModel>();
        }
    }
}
