using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ThuPhi.Domain
{
    class BaseViewModel : BaseBinding
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #region Extend
        
        protected Api Api { get; set; }
        protected async Task TimeoutSession(string message)
        {
            //await MaterialDialog.Instance.SnackbarAsync(message: message,
            //                  msDuration: MaterialSnackbar.DurationLong);
            //await Shell.Current.GoToAsync("//LoginPage");
        }
        #endregion
    }
}
