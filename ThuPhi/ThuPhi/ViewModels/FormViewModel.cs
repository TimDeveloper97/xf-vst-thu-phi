using Newtonsoft.Json;
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
    [QueryProperty(nameof(ParameterForm), nameof(ParameterForm))]
    class FormViewModel : BaseViewModel
    {
        #region Property
        private Form model;
        private string parameterForm;
        private ObservableCollection<Info> users;

        public string ParameterForm
        {
            get => parameterForm; set
            {
                parameterForm = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterForm, value);
                IsBusy = true;
            }
        }
        public Form Model { get => model; set => SetProperty(ref model, value); }
        public ObservableCollection<Info> Users { get => users; set => SetProperty(ref users, value); }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            OnLoad();
        });

        public ICommand BackCommand => new Command(async () => await Shell.Current.GoToAsync($".."));

        public ICommand HomeCommand => new Command(async () => await Shell.Current.Navigation.PopToRootAsync());

        public ICommand LoadUserCommand => new Command(async () =>
        {
            IsBusy = true;

            try
            {
                //var res = await Service.InfoCollection(Token);
                //if (res == null) return;

                Users?.Clear();
                for (int i = 0; i < 5; i++)
                {
                    Users.Add(new Info
                    {
                        Name = "Đinh Duy Anh",
                        Pay = "1000000000",
                    });
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

        public FormViewModel()
        {
            Init();
        }

        #region Method
        void Init()
        {
            Title = "Form";
            Users = new ObservableCollection<Info>();
        }

        void OnLoad()
        {
            Model = JsonConvert.DeserializeObject<Form>(ParameterForm);
        }
        #endregion

    }
}
