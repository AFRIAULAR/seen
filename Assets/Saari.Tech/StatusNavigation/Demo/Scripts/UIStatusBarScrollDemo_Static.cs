using UnityEngine;
using SaariTech.UI;

namespace SaariTech.Demo
{
	public class UIStatusBarScrollDemo_Static : MonoBehaviour
	{
		public RectTransform[] topbuttons;
		public RectTransform[] bottombuttons;

		public RectTransform[] top2buttons;
		public RectTransform[] bottom2buttons;

		public void SetStatusDarkCustom()
		{
			StatusNavigation.SetStatusDarkMode(TextMode.Dark);
		}

		public void SetStatusLightCustom()
		{
			StatusNavigation.SetStatusLightMode(TextMode.Dark);
		}

		public void SetNavigationDarkCustom()
		{
			StatusNavigation.SetNavigationDarkMode(TextMode.Dark);
		}

		public void SetNavigationLightCustom()
		{
			StatusNavigation.SetNavigationLightMode(TextMode.Dark);
		}

		public void Hide()
		{
			StatusNavigation.Hide();
		}

		public void Show()
		{
			StatusNavigation.Show();

		}

		private void Start()
		{
			Application.targetFrameRate = 60;
		}

		public void Update()
		{
			for (int i = 0; i < topbuttons.Length; i++)
			{
				topbuttons[i].anchoredPosition =
				new Vector2(
					topbuttons[i].anchoredPosition.x,
					-StatusNavigation.StatusBarHeight - topbuttons[i].rect.height / 2f - 100f
				);
			}

			for (int i = 0; i < bottombuttons.Length; i++)
			{
				bottombuttons[i].anchoredPosition =
				new Vector2(
					bottombuttons[i].anchoredPosition.x,
					StatusNavigation.NavigationBarHeight + bottombuttons[i].rect.height / 2f + 100f
				);
			}

			for (int i = 0; i < top2buttons.Length; i++)
			{
				top2buttons[i].anchoredPosition =
				new Vector2(
					top2buttons[i].anchoredPosition.x,
					-MobileStatusNavigationBar.StatusBarHeight - top2buttons[i].rect.height / 2f - 350f
				);
			}

			for (int i = 0; i < bottom2buttons.Length; i++)
			{
				bottom2buttons[i].anchoredPosition =
				new Vector2(
					bottom2buttons[i].anchoredPosition.x,
					MobileStatusNavigationBar.NavigationBarHeight + bottom2buttons[i].rect.height / 2f + 350f
				);
			}
		}
	}
}