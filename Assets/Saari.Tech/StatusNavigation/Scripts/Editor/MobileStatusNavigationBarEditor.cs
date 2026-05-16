using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MobileStatusNavigationBar))]
public class MobileStatusNavigationBarEditor : Editor
{
	// Virtual Device
	SerializedProperty virtualDevice;

	// Bars
	SerializedProperty statusBar;
	SerializedProperty navigationBar;

	// Colors
	SerializedProperty statusDark;
	SerializedProperty navDark;
	SerializedProperty statusLight;
	SerializedProperty navLight;

	// Display
	SerializedProperty iOSHideHomeBar;
	SerializedProperty iOSHideHomeBarInLandscape;
	SerializedProperty startLightMode;
	SerializedProperty displayStatusBar;
	SerializedProperty displayNavigtionBar;
	SerializedProperty debugMode;
	SerializedProperty autoHideDelay;
	SerializedProperty previewStatusLabel;

	// Events
	SerializedProperty backButton;
	SerializedProperty slideUp;
	SerializedProperty autoHidden;

	// Experimental
	SerializedProperty colorizeSlideUp;

	// Foldouts
	bool showDevice = true;
	bool showReferences = false;
	bool showTheme = false;
	bool showDisplay = false;
	bool showEvents = false;
	bool showExperimental = false;

	// =====================================================

	void OnEnable()
	{
		virtualDevice = serializedObject.FindProperty("virtualDevice");

		// Bars
		statusBar = serializedObject.FindProperty("statusBar");
		navigationBar = serializedObject.FindProperty("navigationBar");

		// Colors
		statusDark = serializedObject.FindProperty("statusDarkModeBackgroundColor");
		navDark = serializedObject.FindProperty("navigationDarkModeBackgroundColor");

		statusLight = serializedObject.FindProperty("statusLightModeBackgroundColor");
		navLight = serializedObject.FindProperty("navigationLightModeBackgroundColor");

		// Display
		previewStatusLabel = serializedObject.FindProperty("previewStatusLabel");
		
		iOSHideHomeBar = serializedObject.FindProperty("iOSHideHomeBar");
		iOSHideHomeBarInLandscape = serializedObject.FindProperty("iOSHideHomeBarInLandscape");
		startLightMode = serializedObject.FindProperty("startLightMode");

		displayStatusBar = serializedObject.FindProperty("displayStatusBar");
		displayNavigtionBar = serializedObject.FindProperty("displayNavigtionBar");

		debugMode = serializedObject.FindProperty("DebugMode");

		autoHideDelay = serializedObject.FindProperty("androidAutoHideNavigationBarDelay");

		// Events
		backButton = serializedObject.FindProperty("backButtonOnClick");
		slideUp = serializedObject.FindProperty("androidNavigationBarUserSlideUp");
		autoHidden = serializedObject.FindProperty("androidNavigationBarAutoHidden");

		// Experimental
		colorizeSlideUp = serializedObject.FindProperty("colorizeAndoridSlideUpNavigationBar");
	}

	// =====================================================

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		DrawDevice();
		DrawEvents();
		DrawTheme();
		DrawDisplay();
		DrawBars();
		DrawExperimental();
		DrawAutoSetup();

		serializedObject.ApplyModifiedProperties();
	}

	// =====================================================

	void DrawAutoSetup()
	{
		GUILayout.Space(10);
		Draw(debugMode);
		EditorGUILayout.BeginVertical("box");
		EditorGUILayout.LabelField("Project Setup", EditorStyles.boldLabel);
		if (GUILayout.Button("Run Auto Setup", GUILayout.Height(30)))
		{
			if (EditorUtility.DisplayDialog(
				"Run Auto Setup",
				"This will modify PlayerSettings for the current build target.\n\nContinue?",
				"Apply",
				"Cancel"
			))
			{
				AutoSetup.RunAutoSetup();
			}
		}
		EditorGUILayout.HelpBox(
			"Apply recommended Android & iOS Player Settings for this project.",
			MessageType.None
		);

		EditorGUILayout.EndVertical();
	}

	// =====================================================

	void DrawDevice()
	{
		//showDevice = EditorGUILayout.BeginFoldoutHeaderGroup(showDevice, "Virtual Device");



		if (showDevice)
		{

			Draw(virtualDevice);

		}

		//EditorGUILayout.EndFoldoutHeaderGroup();
	}

	// =====================================================

	void DrawBars()
	{
		showReferences = EditorGUILayout.BeginFoldoutHeaderGroup(showReferences, "References");

		if (showReferences)
		{
			BeginBox();

			Draw(statusBar);
			Draw(navigationBar);

			EndBox();
		}

		EditorGUILayout.EndFoldoutHeaderGroup();
	}

	// =====================================================

	void DrawTheme()
	{
		showTheme = EditorGUILayout.BeginFoldoutHeaderGroup(showTheme, "Theme");

		if (showTheme)
		{
			Draw(startLightMode);

			BeginBox();

			EditorGUILayout.LabelField("Dark Mode", EditorStyles.boldLabel);

			Draw(statusDark);
			Draw(navDark);
			EndBox();

			GUILayout.Space(5);

			BeginBox();

			EditorGUILayout.LabelField("Light Mode", EditorStyles.boldLabel);

			Draw(statusLight);
			Draw(navLight);

			EndBox();
		}

		EditorGUILayout.EndFoldoutHeaderGroup();
	}

	// =====================================================

	void DrawDisplay()
	{
		showDisplay = EditorGUILayout.BeginFoldoutHeaderGroup(showDisplay, "Display");

		if (showDisplay)
		{
			BeginBox();

			Draw(previewStatusLabel);

			Draw(displayStatusBar);
			Draw(displayNavigtionBar);

			Draw(iOSHideHomeBar);
			if(!iOSHideHomeBar.boolValue)
				Draw(iOSHideHomeBarInLandscape);
			Draw(autoHideDelay);

			EndBox();
		}

		EditorGUILayout.EndFoldoutHeaderGroup();
	}

	// =====================================================

	void DrawEvents()
	{
		showEvents = EditorGUILayout.BeginFoldoutHeaderGroup(showEvents, "Events");

		if (showEvents)
		{
			BeginBox();

			Draw(backButton);
			Draw(slideUp);
			Draw(autoHidden);

			EndBox();
		}

		EditorGUILayout.EndFoldoutHeaderGroup();
	}

	// =====================================================

	void DrawExperimental()
	{
		showExperimental = EditorGUILayout.BeginFoldoutHeaderGroup(showExperimental, "Experimental");

		if (showExperimental)
		{
			BeginBox();

			Draw(colorizeSlideUp);

			EndBox();
		}

		EditorGUILayout.EndFoldoutHeaderGroup();
	}

	// =====================================================

	void Draw(SerializedProperty property)
	{
		if (property != null)
		{
			EditorGUILayout.PropertyField(property, true);
		}
	}

	void BeginBox()
	{
		EditorGUILayout.BeginVertical("box");
	}

	void EndBox()
	{
		EditorGUILayout.EndVertical();
	}
}