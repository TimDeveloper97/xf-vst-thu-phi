using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThuPhi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MobileShell : Shell
    {
        public MobileShell()
        {
            InitializeComponent();
            
            InitDependencyService();
            InitRoute();
        }

        async void InitDependencyService()
        {
            await CheckAndRequestPermission();
            DependencyService.Register<Services.Temp.SomeService>();
        }

        void InitRoute()
        {
            //Routing.RegisterRoute(nameof(DeviceV30Page), typeof(DeviceV30Page));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public async Task<PermissionStatus> CheckAndRequestPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Sms>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.Sms>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.Sms>();

            return status;
        }
    }
}