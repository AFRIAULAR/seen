#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;
public static class AutoSetup
{
	private const bool StatusBarHidden = false;
	private const bool HideNavigationBar = false;
	private const bool RenderOutsideSafeArea = true;
	private const bool AllowedAutorotateToPortraitUpsideDown = false;
	internal static void RunAutoSetup()
	{
		bool ranSomething = false;
		bool changedSomething = false;
		bool currentAllowedAutorotateToPortraitUpsideDown = PlayerSettings.allowedAutorotateToPortraitUpsideDown;
		if (currentAllowedAutorotateToPortraitUpsideDown != AllowedAutorotateToPortraitUpsideDown)
		{
			PlayerSettings.allowedAutorotateToPortraitUpsideDown = AllowedAutorotateToPortraitUpsideDown;
			changedSomething = changedSomething || true;
			Debug.Log($"[Auto Setup] PlayerSettings.allowedAutorotateToPortraitUpsideDown changed: {currentAllowedAutorotateToPortraitUpsideDown} -> {AllowedAutorotateToPortraitUpsideDown}");
		}
		if (changedSomething)
			AssetDatabase.SaveAssets();
		// Android
		if (BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android))
		{
			Debug.Log("[Auto Setup] Android module detected.");
			changedSomething = changedSomething || ApplyAndroidPlayerSettings();
			ranSomething = true;
		}
		else
			Debug.Log("[Auto Setup] Android module not installed. Skipping.");
		// iOS
		if (BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.iOS, BuildTarget.iOS))
		{
			Debug.Log("[Auto Setup] iOS module detected.");
			changedSomething = changedSomething || ApplyIOSPlayerSettings();
			ranSomething = true;
		}
		else
			Debug.Log("[Auto Setup] iOS module not installed. Skipping.");
		if (!ranSomething)
			Debug.LogWarning("[Auto Setup] No supported mobile build modules found (Android/iOS).");
		else if(!changedSomething)
			Debug.Log("[Auto Setup] No changes needed. Values are already set.");
		else
			Debug.Log("[Auto Setup] Executed.");
	}
	private static bool ApplyAndroidPlayerSettings()
	{
		bool changed = false;
		bool currentStartInFullscreen = PlayerSettings.Android.startInFullscreen;
		if (currentStartInFullscreen != HideNavigationBar)
		{
			PlayerSettings.Android.startInFullscreen = HideNavigationBar;
			changed = true;
			Debug.Log($"[Auto Setup] PlayerSettings.Android.startInFullscreen changed: {currentStartInFullscreen} -> {HideNavigationBar} (Hide Navigation Bar OFF)");
		}
		bool currentRenderOutside = PlayerSettings.Android.renderOutsideSafeArea;
		if (currentRenderOutside != RenderOutsideSafeArea)
		{
			PlayerSettings.Android.renderOutsideSafeArea = RenderOutsideSafeArea;
			changed = true;
			Debug.Log($"[Auto Setup] PlayerSettings.Android.renderOutsideSafeArea changed: {currentRenderOutside} -> {RenderOutsideSafeArea}");
		}
		if (changed)
			AssetDatabase.SaveAssets();
		return changed;
	}
	private static bool ApplyIOSPlayerSettings()
	{
		bool changed = false;
		if (PlayerSettings.statusBarHidden != StatusBarHidden)
		{
			PlayerSettings.statusBarHidden = StatusBarHidden;
			changed = true;
			Debug.Log($"[Auto Setup] PlayerSettings.iOS.statusBarHidden set to {StatusBarHidden}");
		}
		PropertyInfo property = typeof(PlayerSettings).GetProperty("statusBarHidden");
		if (property != null)
		{
			bool currentStatusBarHidden = (bool)property.GetValue(null, null);
			if (currentStatusBarHidden != StatusBarHidden)
			{
				property.SetValue(null, StatusBarHidden, null);
				changed = true;
				Debug.Log($"[Auto Setup] PlayerSettings.statusBarHidden changed: {currentStatusBarHidden} -> {StatusBarHidden}");
			}
		}
		if (changed)
			AssetDatabase.SaveAssets();
		return changed;
	}
}
#endif
