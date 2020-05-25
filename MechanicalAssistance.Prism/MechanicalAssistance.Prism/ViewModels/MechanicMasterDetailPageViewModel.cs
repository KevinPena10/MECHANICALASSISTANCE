using MechanicalAssistance.Common.Enums;
using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Common.Services;
using MechanicalAssistance.Prism.Helpers;
using MechanicalAssistance.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class MechanicMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static MechanicMasterDetailPageViewModel _instance;
        private UserResponse _user;
        private DelegateCommand _modifyUserCommand;
        private bool _isEnabled;
        private bool _isEnabledTwo;


        public MechanicMasterDetailPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _apiService = apiService;
            _navigationService = navigationService;
            LoadUser();
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool IsEnabledTwo
        {
            get => _isEnabledTwo;
            set => SetProperty(ref _isEnabledTwo, value);
        }


        public string menuImage { get; set; }

        public DelegateCommand ModifyUserCommand => _modifyUserCommand ?? (_modifyUserCommand = new DelegateCommand(ModifyUserAsync));

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private async void ModifyUserAsync()
        {
            await _navigationService.NavigateAsync($"/MechanicMasterDetailPage/NavigationPage/{nameof(ModifyUserPage)}");
        }

        public static MechanicMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }

        public async void ReloadUser()
        {
            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                return;
            }

            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            EmailRequest emailRequest = new EmailRequest
            {
                Email = user.Email,
                CultureInfo = Languages.Culture

            };

            Response response = await _apiService.GetUserByEmail(url, "/api", "/Account/GetUserByEmail", "bearer", token.Token, emailRequest);
            UserResponse userResponse = (UserResponse)response.Result;
            Settings.User = JsonConvert.SerializeObject(userResponse);
            LoadUser();
        }


        private void LoadUser()
        {

            if (Settings.IsLogin)
            {
                IsEnabledTwo = false;
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                IsEnabled = true;

                if (User.UserType == UserType.Mechanic || User.UserType == UserType.Admin) {
                    LoadMenusThree();
                } else {
                    LoadMenus();
                }

            } else {
                IsEnabledTwo = true;
                IsEnabled = false;
                menuImage = "mechanicalImage.png";
                LoadMenusTwo();

            }
        }
        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "gestures",
                    PageName = "ServicePage",
                    Title = Languages.ServiceMenu
                },

                new Menu
                {
                    Icon = "modify" ,
                    PageName = "ModifyUserPage" ,
                    Title = Languages.ModifyMenu
                },
                new Menu
                {
                    Icon = "login",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? Languages.LogoutMenu : Languages.LoginMenu
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }

        private void LoadMenusThree()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "gestures",
                    PageName = "ServicePage",
                    Title = Languages.ServiceMenu
                },

                 new Menu
                {
                    Icon = "it",
                    PageName = "ServiceRequestsPage",
                    Title = Languages.titleRequest
                },
                new Menu
                {
                    Icon = "modify" ,
                    PageName = "ModifyUserPage" ,
                    Title = Languages.ModifyMenu
                },
                new Menu
                {
                    Icon = "login",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? Languages.LogoutMenu : Languages.LoginMenu
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }

        private void LoadMenusTwo()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "gestures",
                    PageName = "ServicePage",
                    Title = Languages.ServiceMenu
                },
                new Menu
                {
                    Icon = "login",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? Languages.LogoutMenu : Languages.LoginMenu
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}
