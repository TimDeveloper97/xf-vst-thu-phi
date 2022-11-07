using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ThuPhi.Domain;
using ThuPhi.Model.Receive;
using ThuPhi.Model.Send;
using ThuPhi.Pages;
using ThuPhi.Pages.Popup;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace ThuPhi.ViewModels
{
    [QueryProperty(nameof(ParameterToken), nameof(ParameterToken))]
    class HomeViewModel : BaseViewModel
    {
        #region Property
        private ObservableCollection<Form> collections;
        private string parameterToken;
        private bool isLoading;

        public string ParameterToken
        {
            get => parameterToken; set
            {
                parameterToken = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterToken, value);

                if (!string.IsNullOrEmpty(parameterToken))
                    Token = parameterToken;
                IsBusy = true;
            }
        }

        public ObservableCollection<Form> Collections { get => collections; set => SetProperty(ref collections, value); }
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() => OnLoad());

        public ICommand LogoutCommand => new Command(async () => await Shell.Current.GoToAsync("//LoginPage"));

        public ICommand NewFormPopupCommand => new Command(async () =>
        {
            var x = await Shell.Current.ShowPopupAsync(new NewFormPopup(new FormCode()));
        });

        public ICommand SelectCollectionCommand => new Command<Form>(async (obj) =>
        {
            try
            {
                IsLoading = true;
                obj.Time = new DateTime(obj.Time.Year, obj.Time.Month, obj.Time.Day);
                var json = JsonConvert.SerializeObject(obj);

                await Shell.Current.GoToAsync($"{nameof(FormPage)}?{nameof(FormViewModel.ParameterForm)}={json}");
            }
            catch (Exception) { }
            finally
            {
                IsLoading = false;
            }
        });

        public ICommand LoadCollectionCommand => new Command(async () =>
        {
            IsBusy = true;

            try
            {
                var res = await Service.InfoCollection(Token);
                if (res == null) return;

                Collections?.Clear();

                foreach (var item in res)
                {
                    Collections.Add(item);
                }
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

        public HomeViewModel()
        {
            Init();
        }

        #region Method
        void Init()
        {
            Title = "Home";
            IsLoading = false;
            Collections = new ObservableCollection<Form>();
        }

        void OnLoad()
        {

        }
        #endregion
    }
}
