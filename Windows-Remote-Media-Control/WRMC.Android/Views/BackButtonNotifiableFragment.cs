using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WRMC.Android.Views {
	public abstract class BackButtonNotifiableFragment : Fragment {
		public abstract void OnBackButton();
	}
}