using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading;
using ThuPhi.Domain;
using ThuPhi.Resources.Languages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ThuPhi.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        #region Property
        private string selectedLanguage;
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
        #endregion

        #region Command 

        #endregion

        public LoginViewModel()
        {
            Init();
        }

        #region Method
        void Init()
        {
            Title = "Login";
            Languages = new ObservableCollection<string> { "English", "Vietnamese"};
            SelectedLanguage = Preferences.Get("language", "English");
        }
        #endregion
    }
}
