using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ThuPhi.Domain;
using ThuPhi.Model.Receive;
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

        public ICommand LoadCollectionCommand => new Command(async () =>
        {
            IsBusy = true;

            try
            {
                var res = await Service.InfoCollection(ParameterToken);
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
            Collections = new ObservableCollection<Form>();
        }

        void OnLoad()
        {
            
        }
        #endregion
    }
}
