#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Threading.Tasks;
using UnityEngine;
namespace SaariTech.UI
{
	internal class UIStatusNavigationBarAndroid
	{
		private const int SYSTEM_UI_FLAG_LIGHT_NAVIGATION_BAR = 16;
		private const int SYSTEM_UI_FLAG_LIGHT_STATUS_BAR = 8192;
		private const int SYSTEM_UI_FLAG_LAYOUT_STABLE = 0x00000100;
		private const int WINDOW_FLAG_LAYOUT_NO_LIMITS = 0x00000200;
		private const int WINDOW_FLAG_FULLSCREEN = 0x00000400;
		private const int WINDOW_FLAG_FORCE_NOT_FULLSCREEN = 0x00000800;
		private const int WINDOW_FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS = -2147483648;
		private static int systemUiVisibilityValue;
		private static int flagsValue;
		internal static bool LightModeStatus;
		internal static bool LightModeNavigation;
		internal static bool SimpleMode = false;
		internal delegate void Callback();
		static bool beforeMode = false;
		static Callback setFlagsInThreadCallback;
		internal const int UpdateLaterTime = 50;
		internal const int BeforeUpdateLaterTime = 100;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		static void RuntimeInitializeOnLoadMethod()
		{
			if (BeforeSplashScreen.Enabled)
			{
				LightModeStatus = BeforeSplashScreen.BeforeSplashScreenStatusBarLightMode;
				LightModeNavigation = BeforeSplashScreen.BeforeSplashScreenNavigationBarLightMode;
				beforeMode = true;
				Task.Run(async delegate
				{
					await Task.Delay(BeforeUpdateLaterTime);
					RunOnAndroidUiThread(ClearAndoridColor);
					return 0;
				});
				UpdateDisplay(BeforeSplashScreen.BeforeSplashScreenDisplayStatusBar, BeforeSplashScreen.BeforeSplashScreenDisplayNavigationBar, true);
				UpdateMode();
			}
		}
		internal static void UpdateDisplay(bool DisplayStatusBar, bool DisplayNavigationBar)
		{
			UpdateDisplay(DisplayStatusBar, DisplayNavigationBar, null, false);
		}
		internal static void UpdateDisplay(bool DisplayStatusBar, bool DisplayNavigationBar, bool forceUpdate = false)
		{
			UpdateDisplay(DisplayStatusBar, DisplayNavigationBar, null, forceUpdate);
		}
		internal static void UpdateDisplay(bool DisplayStatusBar, bool DisplayNavigationBar, Callback callback = null, bool forceUpdate = false, bool noDisplayUpdate = false)
		{
			setFlagsInThreadCallback = callback;
			int newFlagsValue = !SimpleMode ? WINDOW_FLAG_LAYOUT_NO_LIMITS : 0;
			if (GetSDKLevel() < 30)
			{
				newFlagsValue |= WINDOW_FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS;
				if (DisplayStatusBar && !DisplayNavigationBar)
				{
					newFlagsValue |= WINDOW_FLAG_FORCE_NOT_FULLSCREEN;
					Screen.fullScreen = true;
				}
				else if (!DisplayStatusBar && !DisplayNavigationBar)
					Screen.fullScreen = true;
				else if (!DisplayStatusBar && DisplayNavigationBar)
				{
					newFlagsValue |= WINDOW_FLAG_FULLSCREEN;
					Screen.fullScreen = false;
				}
				else
					Screen.fullScreen = false;
				if (flagsValue != newFlagsValue || forceUpdate)
				{
					flagsValue = newFlagsValue;
					if (beforeMode)
					{
						Task.Run(async delegate
						{
							await Task.Delay(BeforeUpdateLaterTime);
							RunOnAndroidUiThread(SetFlagsInThread);
							return 0;
						});
						beforeMode = false;
					}
					else
					{
						AndroidSystemBars.updateLaterList.Add(new AndroidSystemBars.UpdateLater
						{
							Action = () =>
							{
								RunOnAndroidUiThread(SetFlagsInThread);
							},
							seconds = (float)UpdateLaterTime / 1000f
						});
					}
				}
			}
			else if (GetSDKLevel() == 30)
			{
				newFlagsValue |= WINDOW_FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS;
				newFlagsValue |= SYSTEM_UI_FLAG_LAYOUT_STABLE;
				Screen.fullScreen = true;
				if (flagsValue != newFlagsValue || forceUpdate)
				{
					flagsValue = newFlagsValue;
					RunOnAndroidUiThread(SetFlagsInThread);
				}
				if (!noDisplayUpdate)
				{
					if (beforeMode)
					{
						Task.Run(async delegate
						{
							await Task.Delay(BeforeUpdateLaterTime);
							RunOnAndroidUiThread(() => { UpdateStatusBar(DisplayStatusBar ? "Show" : "Hide"); });
							RunOnAndroidUiThread(() => { UpdateNavigationBar(DisplayNavigationBar ? "Show" : "Hide"); });
							return 0;
						});
						beforeMode = false;
					}
					else
					{
						AndroidSystemBars.updateLaterList.Add(new AndroidSystemBars.UpdateLater
						{
							Action = () =>
							{
								RunOnAndroidUiThread(() => { UpdateStatusBar(DisplayStatusBar ? "Show" : "Hide"); });
								RunOnAndroidUiThread(() => { UpdateNavigationBar(DisplayNavigationBar ? "Show" : "Hide"); });
							},
							seconds = (float)UpdateLaterTime / 1000f
						});
					}
				}
			}
			else
			{
				if(!Screen.fullScreen)
					Screen.fullScreen = true;

				if (SimpleMode)
					newFlagsValue |= WINDOW_FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS;

				if (flagsValue != newFlagsValue || forceUpdate)
				{
					flagsValue = newFlagsValue;
					RunOnAndroidUiThread(SetFlagsInThread);
				}
				if (!noDisplayUpdate)
				{
					if (beforeMode)
					{
						Task.Run(async delegate
						{
							await Task.Delay(BeforeUpdateLaterTime);
							RunOnAndroidUiThread(() => { UpdateStatusBar(DisplayStatusBar ? "Show" : "Hide"); });
							RunOnAndroidUiThread(() => { UpdateNavigationBar(DisplayNavigationBar ? "Show" : "Hide"); });
							return 0;
						});
						beforeMode = false;
					}
					else
					{
						AndroidSystemBars.updateLaterList.Add(new AndroidSystemBars.UpdateLater
						{
							Action = () =>
							{
								RunOnAndroidUiThread(() => { UpdateStatusBar(DisplayStatusBar ? "Show" : "Hide"); });
								RunOnAndroidUiThread(() => { UpdateNavigationBar(DisplayNavigationBar ? "Show" : "Hide"); });
							},
							seconds = (float)UpdateLaterTime / 1000f
						});
					}
				}
			}
		}
		internal static int GetSDKLevel()
		{
			IntPtr clazz = AndroidJNI.FindClass("android/os/Build$VERSION");
			IntPtr fieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
			int sdkLevel = AndroidJNI.GetStaticIntField(clazz, fieldID);
			return sdkLevel;
		}
		internal static void UpdateMode()
		{
			int newSystemUiVisibilityValue = 0;
			if (LightModeStatus) newSystemUiVisibilityValue |= SYSTEM_UI_FLAG_LIGHT_STATUS_BAR;
			if (LightModeNavigation) newSystemUiVisibilityValue |= SYSTEM_UI_FLAG_LIGHT_NAVIGATION_BAR;
			if (systemUiVisibilityValue != newSystemUiVisibilityValue)
			{
				systemUiVisibilityValue = newSystemUiVisibilityValue;
				RunOnAndroidUiThread(SetSystemUiVisibilityInThread);
			}
			RunOnAndroidUiThread(UpdateColor);
		}
		private static void UpdateColor()
		{
			if (GetSDKLevel() == 30 || SimpleMode)
				SetAndorid11Color();
		}
		internal static void RunOnAndroidUiThread(Action target)
		{
			using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					activity.Call("runOnUiThread", new AndroidJavaRunnable(target));
				}
			}
		}
		private static void SetSystemUiVisibilityInThread()
		{
			using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (var window = activity.Call<AndroidJavaObject>("getWindow"))
					{
						using (var view = window.Call<AndroidJavaObject>("getDecorView"))
						{
							view.Call("setSystemUiVisibility", systemUiVisibilityValue);
						}
					}
				}
			}
		}
		private static void SetFlagsInThread()
		{
			using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (var window = activity.Call<AndroidJavaObject>("getWindow"))
					{
						window.Call("setFlags", flagsValue, -1);
						if (setFlagsInThreadCallback != null)
							setFlagsInThreadCallback();
						setFlagsInThreadCallback = null;
					}
				}
			}
		}
		private static void SetAndorid11Color()
		{
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow"))
					{
						Color c = MobileStatusNavigationBar.Instance.navigationBarColor;
						window.Call("setNavigationBarColor", ConvertColor(new Color(c.r, c.g, c.b)));
						window.Call("setStatusBarColor", 0x00000000);
					}
				}
			}
		}
		private static void SetAndoridColor()
		{
			using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (var window = activity.Call<AndroidJavaObject>("getWindow"))
					{
						Color s = MobileStatusNavigationBar.Instance.statusBarColor;
						window.Call("setStatusBarColor", ConvertColor(new Color(s.r, s.g, s.b)));
						Color n = MobileStatusNavigationBar.Instance.navigationBarColor;
						window.Call("setNavigationBarColor", ConvertColor(new Color(n.r, n.g, n.b)));
					}
				}
			}
		}

		private static void ClearAndoridColor()
		{
			using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (var window = activity.Call<AndroidJavaObject>("getWindow"))
					{
						window.Call("setNavigationBarColor", 0x00000000);
						window.Call("setStatusBarColor", 0x00000000);
					}
				}
			}
		}
		private static int ConvertColor(Color color)
		{
			return (Mathf.RoundToInt(color.a * 255) << 24) | (Mathf.RoundToInt(color.r * 255) << 16) | (Mathf.RoundToInt(color.g * 255) << 8) | Mathf.RoundToInt(color.b * 255);
		}
		private static void UpdateStatusBar(string code)
		{
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow"))
					{
						using (AndroidJavaClass statusNavigationBar = new AndroidJavaClass("com.saaritech.uistatusnavigationbar.UIStatusNavigationBar"))
						{
							statusNavigationBar.CallStatic(code + "StatusBar", window);
						}
					}
				}
			}
		}
		private static void UpdateNavigationBar(string code)
		{
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow");
			AndroidJavaClass statusNavigationBar = new AndroidJavaClass("com.saaritech.uistatusnavigationbar.UIStatusNavigationBar");
			AndroidJavaObject view = window.Call<AndroidJavaObject>("getDecorView");

			statusNavigationBar.CallStatic(code + "NavigationBar", window);
			view.Call("setSystemUiVisibility", systemUiVisibilityValue);
			window.Call("setFlags", flagsValue, -1);
		}
	}
}
#endif