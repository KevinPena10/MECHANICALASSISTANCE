using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Common.Services;
using MechanicalAssistance.Prism.Helpers;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MechanicalAssistance.Prism.ViewModels
{
    public class NewProductPageViewModel : ViewModelBase
    {
        public int ServiceId;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private ProductRequest _product;
        private ProductBrandResponse _productBrand;
        private ObservableCollection<ProductBrandResponse> _productBrands;
        private ImageSource _image;
        private MediaFile _file;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changeImageCommand;

        private bool _isRunning;
        private bool _isEnabled;
        public NewProductPageViewModel(INavigationService navigationService, IApiService apiService, IFilesHelper filesHelper) : base(navigationService)
        {
            Title = Languages.titleNewProduct;
            _navigationService = navigationService;
            _apiService = apiService;
            _filesHelper = filesHelper;
            Image = App.Current.Resources["UrlNoImage"].ToString();
            Product = new ProductRequest();
            LoadProductBrandsAsync();
        }

         public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));
         public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

         public ImageSource Image
         {
             get => _image;
             set => SetProperty(ref _image, value);
         }

         public ProductRequest Product
         {
             get => _product;
             set => SetProperty(ref _product, value);
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

         public ProductBrandResponse ProductBrand
         {
             get => _productBrand;
             set => SetProperty(ref _productBrand, value);
         }

         public ObservableCollection<ProductBrandResponse> ProductBrands
         {
             get => _productBrands;
             set => SetProperty(ref _productBrands, value);
         }


         public override void OnNavigatedTo(INavigationParameters parameters)
         {
             base.OnNavigatedTo(parameters);

            ServiceId = parameters.GetValue<int>("newProduct");
            Product.ServiceId = ServiceId;

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

            if (string.IsNullOrEmpty(Product.ProductName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PNameValidation, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Product.Description))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DescriptionValidation, Languages.Accept);
                return false;
            }
            if (string.IsNullOrEmpty(Product.Price.ToString()) || Product.Price <= 0)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceValidation, Languages.Accept);
                return false;
            }

            if (ProductBrand == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.BrandValidation, Languages.Accept);
                return false;
            }

            return true;
        }

        private async void SaveAsync()
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

            Product.Photo = imageArray;
            Product.ServiceId = ServiceId;
            Product.ProductBrandId = ProductBrand.Id;
            Product.CultureInfo = Languages.Culture;

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.NewProductAsync(url, "/api", "/Products", Product, "bearer", token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }
            await App.Current.MainPage.DisplayAlert(Languages.Ok, response.Message, Languages.Accept);
            ProductPageViewModel.GetInstance().LoadProductsAsync();
            await _navigationService.GoBackAsync();

        }



        private async void LoadProductBrandsAsync()
         {
             string url = App.Current.Resources["UrlAPI"].ToString();
             bool connection = await _apiService.CheckConnectionAsync(url);
             if (!connection)
             {

                 await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);

                 return;
             }

             IsRunning = true;
             TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
             Response response = await _apiService.GetListAsync<ProductBrandResponse>(url, "/api", "/ProductBrands", "bearer", token.Token);

             if (!response.IsSuccess)
             {
                 await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                 return;
             }

             List<ProductBrandResponse> list = (List<ProductBrandResponse>)response.Result;
             ProductBrands = new ObservableCollection<ProductBrandResponse>(list.OrderBy(p => p.BrandName));
             IsRunning = false;
         }
     }
  }

