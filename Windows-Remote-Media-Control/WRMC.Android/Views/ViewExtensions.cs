using Android.App;
using Android.Content;
using Android.Views;

namespace WRMC.Android.Views {
	public static class ViewExtensions {
        public static Activity GetActivity(this View view) {
            Context context = view.Context;

            while (context is ContextWrapper) {
                if (context is Activity)
                    return (Activity)context;

                context = ((ContextWrapper)context).BaseContext;
            }

            return null;
        }
    }
}