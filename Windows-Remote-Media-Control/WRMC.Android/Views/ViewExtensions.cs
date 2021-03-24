using Android.App;
using Android.Content;
using Android.Views;

namespace WRMC.Android.Views {
    /// <summary>
    /// Class containing extension methods for the class <see cref="View"/>
    /// </summary>
	public static class ViewExtensions {
        /// <summary>
        /// Gets the activity of the view.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
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