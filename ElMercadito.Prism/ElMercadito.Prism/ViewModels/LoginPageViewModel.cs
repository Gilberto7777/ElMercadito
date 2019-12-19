using ElMercadito.Common.Models;
using ElMercadito.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElMercadito.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;

        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService ) : base(navigationService)
        {
            _apiService = apiService;
            Title = "Login";
            IsEnabled = true;

        //: TODO Eliminar las lineas
        Email = "jzuluaga55@hotmail.com";
        Password = "123456";

            
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public string Email { get; set; }

        public string Password 
        {
            // obtener password
            get => _password;
            //actualiza la interfaz de usuario
            set => SetProperty(ref _password, value);
        
        }
        public bool IsRunning
        {
            // obtener 
            get => _isRunning;
            //actualiza la interfaz de usuario
            set => SetProperty(ref _isRunning, value);

        }
        public bool IsEnabled
        {
            // obtener 
            get => _isEnabled;
            //actualiza la interfaz de usuario
            set => SetProperty(ref _isEnabled, value);
        }
        private async void Login()
        {
            if(string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Enter an email.", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Enter an password.", "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection.", "Accept");
                return;
            }


            var request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };


            //var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);


            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "User or password incorrect.", "Accept");
                Password = string.Empty;
                return;
            }

            var token = (TokenResponse)response.Result;

            await App.Current.MainPage.DisplayAlert("Ok", "Muy Bien", "Accept");

        }
    }
}
