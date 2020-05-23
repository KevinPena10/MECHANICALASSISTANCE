using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Services;
using MechanicalAssistance.Prism.Helpers;
using Prism.Navigation;
using System;
using Xamarin.Forms;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class NewRequestServiceViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private ImageSource _image;

        public NewRequestServiceViewModel(INavigationService navigationService, IApiService apiService, IFilesHelper filesHelper) : base(navigationService)
        {

            _navigationService = navigationService;
            _apiService = apiService;
            _filesHelper = filesHelper;
            Image = App.Current.Resources["AddImage"].ToString();
            Title = Languages.txtRequest;
            CurrentTime = DateTime.Now.TimeOfDay;
        }

        public TimeSpan CurrentTime { get; set; }
        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
    }
}
