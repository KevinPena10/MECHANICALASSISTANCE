using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Common.Services;
using MechanicalAssistance.Prism.Helpers;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class NewRequestServiceViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private RequestServiceRequest _request;
        private ImageSource _image;
        private MediaFile _file;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changeImageCommand;
        private bool _isRunning;
        private bool _isEnabled;

        public NewRequestServiceViewModel(INavigationService navigationService, IApiService apiService, IFilesHelper filesHelper) : base(navigationService)
        {

            _navigationService = navigationService;
            _apiService = apiService;
            _filesHelper = filesHelper;
            Image = App.Current.Resources["AddImage"].ToString();
            Title = Languages.txtRequest;
            CurrentTime = DateTime.Now.TimeOfDay;
            Date = DateTime.Now.Date;
            Request = new RequestServiceRequest();
        }

        public TimeSpan CurrentTime { get; set; }
        public DateTime Date { get; set; }
        public DelegateCommand RequestCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(RequestAsync));

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));
        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public RequestServiceRequest Request
        {
            get => _request;
            set => SetProperty(ref _request, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        private async void RequestAsync()
        {
            var isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);

                return;
            }

            byte[] imageArray = null;

            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());

            }

            ServiceResponse service = JsonConvert.DeserializeObject<ServiceResponse>(Settings.Service);
            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            Request.DateAndTime = Date + CurrentTime;
            Request.Photo = imageArray;
            Request.Status = Languages.statusFirst;
            Request.ServiceId = service.Id;
            Request.UserId = Guid.Parse(user.Id);
            Request.CultureInfo = Languages.Culture;


            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.NewRequestAsync(url, "/api", "/RequestService/NewRequest", Request, "bearer", token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, response.Message, Languages.Accept);
            await _navigationService.NavigateAsync("/MechanicMasterDetailPage/NavigationPage/ServicePage");

        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                 Languages.PictureMessage,
               Languages.Cancel,
               null,
               Languages.Gallery,
               Languages.Camera);

            if (source == Languages.Cancel)
            {
                _file = null;
                return;
            }

            try
            {
                if (source == Languages.Camera)
                {
                    _file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    _file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            catch (System.Exception)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Permissions, Languages.Accept);
                return;
            }


            if (_file != null)
            {
                Image = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = _file.GetStream();
                    return stream;
                });
            }
        }


        private async Task<bool> ValidateDataAsync()
        {

            if (string.IsNullOrEmpty(Request.Observation))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PlaceHolderObservation, Languages.Accept);
                return false;
            }

            return true;
        }

    }
}
