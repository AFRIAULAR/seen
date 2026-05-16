using System;
using UnityEngine;

namespace SaariTech.UI
{
	public class StatusBar
	{
		internal static bool displayBar
		{
			get
			{
				return StatusNavigation.displayStatusBar;
			}
		}
		/// <summary>
		/// Get height of the Status bar.
		/// </summary>
		internal static bool IsReady
		{
			get
			{
				return MobileStatusNavigationBar.IsReadyStatusBar;
			}
		}
		/// <summary>
		/// Get height of the currently active Status bar.
		/// </summary>
		internal static float Height
		{
			get
			{
#if UNITY_ANDROID && !UNITY_EDITOR
				return AndroidSystemBars.StatusBarHeight;
#elif UNITY_IOS && !UNITY_EDITOR
				return IOSSystemBars.StatusBarHeight;
#else
				return OtherSystemBars.StatusBarHeight;
#endif
			}
		}
		/// <summary>
		/// Get height of the static Status bar.
		/// </summary>
		internal static float Height_STATIC
		{
			get
			{
#if UNITY_ANDROID && !UNITY_EDITOR
				return AndroidSystemBars.StatusBarHeight_STATIC;
#elif UNITY_IOS && !UNITY_EDITOR
				return IOSSystemBars.StatusBarHeight_STATIC;
#else
				return OtherSystemBars.StatusBarHeight_STATIC;
#endif
			}
		}
		/// <summary>
		/// Hide the currently active Status bar.
		/// </summary>
		public static bool Hide(DelegateAction completeCallback = null, bool force = false)
		{
			return StatusNavigation.HideStatus(completeCallback, force);
		}
		/// <summary>
		/// Hide the currently active Status bar with color transition.
		/// </summary>
		public static bool Hide(Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			return StatusNavigation.HideStatus(toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Show the currently active Status bar.
		/// </summary>
		public static bool Show(DelegateAction completeCallback = null, bool force = false)
		{
			return StatusNavigation.ShowStatus(completeCallback, force);
		}
		/// <summary>
		/// Show the currently active Status bar with color transition.
		/// </summary>
		public static bool Show(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			return StatusNavigation.ShowStatus(fromColorTransition, toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Change the color of Status bar to light with your choice of text mode.
		/// </summary>
		public static void ChangeLightModeColorTemplate(Color color)
		{
			StatusNavigation.ChangeStatusLightModeColorTemplate(color);
		}
		/// <summary>
		/// Change the color of Status bar to dark with your choice of text mode.
		/// </summary>
		public static void ChangeDarkModeColorTemplate(Color color)
		{
			StatusNavigation.ChangeStatusDarkModeColorTemplate(color);
		}
		/// <summary>
		/// Color transition of the currently active Status bar.
		/// </summary>
		public static void TransitionColor(Color color, float seconds, TextMode textMode = TextMode.Auto)
		{
			StatusNavigation.TransitionStatusColor(color, seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Status bar to dark mode.
		/// </summary>
		public static void TransitionDarkMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			StatusNavigation.TransitionStatusDarkMode(seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Status bar to light mode.
		/// </summary>
		public static void TransitionLightMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			StatusNavigation.TransitionStatusLightMode(seconds, textMode);
		}
		/// <summary>
		/// Set the color of the currently active Status bar to light with dark text.
		/// </summary>
		public static void SetLightModeWithDarkContent()
		{
			StatusNavigation.SetStatusLightModeWithDarkContent();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to light with light text.
		/// </summary>
		public static void SetLightModeWithLightContent()
		{
			StatusNavigation.SetStatusLightModeWithLightContent();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to dark with light text.
		/// </summary>
		public static void SetDarkModeWithLightContent()
		{
			StatusNavigation.SetStatusDarkModeWithLightContent();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to dark with dark text.
		/// </summary>
		public static void SetDarkModeWithDarkContent()
		{
			StatusNavigation.SetStatusDarkModeWithDarkContent();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to light.
		/// </summary>
		public static void SetLightMode()
		{
			StatusNavigation.SetStatusLightMode();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to light with your choice of text mode.
		/// </summary>
		public static void SetLightMode(TextMode statusBackgroundMode)
		{
			StatusNavigation.SetStatusLightMode(statusBackgroundMode);
		}
		/// <summary>
		/// Set the color of the currently active Status bar to dark.
		/// </summary>
		public static void SetDarkMode()
		{
			StatusNavigation.SetStatusDarkMode();
		}
		/// <summary>
		/// Set the color of the currently active Status bar to dark with your choice of text mode.
		/// </summary>
		public static void SetDarkMode(TextMode statusBackgroundMode)
		{
			StatusNavigation.SetStatusDarkMode(statusBackgroundMode);
		}
		/// <summary>
		/// Set the color of the currently active Status bar.
		/// </summary>
		public static void SetColor(Color color)
		{
			StatusNavigation.SetStatusColor(color);
		}
		/// <summary>
		/// Set the color of the currently active Status bar with your choice of text mode.
		/// </summary>
		public static void SetColor(Color color, TextMode statusBackgroundMode = TextMode.Auto)
		{
			StatusNavigation.SetStatusColor(color, statusBackgroundMode);
		}
	}
}
