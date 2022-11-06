using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Input;
using ThuPhi.Domain;
using ThuPhi.Model.Receive;
using ThuPhi.Model.Send;
using ThuPhi.Pages;
using ThuPhi.Resources.Languages;
using ThuPhi.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ThuPhi.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        #region Property
        private string selectedLanguage, userName, password;
        private bool isSave;
        private ObservableCollection<string> languages;
        private static string _currentLanguage;

        public ObservableCollection<string> Languages { get => languages; set => SetProperty(ref languages, value); }
        public string SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                SetProperty(ref selectedLanguage, value);

                if (_currentLanguage == SelectedLanguage)
                    return;

                var code = "";
                if (SelectedLanguage == "Vietnamese")
                    code = "vi";

                _currentLanguage = selectedLanguage;
                Preferences.Set("language", SelectedLanguage);
                CultureInfo language = new CultureInfo(code);
                Thread.CurrentThread.CurrentUICulture = language;
                LanguageResource.Culture = language;
                Application.Current.MainPage = new MobileShell();
            }
        }

        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public bool IsSave { get => isSave; set => SetProperty(ref isSave, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            OnLoad();
        });

        public ICommand LoginCommand => new Command(async () =>
        {
            IsBusy = true;

            if(string.IsNullOrEmpty(UserName))
            {
                Message.ShortAlert("Username cann't be empty");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                Message.ShortAlert("Password cann't be empty");
                return;
            }

            try
            {
                var res = await Service.Login(UserName, Password); 

                if(res?.Token != null)
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}?{nameof(HomeViewModel.ParameterToken)}={res.Token}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

        });

        #endregion

        public LoginViewModel()
        {
            Init();
        }

        #region Method
        void Init()
        {
            Title = "Login";
            UserName = "0989154248";
            Password = "1";
            Languages = new ObservableCollection<string> { "English", "Vietnamese"};
        }

        void OnLoad()
        {
            SelectedLanguage = Preferences.Get("language", "English");
            IsSave = Preferences.Get("save", false);
        }
        #endregion
    }
}
