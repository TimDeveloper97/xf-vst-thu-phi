using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.App;
using Android;
using Java.Interop;

namespace ThuPhi.Droid
{
    [Activity(Label = "Collection", Icon = "@mipmap/pay", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            if (!(CheckPermissionGranted(Manifest.Permission.BroadcastSms) &&
                    CheckPermissionGranted(Manifest.Permission.ReadSms) &&
                    CheckPermissionGranted(Manifest.Permission.ReceiveSms)))
            {
                RequestSmsPermission();
            }

            #region Style Init
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            //CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Plugin.MaterialDesignControls.Android.Renderer.Init();
            //XF.Material.Droid.Material.Init(this, savedInstanceState);
            #endregion

            LoadApplication(new App());
        }

        //[Export]
        public bool CheckPermissionGranted(string Permissions)
        {
            // Check if the permission is already available.
            if (ActivityCompat.CheckSelfPermission(this, Permissions) != Permission.Granted)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void RequestSmsPermission()
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.BroadcastSms))
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.BroadcastSms }, 1);
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadSms }, 1);
            }

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ReadSms))
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadSms }, 2);
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadSms }, 2);
            }

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ReceiveSms))
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReceiveSms }, 3);
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReceiveSms }, 3);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}