using Android.Support.V4.App;
using Android.Support.V7.App;

namespace WRMC.Android.Views {
	/// <summary>
	/// A class that gets notified when the android back button was pressed.
	/// </summary>
	public abstract class BackButtonNotifiableFragment : Fragment {
		/// <summary>
		/// Called when the android back button was pressed.
		/// </summary>
		/// <returns>If true the fragment handled the event and the <see cref="AppCompatActivity.OnBackPressed"/> should not be called.</returns>
		public abstract bool OnBackButton();
	}
}