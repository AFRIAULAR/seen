using System;
using UnityEngine;
namespace SaariTech.UI
{
	public static class StatusNavigation
	{
		static internal MobileStatusNavigationBar instance;
		static private void CheckInstance()
		{
			if (instance == null)
			{
				instance = GameObject.FindFirstObjectByType<MobileStatusNavigationBar>(FindObjectsInactive.Exclude);
				if (MobileStatusNavigationBar.systemBars != null)
					MobileStatusNavigationBar.systemBars.Init();
			}
		}
		internal static UnityEngine.Events.UnityEvent backButtonOnClick
		{
			get
			{
				CheckInstance();
				return instance.backButtonOnClick;
			}
			set
			{
				CheckInstance();
				instance.backButtonOnClick = value;
			}
		}
		internal static bool startLightMode
		{
			get
			{
				CheckInstance();
				return instance.startLightMode;
			}
		}
		internal static bool displayStatusBar
		{
			get
			{
				CheckInstance();
				return instance.displayStatusBar;
			}
		}
		internal static bool displayNavigtionBar
		{
			get
			{
				CheckInstance();
				return instance.displayNavigtionBar;
			}
		}
		/// <summary>
		/// Get height of the Status bar.
		/// </summary>
		internal static bool IsReadyStatusBar
		{
			get
			{
				return MobileStatusNavigationBar.IsReadyStatusBar;
			}
		}
		/// <summary>
		/// Get height of the Navigation bar.
		/// </summary>
		internal static bool IsReadyNavigationBar
		{
			get
			{
				return MobileStatusNavigationBar.IsReadyNavigationBar;
			}
		}
		/// <summary>
		/// Get height of the currently active Status bar.
		/// </summary>
		internal static float StatusBarHeight
		{
			get
			{
				return MobileStatusNavigationBar.StatusBarHeight;
			}
		}
		/// <summary>
		/// Get height of the currently active Navigation bar.
		/// </summary>
		internal static float NavigationBarHeight
		{
			get
			{
				return MobileStatusNavigationBar.NavigationBarHeight;
			}
		}
		/// <summary>
		/// Get height of the static Status bar.
		/// </summary>
		internal static float StatusBarHeight_STATIC
		{
			get
			{
				return MobileStatusNavigationBar.StatusBarHeight_STATIC;
			}
		}
		/// <summary>
		/// Get height of the static Navigation bar.
		/// </summary>
		internal static float NavigationBarHeight_STATIC
		{
			get
			{
				return MobileStatusNavigationBar.NavigationBarHeight_STATIC;
			}
		}
		/// <summary>
		/// Hide the currently active Status & Navigation bar
		/// </summary>
		public static bool Hide(DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.Hide(completeCallback, force);
		}
		/// <summary>
		/// Hide the currently active Status & Navigation bar with color transition.
		/// </summary>
		public static bool Hide(Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.Hide(toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Hide the currently active Status bar.
		/// </summary>
		public static bool HideStatus(DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.HideStatus(completeCallback, force);
		}
		/// <summary>
		/// Hide the currently active Status bar with color transition.
		/// </summary>
		public static bool HideStatus(Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.HideStatus(toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Hide the currently active Navigation bar.
		/// </summary>
		public static bool HideNavigation(DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.HideNavigation(completeCallback, force);
		}
		/// <summary>
		/// Hide the currently active Navigation bar with color transition.
		/// </summary>
		public static bool HideNavigation(Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.HideNavigation(toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Show the currently active Status & Navigation bar.
		/// </summary>
		public static void Show(DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			instance.Show(completeCallback, force);
		}
		/// <summary>
		/// Show the currently active Status & Navigation bar with color transition.
		/// </summary>
		public static bool Show(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.Show(fromColorTransition, toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Show the currently active Status bar.
		/// </summary>
		public static bool ShowStatus(DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.ShowStatus(completeCallback, force);
		}
		/// <summary>
		/// Show the currently active Status bar with color transition.
		/// </summary>
		public static bool ShowStatus(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.ShowStatus(fromColorTransition, toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Show the currently active Navigation bar.
		/// </summary>
		public static bool ShowNavigation(DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.ShowNavigation(completeCallback, force);
		}
		/// <summary>
		/// Show the currently active  Navigation bar with color transition.
		/// </summary>
		public static bool ShowNavigation(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			CheckInstance();
			return instance.ShowNavigation(fromColorTransition, toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Set the colors of the currently active Status & Navigation bar to light.
		/// </summary>
		public static void SetLightMode()
		{
			CheckInstance();
			instance.SetLightMode();
		}
		/// <summary>
		/// Set the colors of the currently active Status & Navigation bar to dark.
		/// </summary>
		public static void SetDarkMode()
		{
			CheckInstance();
			instance.SetDarkMode();
		}
		[Obsolete("Use ChangeLightModeColorTemplate() instead.", true)]
		public static void ChangeColorLightMode(Color color)
		{ }
		[Obsolete("Use ChangeDarkModeColorTemplate() instead.", true)]
		public static void ChangeColorDarkMode(Color color)
		{ }
		/// <summary>
		/// Change the color of bars in light with your choice of text mode.
		/// </summary>
		public static void ChangeLightModeColorTemplate(Color color)
		{
			CheckInstance();
			instance.ChangeLightModeColorTemplate(color);
		}
		/// <summary>
		/// Change the color of bars in dark with your choice of text mode.
		/// </summary>
		public static void ChangeDarkModeColorTemplate(Color color)
		{
			CheckInstance();
			instance.ChangeDarkModeColorTemplate(color);
		}
		/// <summary>
		/// Change the color of Status bar to light with your choice of text mode.
		/// </summary>
		public static void ChangeStatusLightModeColorTemplate(Color color)
		{
			CheckInstance();
			instance.ChangeStatusLightModeColorTemplate(color);
		}
		/// <summary>
		/// Change the color of Status bar to dark with your choice of text mode.
		/// </summary>
		public static void ChangeStatusDarkModeColorTemplate(Color color)
		{
			CheckInstance();
			instance.ChangeStatusDarkModeColorTemplate(color);
		}
		/// <summary>
		/// Change the color of Navigation bar to light with your choice of text mode.
		/// </summary>
		public static void ChangeNavigationLightModeColorTemplate(Color color)
		{
			CheckInstance();
			instance.ChangeNavigationLightModeColorTemplate(color);
		}
		/// <summary>
		/// Change the color of Navigation bar to dark with your choice of text mode.
		/// </summary>
		public static void ChangeNavigationDarkModeColorTemplate(Color color)
		{
			CheckInstance();
			instance.ChangeNavigationDarkModeColorTemplate(color);
		}
		/// <summary>
		/// Set the colors of the currently active  Status & Navigation bar to light with your choice of text.
		/// </summary>
		public static void SetLightMode(TextMode textMode)
		{
			CheckInstance();
			instance.SetLightMode(textMode);
		}
		/// <summary>
		/// Set the colors of the currently active Status & Navigation bar to light with light text.
		/// </summary>
		public static void SetLightModeWithDarkContent()
		{
			CheckInstance();
			instance.SetLightModeWithDarkContent();
		}
		/// <summary>
		/// Set the colors of the currently active Status & Navigation bar to light with dark text.
		/// </summary>
		public static void SetLightModeWithLightContent()
		{
			CheckInstance();
			instance.SetLightModeWithLightContent();
		}
		/// <summary>
		/// Set the colors of the currently active Status & Navigation bar to dark with dark text.
		/// </summary>
		public static void SetDarkModeWithLightContent()
		{
			CheckInstance();
			instance.SetDarkModeWithLightContent();
		}
		/// <summary>
		/// Set the colors of the currently active Status & Navigation bar to dark with light text.
		/// </summary>
		public static void SetDarkModeWithDarkContent()
		{
			CheckInstance();
			instance.SetDarkModeWithDarkContent();
		}
		/// <summary>
		/// Set the colors of the currently active Status & Navigation bar to light with your choice of text mode.
		/// </summary>
		public static void SetDarkMode(TextMode textMode)
		{
			CheckInstance();
			instance.SetDarkMode(textMode);
		}
		/// <summary>
		/// Set the colors of the currently active Status & Navigation bar.
		/// </summary>
		public static void SetColor(Color color, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.SetColor(color, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Status & Navigation bar.
		/// </summary>
		public static void TransitionColor(Color color, float seconds, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.TransitionColor(color, seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Status bar.
		/// </summary>
		public static void TransitionStatusColor(Color color, float seconds, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.TransitionStatusColor(color, seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Navigation bar.
		/// </summary>
		public static void TransitionNavigationColor(Color color, float seconds, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.TransitionNavigationColor(color, seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Status & Navigation bar to dark mode.
		/// </summary>
		public static void TransitionDarkMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.TransitionDarkMode(seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Status bar to dark mode.
		/// </summary>
		public static void TransitionStatusDarkMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.TransitionStatusDarkMode(seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Navigation bar to dark mode.
		/// </summary>
		public static void TransitionNavigationDarkMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.TransitionNavigationDarkMode(seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Status & Navigation bar to light mode.
		/// </summary>
		public static void TransitionLightMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.TransitionLightMode(seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Status bar to light mode.
		/// </summary>
		public static void TransitionStatusLightMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.TransitionStatusLightMode(seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Navigation bar to light mode.
		/// </summary>
		public static void TransitionNavigationLightMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			CheckInstance();
			instance.TransitionNavigationLightMode(seconds, textMode);
		}
		/// <summary>
		/// Set the color of the currently active Status bar to light with dark text.
		/// </summary>
		public static void SetStatusLightModeWithDarkContent()
		{
			CheckInstance();
			instance.SetStatusLightModeWithDarkContent();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to light with light text.
		/// </summary>
		public static void SetStatusLightModeWithLightContent()
		{
			CheckInstance();
			instance.SetStatusLightModeWithLightContent();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to dark with light text.
		/// </summary>
		public static void SetStatusDarkModeWithLightContent()
		{
			CheckInstance();
			instance.SetStatusDarkModeWithLightContent();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to dark with dark text.
		/// </summary>
		public static void SetStatusDarkModeWithDarkContent()
		{
			CheckInstance();
			instance.SetStatusDarkModeWithDarkContent();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to light.
		/// </summary>
		public static void SetStatusLightMode()
		{
			CheckInstance();
			instance.SetStatusLightMode();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to light with your choice of text mode.
		/// </summary>
		public static void SetStatusLightMode(TextMode statusBackgroundMode)
		{
			CheckInstance();
			instance.SetStatusLightMode(statusBackgroundMode);
		}
		/// <summary>
		/// Set the color of the currently active Status bar to dark.
		/// </summary>
		public static void SetStatusDarkMode()
		{
			CheckInstance();
			instance.SetStatusDarkMode();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to dark with your choice of text mode.
		/// </summary>
		public static void SetStatusDarkMode(TextMode statusBackgroundMode)
		{
			CheckInstance();
			instance.SetStatusDarkMode(statusBackgroundMode);
		}
		/// <summary>
		/// Set the color of the currently active Status bar.
		/// </summary>
		public static void SetStatusColor(Color color)
		{
			CheckInstance();
			instance.SetStatusColor(color);
		}
		/// <summary>
		/// Set the color of the currently active Status bar with your choice of text mode.
		/// </summary>
		public static void SetStatusColor(Color color, TextMode statusBackgroundMode = TextMode.Auto)
		{
			CheckInstance();
			instance.SetStatusColor(color, statusBackgroundMode);
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to light with dark text.
		/// </summary>
		public static void SetNavigationLightModeWithDarkContent()
		{
			CheckInstance();
			instance.SetNavigationLightModeWithDarkContent();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to light with light text.
		/// </summary>
		public static void SetNavigationLightModeWithLightContent()
		{
			CheckInstance();
			instance.SetNavigationLightModeWithLightContent();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to dark with light text.
		/// </summary>
		public static void SetNavigationDarkModeWithLightContent()
		{
			CheckInstance();
			instance.SetNavigationDarkModeWithLightContent();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to dark with dark text.
		/// </summary>
		public static void SetNavigationDarkModeWithDarkContent()
		{
			CheckInstance();
			instance.SetNavigationDarkModeWithDarkContent();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to light mode.
		/// </summary>
		public static void SetNavigationLightMode()
		{
			CheckInstance();
			instance.SetNavigationLightMode();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to dark mode.
		/// </summary>
		public static void SetNavigationDarkMode()
		{
			CheckInstance();
			instance.SetNavigationDarkMode();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to light with your choice of text mode.
		/// </summary>
		public static void SetNavigationLightMode(TextMode navigationTextMode = TextMode.Auto)
		{
			CheckInstance();
			instance.SetNavigationLightMode(navigationTextMode);
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to dark with your choice of text mode.
		/// </summary>
		public static void SetNavigationDarkMode(TextMode navigationTextMode = TextMode.Auto)
		{
			CheckInstance();
			instance.SetNavigationDarkMode(navigationTextMode);
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar.
		/// </summary>
		public static void SetNavigationColor(Color color)
		{
			CheckInstance();
			instance.SetNavigationColor(color);
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar with your choice of text mode.
		/// </summary>
		public static void SetNavigationColor(Color color, TextMode navigationTextMode = TextMode.Auto)
		{
			CheckInstance();
			instance.SetNavigationColor(color, navigationTextMode);
		}
	}
}