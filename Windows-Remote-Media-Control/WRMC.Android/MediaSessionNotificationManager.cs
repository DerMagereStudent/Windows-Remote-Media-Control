using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Media;
using Android.Support.V4.Media.Session;

using System.Collections.Generic;

using WRMC.Android.Networking;
using WRMC.Core.Models;
using WRMC.Core.Networking;

namespace WRMC.Android {
	public static class MediaSessionNotificationManager {
		private const string CHANNEL_ID = "WRMC.Android.MediaSessionNotificationChannel";
		private const int NOTIFICATION_ID = 18517;

		private static List<MediaSession> sessions;
		private static MediaSession currentSession;
		private static int currentIndex;

		private static Context context;
		private static Bitmap currentThumbnail;
		private static MediaSessionCompat mediaSession;

		public static void Initialize(Context context) {
			MediaSessionNotificationManager.context = context;

			NotificationChannel channel = new NotificationChannel(CHANNEL_ID, new Java.Lang.String("WRMC-Media-Sessions"), NotificationImportance.Low);
			(context.GetSystemService(Context.NotificationService) as NotificationManager).CreateNotificationChannel(channel);

			mediaSession = new MediaSessionCompat(context, "MediaSession");
			MediaMetadataCompat metadata = new MediaMetadataCompat.Builder()
				.PutLong(MediaMetadataCompat.MetadataKeyDuration, -1L)
				.Build();

			mediaSession.SetMetadata(metadata);
			mediaSession.Active = true;

			ConnectionManager.OnConnectSuccess += ConnectionManager_OnConnectSuccess;
		}

		private static void ConnectionManager_OnConnectSuccess(object sender, System.EventArgs e) {
			ConnectionManager.OnConnectSuccess -= ConnectionManager_OnConnectSuccess;
			ConnectionManager.OnMediaSessionsReceived += ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged += ConnectionManager_OnMediaSessionChanged;
			ConnectionManager.SendRequest(new Request() {
				Method = Request.Type.GetMediaSessions,
				Body = new AuthenticatedMessageBody() {
					ClientDevice = DeviceInformation.GetClientDevice(context.ApplicationContext)
				}
			});
			ConnectionManager.OnConnectionClosed += ConnectionManager_OnConnectionClosed;
		}

		private static void ConnectionManager_OnConnectionClosed(object sender, System.EventArgs e) {
			currentSession = null;
			currentIndex = 0;
			ConnectionManager.OnConnectionClosed -= ConnectionManager_OnConnectionClosed;
			ConnectionManager.OnMediaSessionsReceived -= ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged -= ConnectionManager_OnMediaSessionChanged;
			(context.GetSystemService(Context.NotificationService) as NotificationManager).Cancel(NOTIFICATION_ID);
			ConnectionManager.OnConnectSuccess += ConnectionManager_OnConnectSuccess;
		}

		private static void ConnectionManager_OnMediaSessionsReceived(object sender, Core.EventArgs<System.Collections.Generic.List<MediaSession>> e) {
			sessions = e.Data;

			if (sessions.Count == 0) {
				currentSession = null;
				currentIndex = 0;
				(context.GetSystemService(Context.NotificationService) as NotificationManager).Cancel(NOTIFICATION_ID);
				return;
			}

			if (sessions.Contains(currentSession)) {
				currentIndex = sessions.IndexOf(currentSession);
				return;
			}

			UpdateDisplayedSession(sessions[0]);
			currentIndex = 0;
			DisplayNotification();
		}

		private static void ConnectionManager_OnMediaSessionChanged(object sender, Core.EventArgs<MediaSession> e) {
			UpdateDisplayedSession(e.Data);
			DisplayNotification();
		}

		private static void ConnectionManager_OnThumbnailReceived(object sender, Core.EventArgs<byte[]> e) {
			ConnectionManager.OnThumbnailReceived -= ConnectionManager_OnThumbnailReceived;
			currentThumbnail = BitmapFactory.DecodeByteArray(e.Data, 0, e.Data.Length);
			DisplayNotification();
		}

		public static void UpdateDisplayedSession(MediaSession session) {
			if (currentSession == null && session == null)
				return;

			currentSession = session;

			if (session == null) {
				(context.GetSystemService(Context.NotificationService) as NotificationManager).Cancel(NOTIFICATION_ID);
				return;
			}

			DisplayNotification();

			ConnectionManager.OnThumbnailReceived += ConnectionManager_OnThumbnailReceived;
			ConnectionManager.SendRequest(new Request() {
				Method = Request.Type.GetThumbnail,
				Body = new MediaSessionMessageBody() {
					ClientDevice = DeviceInformation.GetClientDevice(context.ApplicationContext),
					MediaSession = currentSession
				}
			});
		}

		public static void InvokePlayPause() {
			if (currentSession != null)
				ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
					Method = WRMC.Core.Networking.Message.Type.PlayPause,
					Body = new MediaSessionMessageBody() {
						MediaSession = currentSession,
						ClientDevice = DeviceInformation.GetClientDevice(context.ApplicationContext)
					}
				});
		}

		public static void InvokeSkipNext() {
			if (currentSession != null)
				ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
					Method = WRMC.Core.Networking.Message.Type.NextMedia,
					Body = new MediaSessionMessageBody() {
						MediaSession = currentSession,
						ClientDevice = DeviceInformation.GetClientDevice(context.ApplicationContext)
					}
				});
		}

		public static void InvokeSkipPrevious() {
			if (currentSession != null)
				ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
					Method = WRMC.Core.Networking.Message.Type.PreviousMedia,
					Body = new MediaSessionMessageBody() {
						MediaSession = currentSession,
						ClientDevice = DeviceInformation.GetClientDevice(context.ApplicationContext)
					}
				});
		}

		public static void InvokeNextSession() {
			currentIndex++;
			currentIndex = currentIndex >= sessions.Count ? 0 : currentIndex;

			UpdateDisplayedSession(sessions[currentIndex]);
		}

		public static void InvokePreviousSession() {
			currentIndex--;
			currentIndex = currentIndex < 0 ? sessions.Count - 1 : currentIndex;

			UpdateDisplayedSession(sessions[currentIndex]);
		}

		private static void DisplayNotification() {
			Notification notification = new global::Android.Support.V4.App.NotificationCompat.Builder(context, CHANNEL_ID)
				.SetSmallIcon(Resource.Drawable.play)
				.SetContentTitle(currentSession.Title)
				.SetContentText(currentSession.Artist)
				.SetLargeIcon(currentThumbnail)
				.AddAction(Resource.Drawable.chevron_left, "Previous Session", PendingIntent.GetService(
					context, 0, 
					new Intent(context, typeof(MediaPreviousSessionService)),
					PendingIntentFlags.CancelCurrent))
				.AddAction(Resource.Drawable.skip_previous, "Skip Previous", PendingIntent.GetService(
					context, 0,
					new Intent(context, typeof(MediaSkipPreviousService)),
					PendingIntentFlags.CancelCurrent))
				.AddAction(currentSession.State == MediaSession.PlaybackState.Playing ? Resource.Drawable.pause : Resource.Drawable.play, "Play Pause", PendingIntent.GetService(
					context, 0,
					new Intent(context, typeof(MediaPlayPauseService)),
					PendingIntentFlags.CancelCurrent))
				.AddAction(Resource.Drawable.skip_next, "Skip Next", PendingIntent.GetService(
					context, 0,
					new Intent(context, typeof(MediaSkipNextService)),
					PendingIntentFlags.CancelCurrent))
				.AddAction(Resource.Drawable.chevron_right, "Next Session", PendingIntent.GetService(
					context, 0,
					new Intent(context, typeof(MediaNextSessionService)),
					PendingIntentFlags.CancelCurrent))
				.SetStyle(new global::Android.Support.V4.Media.App.NotificationCompat.MediaStyle()
					.SetShowActionsInCompactView(1, 2, 3)
					.SetMediaSession(mediaSession.SessionToken))
				.SetPriority(global::Android.Support.V4.App.NotificationCompat.PriorityHigh)
				.Build();

			(context.GetSystemService(Context.NotificationService) as NotificationManager).Notify(NOTIFICATION_ID, notification);
		}

		public abstract class MediaService : Service {
			public enum Type {
				PlayPause,
				SkipNext,
				SkipPrevious,
				NextSession,
				PreviousSession
			}

			public abstract Type Action { get; }

			[return: GeneratedEnum]
			public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId) {
				switch(this.Action) {
					case Type.PlayPause:
						MediaSessionNotificationManager.InvokePlayPause();
						break;
					case Type.SkipNext:
						MediaSessionNotificationManager.InvokeSkipNext();
						break;
					case Type.SkipPrevious:
						MediaSessionNotificationManager.InvokeSkipPrevious();
						break;
					case Type.NextSession:
						MediaSessionNotificationManager.InvokeNextSession();
						break;
					case Type.PreviousSession:
						MediaSessionNotificationManager.InvokePreviousSession();
						break;
				}

				this.StopService(new Intent(MediaSessionNotificationManager.context, this.GetType()));
				//this.StopSelf();
				return base.OnStartCommand(intent, flags, startId);
			}

			public override IBinder OnBind(Intent intent) {
				return null;
			}
		}

		[Service]
		public class MediaPlayPauseService : MediaService {
			public override Type Action => Type.PlayPause;
		}

		[Service]
		public class MediaSkipNextService : MediaService {
			public override Type Action => Type.SkipNext;
		}

		[Service]
		public class MediaSkipPreviousService : MediaService {
			public override Type Action => Type.SkipPrevious;
		}

		[Service]
		public class MediaNextSessionService : MediaService {
			public override Type Action => Type.NextSession;
		}

		[Service]
		public class MediaPreviousSessionService : MediaService {
			public override Type Action => Type.PreviousSession;
		}
	}
}