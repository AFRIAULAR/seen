using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SaariTech.UI;

namespace SaariTech.Demo
{
	public class AutoTestDemo_Static : MonoBehaviour
	{
		public Text label;

		private void Start()
		{
			Application.targetFrameRate = 60;
			StartCoroutine(ITest());
		}

		IEnumerator IWait(string text, float seconds)
		{
			for (float t = seconds; t >= 0; t -= 0.1f)
			{
				label.text = text + ": " + (Mathf.RoundToInt(t * 10) / 10f).ToString().Replace(",", ".");
				yield return new WaitForSeconds(0.1f);
			}
			label.text = "";
		}

		IEnumerator ITest()
		{
			label.text = "Starting Auto Test";
			yield return new WaitForSeconds(2f);

#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_WEBGL
		label.text = "Android API Level: " + UIStatusNavigationBarAndroid.GetSDKLevel().ToString();

		yield return new WaitForSeconds(2f);

		StatusNavigation.HideNavigation();
		yield return IWait("Hide Navigation Bar", 3f);

		StatusNavigation.HideStatus();
		yield return IWait("Hide Status Bar", 3f);

		StatusNavigation.Show();
		yield return IWait("Show Bars", 3f);
#else
			StatusNavigation.HideNavigation();
			yield return IWait("Hide Navigation Bar", 3f);

			StatusNavigation.HideStatus();
			yield return IWait("Hide Status Bar", 3f);

			StatusNavigation.Show();
			yield return IWait("Show Bars", 3f);
#endif

			StatusNavigation.TransitionStatusColor(Color.green, 1f);
			StatusNavigation.TransitionNavigationColor(Color.blue, 1f);
			yield return IWait("Color Transitions 1", 3f);

			StatusNavigation.TransitionStatusColor(Color.black, 1f);
			StatusNavigation.TransitionNavigationColor(Color.black, 1f);
			yield return IWait("Color Transitions 2", 3f);

			StatusNavigation.TransitionStatusColor(Color.white, 1f);
			StatusNavigation.TransitionNavigationColor(Color.white, 1f);
			yield return IWait("Color Transitions 3", 3f);

			label.text = "Auto Test Completed";
			yield return new WaitForSeconds(2f);
			StartCoroutine(ITest());
		}
	}
}