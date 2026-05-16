using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SaariTech.UI;
namespace SaariTech.Demo
{
	public class UIDayNightDemo : MonoBehaviour
	{
		public Image background;
		public Image berg;
		public RectTransform sun;
		public RectTransform moon;
		public MobileStatusNavigationBar mobileStatusNavigationBar;

		private bool dayMode = true;
		const float LERP = 0.1f;

		void Update()
		{
			if (dayMode)
			{
				background.color = Color.Lerp(background.color, new Color(0.75f, 0.75f, 0.75f), LERP);
				berg.color = Color.Lerp(berg.color, new Color(1f, 0.7007163f, 0.5330188f), LERP);
				sun.GetComponent<Image>().color = Color.Lerp(sun.GetComponent<Image>().color, new Color(1f, 1f, 1f), LERP);
				sun.anchoredPosition = Vector2.Lerp(sun.anchoredPosition, new Vector2(-256f - 64f, -40f), LERP);
				moon.anchoredPosition = Vector2.Lerp(moon.anchoredPosition, new Vector2(-64f, 0f), LERP);
			}
			else
			{
				background.color = Color.Lerp(background.color, new Color(0.25f, 0.25f, 0.25f), LERP);
				berg.color = Color.Lerp(berg.color, new Color(0.128649f, 0.236873f, 0.4622642f), LERP);
				sun.GetComponent<Image>().color = Color.Lerp(sun.GetComponent<Image>().color, new Color(1f, 1f, 0f), LERP);
				sun.anchoredPosition = Vector2.Lerp(sun.anchoredPosition, new Vector2(256f + 0f, -40f), LERP);
				moon.anchoredPosition = Vector2.Lerp(moon.anchoredPosition, new Vector2(-16f, 0f), LERP);
			}
		}

		private void LateUpdate()
		{
			moon.GetComponent<Image>().color = mobileStatusNavigationBar.statusBar.GetComponent<Image>().color;
		}

		public void ToggleToDay()
		{
			dayMode = true;
			mobileStatusNavigationBar.TransitionLightMode(1f);
		}

		public void ToggleToNight()
		{
			dayMode = false;
			mobileStatusNavigationBar.TransitionDarkMode(1f);
		}
	}
}