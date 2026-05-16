using System;
using UnityEngine;

namespace SaariTech.UI
{
	public class NavigationBar
	{
		internal static UnityEngine.Events.UnityEvent backButtonOnClick
		{
			get
			{
				return StatusNavigation.backButtonOnClick;
			}
			set
			{
				StatusNavigation.backButtonOnClick = value;
			}
		}
		internal static bool displayBar
		{
			get
			{
				return StatusNavigation.displayNavigtionBar;
			}
		}
		/// <summary>
		/// Get height of the Navigation bar.
		/// </summary>
		internal static bool IsReady
		{
			get
			{
				return MobileStatusNavigationBar.IsReadyNavigationBar;
			}
		}
		/// <summary>
		/// Get height of the currently active Navigation bar.
		/// </summary>
		internal static float Height
		{
			get
			{
				return MobileStatusNavigationBar.NavigationBarHeight;
			}
		}
		/// <summary>
		/// Get height of the static Navigation bar.
		/// </summary>
		internal static float Height_STATIC
		{
			get
			{
				return MobileStatusNavigationBar.NavigationBarHeight_STATIC;
			}
		}
		/// <summary>
		/// Hide the currently active Navigation bar.
		/// </summary>
		public static bool Hide(DelegateAction completeCallback = null, bool force = false)
		{
			return StatusNavigation.HideNavigation(completeCallback, force);
		}
		/// <summary>
		/// Hide the currently active Navigation bar with color transition.
		/// </summary>
		public static bool Hide(Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			return StatusNavigation.HideNavigation(toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Show the currently active Navigation bar.
		/// </summary>
		public static bool Show(DelegateAction completeCallback = null, bool force = false)
		{
			return StatusNavigation.ShowNavigation(completeCallback, force);
		}
		/// <summary>
		/// Show the currently active  Navigation bar with color transition.
		/// </summary>
		public static bool Show(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
		{
			return StatusNavigation.ShowNavigation(fromColorTransition, toColorTransition, completeCallback, force);
		}
		/// <summary>
		/// Change the color of Navigation bar to light with your choice of text mode.
		/// </summary>
		public static void ChangeLightModeColorTemplate(Color color)
		{
			StatusNavigation.ChangeNavigationLightModeColorTemplate(color);
		}
		/// <summary>
		/// Change the color of Navigation bar to dark with your choice of text mode.
		/// </summary>
		public static void ChangeDarkModeColorTemplate(Color color)
		{
			StatusNavigation.ChangeNavigationDarkModeColorTemplate(color);
		}
		/// <summary>
		/// Color transition of the currently active Navigation bar.
		/// </summary>
		public static void TransitionColor(Color color, float seconds, TextMode textMode = TextMode.Auto)
		{
			StatusNavigation.TransitionNavigationColor(color, seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Navigation bar to dark mode.
		/// </summary>
		public static void TransitionDarkMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			StatusNavigation.TransitionNavigationDarkMode(seconds, textMode);
		}
		/// <summary>
		/// Color transition of the currently active Navigation bar to light mode.
		/// </summary>
		public static void TransitionLightMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
		{
			StatusNavigation.TransitionNavigationLightMode(seconds, textMode);
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to light with dark text.
		/// </summary>
		public static void SetLightModeWithDarkContent()
		{
			StatusNavigation.SetNavigationLightModeWithDarkContent();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to light with light text.
		/// </summary>
		public static void SetLightModeWithLightContent()
		{
			StatusNavigation.SetNavigationLightModeWithLightContent();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to dark with light text.
		/// </summary>
		public static void SetDarkModeWithLightContent()
		{
			StatusNavigation.SetNavigationDarkModeWithLightContent();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to dark with dark text.
		/// </summary>
		public static void SetDarkModeWithDarkContent()
		{
			StatusNavigation.SetNavigationDarkModeWithDarkContent();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to light mode.
		/// </summary>
		public static void SetLightMode()
		{
			StatusNavigation.SetNavigationLightMode();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to dark mode.
		/// </summary>
		public static void SetDarkMode()
		{
			StatusNavigation.SetNavigationDarkMode();
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to light with your choice of text mode.
		/// </summary>
		public static void SetLightMode(TextMode navigationTextMode = TextMode.Auto)
		{
			StatusNavigation.SetNavigationLightMode(navigationTextMode);
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar to dark with your choice of text mode.
		/// </summary>
		public static void SetDarkMode(TextMode navigationTextMode = TextMode.Auto)
		{
			StatusNavigation.SetNavigationDarkMode(navigationTextMode);
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar.
		/// </summary>
		public static void SetColor(Color color)
		{
			StatusNavigation.SetNavigationColor(color);
		}
		/// <summary>
		/// Set the color of the currently active Navigation bar with your choice of text mode.
		/// </summary>
		public static void SetColor(Color color, TextMode navigationTextMode = TextMode.Auto)
		{
			StatusNavigation.SetNavigationColor(color, navigationTextMode);
		}
	}
}