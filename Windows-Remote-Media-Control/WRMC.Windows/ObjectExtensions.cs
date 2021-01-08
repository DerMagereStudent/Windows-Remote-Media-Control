namespace WRMC.Windows {
	public static class ObjectExtensions {
		public static T TryCastOrDefault<T>(this object obj) => obj is T ? (T)obj : default(T); 
	}
}