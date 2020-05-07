using Prism;
using Prism.Ioc;
using MechanicalAssistance.Prism.ViewModels;
using MechanicalAssistance.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.Licensing;

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

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
        }
    }
}
