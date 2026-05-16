using UnityEngine;
using SaariTech.UI;

namespace SaariTech.Demo
{
	public class UITransitionColorDemo_Sattic : MonoBehaviour
	{
		private void Start()
		{
			Application.targetFrameRate = 60;
		}

		public void TransitionStatusToRed()
		{
			StatusNavigation.TransitionStatusColor(new Color(1, 0, 0, 0.5f), 1.25f);
		}

		public void TransitionStatusToGreen()
		{
			StatusNavigation.TransitionStatusColor(new Color(0, 1, 0, 0.5f), 1f);
		}

		public void TransitionStatusToBlue()
		{
			StatusNavigation.TransitionStatusColor(new Color(0, 0, 1, 0.5f), 0.75f);
		}

		public void TransitionToLightMode()
		{
			StatusNavigation.TransitionLightMode(0.25f);
		}

		public void TransitionToGrey()
		{
			StatusNavigation.TransitionColor(Color.grey, 0.5f);
		}

		public void TransitionToDarkMode()
		{
			StatusNavigation.TransitionDarkMode(0.75f);
		}

		public void TransitionNavigationToYellow()
		{
			StatusNavigation.TransitionNavigationColor(new Color(1, 1, 0, 0.5f), Mathf.Deg2Rad * 100f);
		}

		public void TransitionNavigationToMagenta()
		{
			StatusNavigation.TransitionNavigationColor(new Color(1, 0, 1, 0.5f), Mathf.PI);
		}

		public void TransitionNavigationToCyan()
		{
			StatusNavigation.TransitionNavigationColor(new Color(0, 1, 1, 0.5f), Mathf.Rad2Deg / 10f);
		}
	}
}