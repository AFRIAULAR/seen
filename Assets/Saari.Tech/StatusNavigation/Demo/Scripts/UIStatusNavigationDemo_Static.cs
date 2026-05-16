using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaariTech.UI;
using UnityEngine.UI;

namespace SaariTech.Demo
{
	public class UIStatusNavigationDemo_Static : MonoBehaviour
	{
		public ColorPicker_Static colorPicker;
		public Text backTextDemo;
		private int backCount = 0;

		public void ShowWithTransition()
		{
			Color c = colorPicker.pickedColor;
			StatusNavigation.Show(new Color(c.r, c.g, c.b, 0f), colorPicker.pickedColor);
		}

		public void HideWithTransition()
		{
			Color sc = StatusNavigation.instance.statusBar.GetComponent<Image>().color;
			Color nc = StatusNavigation.instance.navigationBar.GetComponent<Image>().color;

			StatusNavigation.Hide(new Color(sc.r, sc.g, sc.b, 0f));
		}

		public void ShowStatus()
		{
			Color c = colorPicker.pickedColor;
			StatusNavigation.ShowStatus(new Color(c.r, c.g, c.b, 0f), colorPicker.pickedColor);
		}

		public void ShowNavigation()
		{
			Color c = colorPicker.pickedColor;
			StatusNavigation.ShowNavigation(new Color(c.r, c.g, c.b, 0f), colorPicker.pickedColor);
		}

		public void DarkMode()
		{
			StatusNavigation.SetDarkMode();
		}

		public void LightMode()
		{
			StatusNavigation.SetLightMode();
		}

		private void Start()
		{
			Application.targetFrameRate = 60;
		}

		public void PressBack()
		{
			backCount++;
			backTextDemo.text = backCount + "x Back button pressed.";
		}
	}
}