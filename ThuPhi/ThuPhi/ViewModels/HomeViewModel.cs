using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ThuPhi.Domain;
using ThuPhi.Model.Receive;
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
        public string ParameterToken
        {
            get => parameterToken; set
            {
                parameterToken = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterToken, value);
                IsBusy = true;
            }
        }

        public ObservableCollection<Form> Collections { get => collections; set => SetProperty(ref collections, value); }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            OnLoad();
        });

        public ICommand NewFormPopupCommand => new Command(async () =>
        {
            //await NavigationExtensions.ShowPopupAsync<Form>(Shell.Current.Navigation, new NewFormPopup());
            //var result = await NavigationExtensions.ShowPopupAsync<object>(new NewFormPopup());
            //Console.WriteLine(result);
            NavigationExtensions.ShowPopup(Shell.Current.Navigation, new NewFormPopup());
            //var result = await Shell.Current.Navigation.ShowPopupAsync(new NewFormPopup());
            //Console.WriteLine(result);
        });

        public ICommand SelectCollectionCommand => new Command<Form>(async (obj) =>
        {
            obj.Time = new DateTime(obj.Time.Year, obj.Time.Month, obj.Time.Day);
            var json = JsonConvert.SerializeObject(obj);

            await Shell.Current.GoToAsync($"{nameof(FormPage)}?{nameof(FormViewModel.ParameterForm)}={json}");
        });

        public ICommand LoadCollectionCommand => new Command(async () =>
        {
            IsBusy = true;

            try
            {
                var res = await Service.InfoCollection(ParameterToken);
                if (res == null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Collections.Add(new Form
                        {
                            Name = "Công đoàn phí",
                            Content = "CDP",
                            Time = DateTime.Now,
                        });
                    }
                    return;
                }

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
            Collections = new ObservableCollection<Form>();
        }

        void OnLoad()
        {
            
        }
        #endregion
    }
}
