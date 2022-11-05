using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public string ParameterForm
        {
            get => parameterForm; set
            {
                parameterForm = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterForm, value);
            }
        }
        public Form Model { get => model; set => SetProperty(ref model, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {
            OnLoad();
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
        }

        void OnLoad()
        {
            Model = JsonConvert.DeserializeObject<Form>(ParameterForm);
        }
        #endregion

    }
}
