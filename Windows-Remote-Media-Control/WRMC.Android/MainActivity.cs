using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

using Xamarin.Essentials;

using WRMC.Android.Views;
using Android.Views;

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

		public override bool OnCreateOptionsMenu(IMenu menu) {
            this.MenuInflater.Inflate(Resource.Menu.home_toolbar_menu, menu);
            return true;
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}