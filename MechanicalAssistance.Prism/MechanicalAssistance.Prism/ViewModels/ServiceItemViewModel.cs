﻿using MechanicalAssistance.Common.Helpers;
using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace MechanicalAssistance.Prism.ViewModels
{


    public class ServiceItemViewModel : ServiceResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectCommand;

        public ServiceItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectCommand => _selectCommand ??
          (_selectCommand = new DelegateCommand(SelectAsync));

        private async void SelectAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "service", this }
            };

            Settings.Service = JsonConvert.SerializeObject(this);

            await _navigationService.NavigateAsync(nameof(ServiceTabbedPage), parameters);
        }
    }
}
