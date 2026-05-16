using UnityEngine;
using SaariTech.UI;

internal abstract class SystemBarsBase
{
	internal abstract void Init(bool force = false);
	internal abstract bool Show(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false);
	internal abstract bool Hide(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false);
	internal abstract bool ShowStatus(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false);
	internal abstract bool HideStatus(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false);
	internal abstract bool ShowNavigation(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false);
	internal abstract bool HideNavigation(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false);
	internal abstract void UpdateStatusBar(bool force = false);
	internal abstract void UpdateNavigationBar(bool force = false);
	internal abstract void UpdateStatusColor(Color c, TextMode statusBackgroundMode = TextMode.Auto);
	internal abstract void UpdateNavigationColor(Color c, TextMode navigationTextMode = TextMode.Auto);
	internal abstract void OnApplicationFocus(bool focus);
	internal abstract void OnDisable();
	internal abstract void Update();
	internal abstract void Awake();
	internal abstract void OnScreenOrientationChange();
	internal abstract void Refresh(bool force = false);
}
