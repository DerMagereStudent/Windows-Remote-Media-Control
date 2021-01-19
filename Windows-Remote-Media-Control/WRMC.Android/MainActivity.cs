using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;

using Xamarin.Essentials;

using WRMC.Android.Views;
using WRMC.Android.Networking;

namespace WRMC.Android {
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            this.SetContentView(Resource.Layout.activity_main);
            this.RequestedOrientation = ScreenOrientation.Portrait;

            this.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.main_fragment_container, new HomeFragment()).Commit();
        }

		protected override void OnStop() {
            ConnectionManager.CloseConnection(DeviceInformation.GetClientDevice(this.ApplicationContext));
            base.OnStop();
		}

		public void ChangeFragment(global::Android.Support.V4.App.Fragment fragment) {
            global::Android.Support.V4.App.FragmentTransaction transaction = this.SupportFragmentManager.BeginTransaction();
            transaction.SetCustomAnimations(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left, Resource.Animation.enter_from_left, Resource.Animation.exit_to_right);
            transaction.Replace(Resource.Id.main_fragment_container, fragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}