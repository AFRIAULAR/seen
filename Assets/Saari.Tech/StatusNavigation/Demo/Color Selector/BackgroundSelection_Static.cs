using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SaariTech.Demo
{
	public class BackgroundSelection_Static : MonoBehaviour
	{
		internal static Color currentColor;
		public ColorPicker_Static colorPicker;
		public HuePicker_Static huePicker;
		public bool isOpned = false;

		public void OpenPicker()
		{
			if (isOpned)
			{
				if (Vector3.Distance(
					new Vector3(currentColor.r, currentColor.g, currentColor.b),
					new Vector3(colorPicker.pickedColor.r, colorPicker.pickedColor.g, colorPicker.pickedColor.b)
				) > 0.01f)
				{
					huePicker.GoToColor(currentColor);
					colorPicker.GoToColor(currentColor);
					currentColor = colorPicker.pickedColor;
				}
			}

			colorPicker.gameObject.SetActive(isOpned);
			huePicker.gameObject.SetActive(isOpned);
		}

		public void OnToggle()
		{
			isOpned = !isOpned;
			OpenPicker();
		}

		public void SetOpen(bool onOpen)
		{
			isOpned = onOpen;
			OpenPicker();
		}

		private void Start()
		{
			Color colorTarget = new Color(30f / 255f, 231f / 255f, 1f / 255f);
			try
			{
				huePicker.GoToColor(colorTarget);
				colorPicker.GoToColor(colorTarget);
				currentColor = colorTarget;
			}
			catch
			{
			}
		}
	}
}