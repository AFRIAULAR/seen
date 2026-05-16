#if UNITY_IOS && !UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
namespace SaariTech.UI
{
	internal class IOSSystemBars : SystemBarsBase
	{
		static MobileStatusNavigationBar instance
		{
			get
			{
				return MobileStatusNavigationBar.Instance;
			}
		}
		internal static float StatusBarHeightValue;
		internal static float StatusBarHeight
		{ 
			get
			{
				return StatusBarHeightValue;
			} 
			private set
			{
				StatusBarHeightValue = value;
				instance.UpdateStatusBarRect(
					0f,
					0f,
					instance.canvasRectTransform.sizeDelta.y - StatusBarHeightValue,
					0f
				);
				instance.UpdateMainRect(
					StatusBarHeightValue,
					0f,
					float.NaN,
					0f
				);
			} 
		}
		internal static float StatusBarHeight_STATIC { get; private set; }
		static float NavigationBarHeightValue;
		internal static float NavigationBarHeight 
		{
			get
			{
				return NavigationBarHeightValue;
			} 
			private set
			{
				NavigationBarHeightValue = value;
				instance.UpdateNavigationBarRect(
					instance.canvasRectTransform.sizeDelta.y - NavigationBarHeight,
					0f,
					0f,
					0f
				);
				instance.UpdateMainRect(
					float.NaN,
					0f,
					NavigationBarHeight,
					0f
				);
			}
		}
		internal static float NavigationBarHeight_STATIC { get; private set; }
		internal static List<UpdateLater> updateLaterList = new List<UpdateLater>();
		internal class UpdateLater
		{
			internal DelegateAction Action;
			internal float seconds;
		}
		internal override void OnScreenOrientationChange()
		{
			instance.statusBar.gameObject.SetActive(false);
			instance.navigationBar.gameObject.SetActive(false);
			updateLaterList.Add(new UpdateLater
			{
				Action = () =>
				{
					instance.statusBar.gameObject.SetActive(true);
					instance.navigationBar.gameObject.SetActive(true);
					Refresh(true);
				},
				seconds = 0f
			});
		}
		static float RAW_STATUSBAR_HEIGHT = 0f;
		static float RAW_HOMEBAR_HEIGHT = 0f;
		internal override void Refresh(bool force = false)
		{
			float scale = Screen.height / instance.canvasRectTransform.sizeDelta.y;
			StatusBarHeight_STATIC = RAW_STATUSBAR_HEIGHT / scale;
			NavigationBarHeight_STATIC = RAW_HOMEBAR_HEIGHT / scale;
			UpdateNavigationBar(force);
			UpdateStatusBar(force);
		}
		internal bool inited = false;
		internal override void Init(bool force = false)
		{
			if (inited && !force)
				return;
			RAW_STATUSBAR_HEIGHT = GetStatusBarHeight();
			RAW_HOMEBAR_HEIGHT = GetNavBarHeight();
#if !NODEBUG || UNITY_EDITOR
			if (instance.debugMode)
			{
				if (inited)
					Debug.Log("MSANB: Reinitialized");
				else
					Debug.Log("MSANB: Initialized");
			}
#endif
			if (inited)
				return;
			if (instance.startLightMode)
				instance.SetStatusLightMode();
			else
				instance.SetStatusDarkMode();
			if (instance.startLightMode)
				instance.SetNavigationLightMode();
			else
				instance.SetNavigationDarkMode();
			instance.UpdateColor();
			Refresh();
			inited = true;
			MobileStatusNavigationBar.IsReadyStatusBar = true;
			MobileStatusNavigationBar.IsReadyNavigationBar = true;
		}
		internal override bool Hide(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((instance.displayStatusBar && MobileStatusNavigationBar.IsReadyStatusBar) || (instance.displayNavigtionBar && MobileStatusNavigationBar.IsReadyNavigationBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Hide Bars");
#endif
				instance.displayStatusBar = false;
				instance.displayNavigtionBar = false;
				MobileStatusNavigationBar.IsReadyStatusBar = false;
				MobileStatusNavigationBar.IsReadyNavigationBar = false;
				if (useColorTransition)
					instance.SetColor(toColorTransition);
				UpdateStatusBar(force);
				UpdateNavigationBar(force);
				if (completeCallback != null)
					completeCallback();
				return true;
			}
			return false;
		}
		internal override bool HideStatus(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((instance.displayStatusBar && MobileStatusNavigationBar.IsReadyStatusBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Hide Status Bar");
#endif
				instance.displayStatusBar = false;
				MobileStatusNavigationBar.IsReadyStatusBar = false;
				if (useColorTransition)
					instance.SetStatusColor(toColorTransition);
				UpdateStatusBar(force);
				if (completeCallback != null)
					completeCallback();
				return true;
			}
			return false;
		}
		internal override bool HideNavigation(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((instance.displayNavigtionBar && MobileStatusNavigationBar.IsReadyNavigationBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Hide Navigation Bar");
#endif
				instance.displayNavigtionBar = false;
				MobileStatusNavigationBar.IsReadyNavigationBar = false;
				if (useColorTransition)
					instance.SetNavigationColor(toColorTransition);
				UpdateNavigationBar(force);
				if (completeCallback != null)
					completeCallback();
				return true;
			}
			return false;
		}
		internal override bool Show(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((!instance.displayStatusBar && MobileStatusNavigationBar.IsReadyStatusBar) || (!instance.displayNavigtionBar && MobileStatusNavigationBar.IsReadyNavigationBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Show Bars");
#endif
				bool _displayNavigtionBar = instance.displayNavigtionBar && !instance.iOSHideHomeBar;
				instance.displayNavigtionBar = !instance.iOSHideHomeBar;
				instance.displayStatusBar = true;
				MobileStatusNavigationBar.IsReadyStatusBar = false;
				MobileStatusNavigationBar.IsReadyNavigationBar = false;
				if (useColorTransition)
					instance.SetColor(toColorTransition);
				UpdateStatusBar(force);
				UpdateNavigationBar(force);
				if (completeCallback != null)
					completeCallback();
				return true;
			}
			return false;
		}
		internal override bool ShowStatus(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((!instance.displayStatusBar && MobileStatusNavigationBar.IsReadyStatusBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Show Status Bar");
#endif
				instance.displayStatusBar = true;
				MobileStatusNavigationBar.IsReadyStatusBar = false;
				if (useColorTransition)
					instance.SetStatusColor(toColorTransition);
				UpdateStatusBar(force);
				if (completeCallback != null)
					completeCallback();
				return true;
			}
			return false;
		}
		internal override bool ShowNavigation(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((!instance.displayNavigtionBar && MobileStatusNavigationBar.IsReadyNavigationBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Show Navigation Bar");
#endif
				instance.displayNavigtionBar = !instance.iOSHideHomeBar;
				MobileStatusNavigationBar.IsReadyNavigationBar = false;
				if (useColorTransition)
					instance.SetNavigationColor(toColorTransition);
				UpdateNavigationBar(force);
				if (completeCallback != null)
					completeCallback();
				return true;
			}
			return false;
		}
		[DllImport("__Internal")]
		private static extern float GetStatusBarHeight();
		[DllImport("__Internal")]
		private static extern float GetNavBarHeight();
		[DllImport("__Internal")]
		private static extern float LightMode();
		[DllImport("__Internal")]
		private static extern float DarkMode();
		[DllImport("__Internal")]
		private static extern float HideStatusNav();
		[DllImport("__Internal")]
		private static extern float ShowStatusNav();
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		static void RuntimeInitializeOnLoadMethod()
		{
			if (!BeforeSplashScreen.BeforeSplashScreenStatusBarLightMode)
				LightMode();
			else
				DarkMode();
		}
		internal override void UpdateStatusBar(bool force = false)
		{
#if !NODEBUG || UNITY_EDITOR
			if (instance.debugMode)
				Debug.Log("MSANB: Update Status Bar");
#endif
			if(instance.displayStatusBar && Screen.orientation == ScreenOrientation.Portrait)
			{
				ShowStatusNav();
				StatusBarHeight = MobileStatusNavigationBar.StatusBarHeight_STATIC;
			}
			else
			{
				HideStatusNav();
				StatusBarHeight = 0f;
			}
			instance.UpdateScrollbars();
			MobileStatusNavigationBar.IsReadyStatusBar = true;
		}
		internal override void UpdateNavigationBar(bool force = false)
		{
#if !NODEBUG || UNITY_EDITOR
			if (instance.debugMode)
				Debug.Log("MSANB: Update Navigation Bar");
#endif
			if (instance.displayNavigtionBar && !instance.iOSHideHomeBar 
			&& !(instance.iOSHideHomeBarInLandscape && Screen.orientation != ScreenOrientation.Portrait))
				NavigationBarHeight = MobileStatusNavigationBar.NavigationBarHeight_STATIC;
			else
				NavigationBarHeight = 0f;
			instance.UpdateScrollbars();
			MobileStatusNavigationBar.IsReadyNavigationBar = true;
		}
		internal override void UpdateStatusColor(Color c, TextMode statusBackgroundMode = TextMode.Auto)
		{
			instance.statusBarColor = c;
			switch (statusBackgroundMode)
			{
				case TextMode.Auto:
					bool lightBackground = MobileStatusNavigationBar.LightBackground(c);
					if (!lightBackground)
						LightMode();
					else
						DarkMode();
					break;
				case TextMode.Dark:
					DarkMode();
					break;
				case TextMode.Light:
					LightMode();
					break;
			}
		}
		internal override void UpdateNavigationColor(Color c, TextMode navigationTextMode = TextMode.Auto)
		{
			instance.navigationBarColor = c;
		}
		internal override void OnApplicationFocus(bool focus)
		{
			Refresh(true);
		}
		internal override void OnDisable() {}
		internal override void Update()
		{
			if (0 < updateLaterList.Count)
			{
				if (updateLaterList[0].seconds <= 0f)
				{
					updateLaterList[0].Action();
					updateLaterList.RemoveAt(0);
				}
				else
					updateLaterList[0].seconds -= Time.deltaTime;
			}
		}
		internal override void Awake() {}
	}
}
#endif