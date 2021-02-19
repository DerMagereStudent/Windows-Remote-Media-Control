using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Runtime;

using Xamarin.Essentials;

using System.Linq;

using WRMC.Android.Views;
using WRMC.Android.Networking;

namespace WRMC.Android {
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity {
        public const int READ_WRITE_STORAGE_REQUEST_CODE = 0x00000001;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            this.SetContentView(Resource.Layout.activity_main);
            this.RequestedOrientation = ScreenOrientation.Portrait;

            this.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.main_fragment_container, new HomeFragment()).Commit();
        }

		protected override void OnResume() {
			base.OnResume();

            if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.ReadExternalStorage) != Permission.Granted || ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted) {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M) {
                    if (!this.ShouldShowRequestPermissionRationale(Manifest.Permission.ReadExternalStorage) || !this.ShouldShowRequestPermissionRationale(Manifest.Permission.WriteExternalStorage)) {
                        ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, READ_WRITE_STORAGE_REQUEST_CODE);
                    }
				}
			}
		}

		protected override void OnDestroy() {
            ConnectionManager.CloseConnection(DeviceInformation.GetClientDevice(this.ApplicationContext));
            base.OnDestroy();
		}

		public void ChangeFragment(global::Android.Support.V4.App.Fragment fragment) {
            global::Android.Support.V4.App.FragmentTransaction transaction = this.SupportFragmentManager.BeginTransaction();
            transaction.SetCustomAnimations(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left, Resource.Animation.enter_from_left, Resource.Animation.exit_to_right);
            transaction.Replace(Resource.Id.main_fragment_container, fragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            if (requestCode == READ_WRITE_STORAGE_REQUEST_CODE) {
                if (!grantResults.All(p => p == Permission.Granted)) {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, READ_WRITE_STORAGE_REQUEST_CODE);
                }
            }
            
            //Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

		public override void OnBackPressed() {
            global::Android.Support.V4.App.Fragment fragment = this.SupportFragmentManager.FindFragmentById(Resource.Id.main_fragment_container);

            if (fragment is BackButtonNotifiableFragment)
                if ((fragment as BackButtonNotifiableFragment).OnBackButton())
                        return;

            base.OnBackPressed();
		}
	}
}