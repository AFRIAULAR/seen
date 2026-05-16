using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SaariTech.UI
{
	[RequireComponent(typeof(Scrollbar))]
	public class MobileScrollVertical : MonoBehaviour
	{
		private RectTransform self;

		private IEnumerator Start()
		{
			self = GetComponent<RectTransform>();

			yield return new WaitWhile(() => (
				MobileStatusNavigationBar.StatusBarHeight == 0
				&& MobileStatusNavigationBar.NavigationBarHeight == 0
			));
			
			UpdateScrollbar(MobileStatusNavigationBar.StatusBarHeight, MobileStatusNavigationBar.NavigationBarHeight);
		}

		internal void UpdateScrollbar(float StatusBarHeight, float NavigationBarHeight)
		{
			if(self == null)
				self = GetComponent<RectTransform>();

			self.SetTop(StatusBarHeight);
			self.SetBottom(NavigationBarHeight);
		}
	}
}
