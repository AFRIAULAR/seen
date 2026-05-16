using UnityEngine;
using SaariTech.UI;
using UnityEngine.UI;
using System.Collections;
public delegate void DelegateAction();
public class MobileStatusNavigationBar : MonoBehaviour
{
	internal static MobileStatusNavigationBar Instance;
	internal static SystemBarsBase systemBars =
#if UNITY_ANDROID && !UNITY_EDITOR
		new AndroidSystemBars();
#elif UNITY_IOS && !UNITY_EDITOR
		new IOSSystemBars();
#else
		new OtherSystemBars();
#endif
	public enum VirtualDevice
	{
		Auto,
		Android,
		iOS
	}
	public VirtualDevice virtualDevice = VirtualDevice.Auto;
	public RectTransform statusBar;
	public RectTransform navigationBar;
	internal RectTransform canvasRectTransform;
	internal RectTransform main;
	public Color statusDarkModeBackgroundColor = Color.black;
	public Color navigationDarkModeBackgroundColor = Color.black;
	public Color statusLightModeBackgroundColor = Color.white;
	public Color navigationLightModeBackgroundColor = Color.white;
	//#pragma warning disable 0414
	public bool iOSHideHomeBar = true;
	public bool iOSHideHomeBarInLandscape = true;
	//#pragma warning restore 0414
	public bool startLightMode = true;
	public bool displayStatusBar = true;
	public bool displayNavigtionBar = true;
	[SerializeField]
	internal string previewStatusLabel;
#if !NODEBUG || UNITY_EDITOR
	/// <summary>
	/// Debug Mode
	/// </summary>
	public bool debugMode = false;
#endif
	internal byte indexStatus = byte.MinValue;
	internal byte indexNavigation = byte.MinValue;
	static internal float KeyboardExcludeNavigationBarHeight = 0f;
	/// <summary>
	/// Navigation bar auto hide delay in seconds. 
	/// </summary>
	public float androidAutoHideNavigationBarDelay = 2f;
	/// <summary>
	/// Execute code at back button.
	/// </summary>
	public UnityEngine.Events.UnityEvent backButtonOnClick;
	/// <summary>
	/// Execute code at Sliding Up Navigation bar.
	/// </summary>
	public UnityEngine.Events.UnityEvent androidNavigationBarUserSlideUp;
	/// <summary>
	/// Execute code at Auto hide Navigation bar.
	/// </summary>
	public UnityEngine.Events.UnityEvent androidNavigationBarAutoHidden;
	/// <summary>
	/// Colorize for slide up navigation bar.
	/// </summary>
	public bool colorizeAndoridSlideUpNavigationBar = false;
	/// <summary>
	/// Color of the background Image component in Status bar.
	/// </summary>
	internal Color statusBarColor
	{
		get
		{
			return statusBar.gameObject.GetComponent<Image>().color;
		}
		set
		{
			statusBar.gameObject.GetComponent<Image>().color = value;
		}
	}
	/// <summary>
	/// Color of the background Image component in Navigation bar.
	/// </summary>
	internal Color navigationBarColor
	{
		get
		{
			return navigationBar.gameObject.GetComponent<Image>().color;
		}
		set
		{
			navigationBar.gameObject.GetComponent<Image>().color = value;
		}
	}
	/// <summary>
	/// Get ready state of the Status bar.
	/// </summary>
	static internal bool IsReadyStatusBar;
	/// <summary>
	/// Get ready state of the Navigation bar.
	/// </summary>
	static internal bool IsReadyNavigationBar;
	/// <summary>
	/// Get the realtime height of the Status bar.
	/// </summary>
	static internal float StatusBarHeight
	{
		get
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return AndroidSystemBars.StatusBarHeight;
#elif UNITY_IOS && !UNITY_EDITOR
			return IOSSystemBars.StatusBarHeight;
#else
			return OtherSystemBars.StatusBarHeight;
#endif
		}
	}
	/// <summary>
	/// Get the realtime height of the Navigation bar.
	/// </summary>
	static internal float NavigationBarHeight
	{
		get
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return AndroidSystemBars.NavigationBarHeight;
#elif UNITY_IOS && !UNITY_EDITOR
			return IOSSystemBars.NavigationBarHeight;
#else
			return OtherSystemBars.NavigationBarHeight;
#endif
		}
	}
	/// <summary>
	/// Get the constant height of the Status bar.
	/// </summary>
	static internal float StatusBarHeight_STATIC
	{
		get
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return AndroidSystemBars.StatusBarHeight_STATIC;
#elif UNITY_IOS && !UNITY_EDITOR
			return IOSSystemBars.StatusBarHeight_STATIC;
#else
			return OtherSystemBars.StatusBarHeight_STATIC;
#endif
		}
	}
	/// <summary>
	/// Get the constant height of the Navigation bar.
	/// </summary>
	static internal float NavigationBarHeight_STATIC
	{
		get
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return AndroidSystemBars.NavigationBarHeight_STATIC;
#elif UNITY_IOS && !UNITY_EDITOR
			return IOSSystemBars.NavigationBarHeight_STATIC;
#else
			return OtherSystemBars.NavigationBarHeight_STATIC;
#endif
		}
	}
	private ScreenOrientation lastScreenOrientation;
	private bool firstTime = true;
	internal void Start()
	{
		InitBase();
		StatusNavigation.instance = this;
		systemBars.Init(true);
		systemBars.UpdateStatusBar(true);
		systemBars.UpdateNavigationBar(true);
		lastScreenOrientation = Screen.orientation;
	}
	private void OnEnable()
	{
		InitBase();
#if UNITY_ANDROID && !UNITY_EDITOR
		((AndroidSystemBars)systemBars).OnEnable();
#endif
		StatusNavigation.instance = this;
		if (firstTime)
			firstTime = false;
		else
			Refresh();
	}
	void OnValidate()
	{
		statusBar.anchorMin = Vector2.zero;
		statusBar.anchorMax = Vector2.one;
		statusBar.pivot = new Vector2(0.5f, 0.5f);
		statusBar.localScale = Vector3.one;
		statusBar.localRotation = Quaternion.identity;
		navigationBar.anchorMin = Vector2.zero;
		navigationBar.anchorMax = Vector2.one;
		navigationBar.pivot = new Vector2(0.5f, 0.5f);
		navigationBar.localScale = Vector3.one;
		navigationBar.localRotation = Quaternion.identity;
		statusBar.SetTop(0f);
		statusBar.SetLeft(0f);
		statusBar.SetRight(0f);
		navigationBar.SetBottom(0f);
		navigationBar.SetLeft(0f);
		navigationBar.SetRight(0f);
		try // This is a foul fix, this CanvasScaler can sometimes be null.
		{
			CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
			statusBar.SetBottom(canvasScaler.referenceResolution.y);
			navigationBar.SetTop(canvasScaler.referenceResolution.y);
		}
		catch { }
	}
	public void Refresh(bool force = false)
	{
		systemBars.Refresh(force);
	}
	private void Awake()
	{
		InitBase();
		StatusNavigation.instance = this;
		systemBars.Init(true);
	}
	void OnApplicationFocus(bool focus)
	{
		systemBars.OnApplicationFocus(focus);
	}
	void OnDisable()
	{
		systemBars.OnDisable();
	}
	void Update()
	{
		systemBars.Update();
		OnScreenOrientationChange();
	}
	void InitBase()
	{
		Instance = this;
		canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
		main = GetComponent<RectTransform>();
	}
	void OnScreenOrientationChange()
	{
		if (lastScreenOrientation != Screen.orientation)
		{
			lastScreenOrientation = Screen.orientation;
			systemBars.OnScreenOrientationChange();
		}
	}
	static internal bool LightBackground(Color color)
	{
		return IsColorCloseToAThanB(color, Color.white, Color.black)
			|| (IsColorCloseToAThanB(color, Color.green, Color.white)
			&& IsColorCloseToAThanB(color, Color.green, Color.black));
	}
	static internal bool IsColorCloseToAThanB(Color C, Color A, Color B)
	{
		return Vector3.Distance(new Vector3(C.r, C.g, C.b), new Vector3(A.r, A.g, A.b)) < Vector3.Distance(new Vector3(C.r, C.g, C.b), new Vector3(B.r, B.g, B.b));
	}
	internal void UpdateScrollbars()
	{
		MobileScrollVertical[] scrollbars = GameObject.FindObjectsByType<MobileScrollVertical>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#if !NODEBUG
		if (debugMode && scrollbars.Length > 0)
			Debug.Log("MSANB: Update Scroll Bars");
#endif
		for (ushort i = 0; i < scrollbars.Length; i++)
		{
			scrollbars[i].UpdateScrollbar(StatusBarHeight, NavigationBarHeight);
		}
	}
	internal void UpdateStatusBarRect(float top, float right, float bottom, float left)
	{
		if (!float.IsNaN(top))
			statusBar.SetTop(top);
		if (!float.IsNaN(right))
			statusBar.SetRight(right);
		if (!float.IsNaN(bottom))
			statusBar.SetBottom(bottom);
		if (!float.IsNaN(left))
			statusBar.SetLeft(left);
	}
	internal void UpdateNavigationBarRect(float top, float right, float bottom, float left)
	{
		if (!float.IsNaN(top))
			navigationBar.SetTop(top);
		if (!float.IsNaN(right))
			navigationBar.SetRight(right);
		if (!float.IsNaN(bottom))
			navigationBar.SetBottom(bottom);
		if (!float.IsNaN(left))
			navigationBar.SetLeft(left);
	}
	internal void UpdateMainRect(float top, float right, float bottom, float left)
	{
		if (!float.IsNaN(top))
			main.SetTop(top);
		if (!float.IsNaN(right))
			main.SetRight(right);
		if (!float.IsNaN(bottom))
			main.SetBottom(bottom);
		if (!float.IsNaN(left))
			main.SetLeft(left);
	}
	/// <summary>
	/// Hide Status & Navigation bar.
	/// </summary>
	public bool Hide()
	{
		return Hide(null, false);
	}
	/// <summary>
	/// Hide Status & Navigation bar.
	/// </summary>
	public void Hide(bool force = false)
	{
		Hide(null, force);
	}
	/// <summary>
	/// Hide Status & Navigation bar.
	/// </summary>
	internal bool Hide(DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.Hide(navigationBarColor, completeCallback, false, force);
	}
	/// <summary>
	/// Hide Status & Navigation bar with color transition.
	/// </summary>
	internal bool Hide(Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.Hide(toColorTransition, completeCallback, true, force);
	}
	public bool Show()
	{
		return Show(null, false);
	}
	/// <summary>
	/// Show Status & Navigation bar with color transition.
	/// </summary>
	public bool Show(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.Show(fromColorTransition, toColorTransition, completeCallback, true, force);
	}
	/// <summary>
	/// Show Status & Navigation bar.
	/// </summary>
	public void Show(bool force = false)
	{
		Show(null, force);
	}
	/// <summary>
	/// Show Status & Navigation bar.
	/// </summary>
	public bool Show(DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.Show(navigationBarColor, navigationBarColor, completeCallback, false, force);
	}
	/// <summary>
	/// Show Status bar.
	/// </summary>
	public bool ShowStatus()
	{
		return ShowStatus(null, false);
	}
	/// <summary>
	/// Show Status bar.
	/// </summary>
	public void ShowStatus(bool force = false)
	{
		ShowStatus(null, force);
	}
	/// <summary>
	/// Show Status bar.
	/// </summary>
	public bool ShowStatus(DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.ShowStatus(statusBarColor, statusBarColor, completeCallback, false, force);
	}
	/// <summary>
	/// Show Status bar with color transition.
	/// </summary>
	public bool ShowStatus(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.ShowStatus(fromColorTransition, toColorTransition, completeCallback, true, force);
	}
	/// <summary>
	/// Hide Status bar.
	/// </summary>
	public bool HideStatus()
	{
		return HideStatus(null, false);
	}
	/// <summary>
	/// Hide Status bar.
	/// </summary>
	public void HideStatus(bool force = false)
	{
		HideStatus(null, force);
	}
	/// <summary>
	/// Hide Status bar.
	/// </summary>
	public bool HideStatus(DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.HideStatus(statusBarColor, completeCallback, false, force);
	}
	/// <summary>
	/// Hide Status bar with color transition.
	/// </summary>
	public bool HideStatus(Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.HideStatus(toColorTransition, completeCallback, true, force);
	}
	/// <summary>
	/// Show Navigation bar.
	/// </summary>
	public bool ShowNavigation()
	{
		return ShowNavigation(null, false);
	}
	/// <summary>
	/// Show Navigation bar.
	/// </summary>
	public void ShowNavigation(bool force = false)
	{
		ShowNavigation(null, force);
	}
	/// <summary>
	/// Show Navigation bar.
	/// </summary>
	public bool ShowNavigation(DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.ShowNavigation(navigationBarColor, navigationBarColor, completeCallback, false, force);
	}
	/// <summary>
	/// Show Navigation bar with color transition.
	/// </summary>
	public bool ShowNavigation(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.ShowNavigation(fromColorTransition, toColorTransition, completeCallback, true, force);
	}
	/// <summary>
	/// Hide Navigation bar.
	/// </summary>
	public bool HideNavigation()
	{
		return HideNavigation(null, false);
	}
	/// <summary>
	/// Hide Navigation bar.
	/// </summary>
	public void HideNavigation(bool force = false)
	{
		HideNavigation(null, force);
	}
	/// <summary>
	/// Hide Navigation bar.
	/// </summary>
	public bool HideNavigation(DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.HideNavigation(navigationBarColor, completeCallback, false, force);
	}
	/// <summary>
	/// Hide Navigation bar with color transition.
	/// </summary>
	public bool HideNavigation(Color toColorTransition, DelegateAction completeCallback = null, bool force = false)
	{
		return systemBars.HideNavigation(toColorTransition, completeCallback, true, force);
	}
	/// <summary>
	/// Set the colors of Status & Navigation bar to light.
	/// </summary>
	public void SetLightMode()
	{
		SetDarkModeLocal();
		SetLightMode(TextMode.Auto);
	}
	private void SetLightModeLocal()
	{
		_SetLightMode(TextMode.Auto);
	}
	/// <summary>
	/// Set the colors of Status & Navigation bar to dark.
	/// </summary>
	public void SetDarkMode()
	{
		SetLightModeLocal();
		SetDarkMode(TextMode.Auto);
	}
	private void SetDarkModeLocal()
	{
		_SetDarkMode(TextMode.Auto);
	}
	/// <summary>
	/// Set the colors of Status & Navigation bar to light with your choice of text.
	/// </summary>
	public void SetLightMode(TextMode textMode)
	{
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Set Light Mode");
#endif
		_SetLightMode(textMode);
	}
	/// <summary>
	/// Set the colors of Status & Navigation bar to light with light text.
	/// </summary>
	public void SetLightModeWithDarkContent()
	{
		SetLightMode(TextMode.Dark);
	}
	/// <summary>
	/// Set the colors of Status & Navigation bar to light with dark text.
	/// </summary>
	public void SetLightModeWithLightContent()
	{
		SetLightMode(TextMode.Light);
	}
	/// <summary>
	/// Set the colors of Status & Navigation bar to dark with dark text.
	/// </summary>
	public void SetDarkModeWithLightContent()
	{
		SetDarkMode(TextMode.Light);
	}
	/// <summary>
	/// Set the colors of Status & Navigation bar to dark with light text.
	/// </summary>
	public void SetDarkModeWithDarkContent()
	{
		SetDarkMode(TextMode.Dark);
	}
	/// <summary>
	/// Set the colors of Status & Navigation bar to light with your choice of text mode.
	/// </summary>
	public void SetDarkMode(TextMode textMode)
	{
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Set Dark Mode");
#endif
		_SetDarkMode(textMode);
	}

	private void _SetLightMode(TextMode textMode)
	{
		statusBarColor = statusLightModeBackgroundColor;
		navigationBarColor = navigationLightModeBackgroundColor;
		UpdateColor(textMode);
	}
	private void _SetDarkMode(TextMode textMode)
	{
		statusBarColor = statusDarkModeBackgroundColor;
		navigationBarColor = navigationDarkModeBackgroundColor;
		UpdateColor(textMode);
	}
	internal void UpdateColor(TextMode textMode = TextMode.Auto)
	{
		UpdateStatusColor(textMode);
		UpdateNavigationColor(textMode);
	}
	internal void UpdateStatusColor(TextMode statusBackgroundMode = TextMode.Auto)
	{
		systemBars.UpdateStatusColor(statusBarColor, statusBackgroundMode);
	}
	internal void UpdateNavigationColor(TextMode navigationTextMode = TextMode.Auto)
	{
		systemBars.UpdateNavigationColor(navigationBarColor, navigationTextMode);
	}
	/// <summary>
	/// Change the color of bars in light with your choice of text mode.
	/// </summary>
	public void ChangeLightModeColorTemplate(Color color)
	{
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Change Light Mode Color Template");
#endif
		statusLightModeBackgroundColor = color;
		navigationLightModeBackgroundColor = color;
	}
	/// <summary>
	/// Change the color of bars in dark with your choice of text mode.
	/// </summary>
	public void ChangeDarkModeColorTemplate(Color color)
	{
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Change Dark Mode Color Template");
#endif
		statusDarkModeBackgroundColor = color;
		navigationDarkModeBackgroundColor = color;
	}
	/// <summary>
	/// Set the color of Status bar to light with dark text.
	/// </summary>
	public void SetStatusLightModeWithDarkContent()
	{
		SetStatusLightMode(TextMode.Dark);
	}
	/// <summary>
	/// Set the color of Status bar to light with light text.
	/// </summary>
	public void SetStatusLightModeWithLightContent()
	{
		SetStatusLightMode(TextMode.Light);
	}
	/// <summary>
	/// Set the color of Status bar to dark with light text.
	/// </summary>
	public void SetStatusDarkModeWithLightContent()
	{
		SetStatusDarkMode(TextMode.Light);
	}
	/// <summary>
	/// Set the color of Status bar to dark with dark text.
	/// </summary>
	public void SetStatusDarkModeWithDarkContent()
	{
		SetStatusDarkMode(TextMode.Dark);
	}
	/// <summary>
	/// Set the color of Status bar to light.
	/// </summary>
	public void SetStatusLightMode()
	{
		SetStatusDarkModeLocal();
		SetStatusLightMode(TextMode.Auto);
	}
	private void SetStatusLightModeLocal()
	{
		SetStatusLightMode(TextMode.Auto);
	}
	/// <summary>
	/// Change the color of Status bar in light with your choice of text mode.
	/// </summary>
	public void ChangeStatusLightModeColorTemplate(Color color)
	{
		statusLightModeBackgroundColor = color;
	}
	/// <summary>
	/// Change the color of Status bar in dark with your choice of text mode.
	/// </summary>
	public void ChangeStatusDarkModeColorTemplate(Color color)
	{
		statusDarkModeBackgroundColor = color;
	}
	/// <summary>
	/// Set the color of Status bar to light with your choice of text mode.
	/// </summary>
	public void SetStatusLightMode(TextMode statusTextMode)
	{
		SetStatusColor(statusLightModeBackgroundColor, statusTextMode);
	}
	/// <summary>
	/// Set the color of Status bar to dark.
	/// </summary>
	public void SetStatusDarkMode()
	{
		SetStatusLightModeLocal();
		SetStatusDarkMode(TextMode.Auto);
	}
	private void SetStatusDarkModeLocal()
	{
		SetStatusDarkMode(TextMode.Auto);
	}
	/// <summary>
	/// Set the color of Status bar to dark with your choice of text mode.
	/// </summary>
	public void SetStatusDarkMode(TextMode statusTextMode)
	{
		SetStatusColor(statusDarkModeBackgroundColor, statusTextMode);
	}
	/// <summary>
	/// Set the color of Status bar with your choice of text mode.
	/// </summary>
	public void SetStatusColor(Color color, TextMode statusTextMode = TextMode.Auto, bool force = true)
	{
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Set Status Bar Color");
#endif
		statusBarColor = color;
		if (force)
			UpdateStatusColor(statusTextMode);
	}
	/// <summary>
	/// Set the color of Navigation bar to light with dark text.
	/// </summary>
	public void SetNavigationLightModeWithDarkContent()
	{
		SetNavigationLightMode(TextMode.Dark);
	}
	/// <summary>
	/// Set the color of Navigation bar to light with light text.
	/// </summary>
	public void SetNavigationLightModeWithLightContent()
	{
		SetNavigationLightMode(TextMode.Light);
	}
	/// <summary>
	/// Set the color of Navigation bar to dark with light text.
	/// </summary>
	public void SetNavigationDarkModeWithLightContent()
	{
		SetNavigationDarkMode(TextMode.Light);
	}
	/// <summary>
	/// Set the color of Navigation bar to dark with dark text.
	/// </summary>
	public void SetNavigationDarkModeWithDarkContent()
	{
		SetNavigationDarkMode(TextMode.Dark);
	}
	/// <summary>
	/// Set the color of Navigation bar to light mode.
	/// </summary>
	public void SetNavigationLightMode()
	{
		SetNavigationDarkModeLocal();
		SetNavigationLightMode(TextMode.Auto);
	}
	private void SetNavigationLightModeLocal()
	{
		SetNavigationLightMode(TextMode.Auto);
	}
	/// <summary>
	/// Set the color of Navigation bar to dark mode.
	/// </summary>
	public void SetNavigationDarkMode()
	{
		SetNavigationLightModeLocal();
		SetNavigationDarkMode(TextMode.Auto);
	}
	private void SetNavigationDarkModeLocal()
	{
		SetNavigationDarkMode(TextMode.Auto);
	}
	/// <summary>
	/// Change the color of Navigation bar in light with your choice of text mode.
	/// </summary>
	public void ChangeNavigationLightModeColorTemplate(Color color)
	{
		navigationLightModeBackgroundColor = color;
	}
	/// <summary>
	/// Change the color of Navigation bar in dark with your choice of text mode.
	/// </summary>
	public void ChangeNavigationDarkModeColorTemplate(Color color)
	{
		navigationDarkModeBackgroundColor = color;
	}
	/// <summary>
	/// Set the color of Navigation bar to light with your choice of text mode.
	/// </summary>
	public void SetNavigationLightMode(TextMode navigationTextMode = TextMode.Auto)
	{
		SetNavigationColor(navigationLightModeBackgroundColor, navigationTextMode);
	}
	/// <summary>
	/// Set the color of Navigation bar to dark with your choice of text mode.
	/// </summary>
	public void SetNavigationDarkMode(TextMode navigationTextMode = TextMode.Auto)
	{
		SetNavigationColor(navigationDarkModeBackgroundColor, navigationTextMode);
	}
	/// <summary>
	/// Set the color of Navigation bar with your choice of text mode.
	/// </summary>
	public void SetNavigationColor(Color color, TextMode navigationTextMode = TextMode.Auto, bool force = true)
	{
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Set Navigation Bar Color");
#endif
		navigationBarColor = color;
		if (force)
			UpdateNavigationColor(navigationTextMode);
	}
	/// <summary>
	/// Set the colors of Status & Navigation bar.
	/// </summary>
	public void SetColor(Color color, TextMode textMode = TextMode.Auto)
	{
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Set Color");
#endif
		statusBarColor = color;
		navigationBarColor = color;
		UpdateColor(textMode);
	}
	byte transitionIndexStatus = 0;
	byte transitionIndexNavigation = 0;
	/// <summary>
	/// Color transition of Status & Navigation bar.
	/// </summary>
	public void TransitionColor(Color color, float seconds, TextMode textMode = TextMode.Auto)
	{
		transitionIndexStatus++;
		transitionIndexNavigation++;
		if (seconds <= 0f)
		{
			SetColor(color, textMode);
			return;
		}
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Start Color Transition");
#endif
		StartCoroutine(TransitionStatusColor(color, seconds, textMode, transitionIndexStatus));
		StartCoroutine(TransitionNavigationColor(color, seconds, textMode, transitionIndexNavigation));
	}
	/// <summary>
	/// Color transition of Status bar.
	/// </summary>
	public void TransitionStatusColor(Color color, float seconds, TextMode textMode = TextMode.Auto)
	{
		transitionIndexStatus++;
		if (seconds <= 0f)
		{
			SetStatusColor(color, textMode);
			return;
		}
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Start Status Color Transition");
#endif
		StartCoroutine(TransitionStatusColor(color, seconds, textMode, transitionIndexStatus));
	}
	/// <summary>
	/// Color transition of Navigation bar.
	/// </summary>
	public void TransitionNavigationColor(Color color, float seconds, TextMode textMode = TextMode.Auto)
	{
		transitionIndexNavigation++;
		if (seconds <= 0f)
		{
			SetNavigationColor(color, textMode);
			return;
		}
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Start Navigation Color Transition");
#endif
		StartCoroutine(TransitionNavigationColor(color, seconds, textMode, transitionIndexNavigation));
	}
	/// <summary>
	/// Color transition of Status & Navigation bar to dark mode.
	/// </summary>
	public void TransitionDarkMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
	{
		transitionIndexStatus++;
		transitionIndexNavigation++;
		if (seconds <= 0f)
		{
			SetStatusColor(statusDarkModeBackgroundColor, textMode);
			SetNavigationColor(navigationDarkModeBackgroundColor, textMode);
			return;
		}
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Start Color Transition To Dark Mode");
#endif
		StartCoroutine(TransitionStatusColor(statusDarkModeBackgroundColor, seconds, textMode, transitionIndexStatus));
		StartCoroutine(TransitionNavigationColor(navigationDarkModeBackgroundColor, seconds, textMode, transitionIndexNavigation));
	}
	/// <summary>
	/// Color transition of Status bar to dark mode.
	/// </summary>
	public void TransitionStatusDarkMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
	{
		transitionIndexStatus++;
		if (seconds <= 0f)
		{
			SetStatusColor(statusDarkModeBackgroundColor, textMode);
			return;
		}
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Start Status Bar Color Transition To Dark Mode");
#endif
		StartCoroutine(TransitionStatusColor(statusDarkModeBackgroundColor, seconds, textMode, transitionIndexStatus));
	}
	/// <summary>
	/// Color transition of Navigation bar to dark mode.
	/// </summary>
	public void TransitionNavigationDarkMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
	{
		transitionIndexNavigation++;
		if (seconds <= 0f)
		{
			SetNavigationColor(navigationDarkModeBackgroundColor, textMode);
			return;
		}
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Start Navigation Bar Color Transition To Dark Mode");
#endif
		StartCoroutine(TransitionNavigationColor(navigationDarkModeBackgroundColor, seconds, textMode, transitionIndexNavigation));
	}
	/// <summary>
	/// Color transition of Status & Navigation bar to light mode.
	/// </summary>
	public void TransitionLightMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
	{
		transitionIndexStatus++;
		transitionIndexNavigation++;
		if (seconds <= 0f)
		{
			SetStatusColor(statusLightModeBackgroundColor, textMode);
			SetNavigationColor(navigationLightModeBackgroundColor, textMode);
			return;
		}
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Start Color Transition To Light Mode");
#endif
		StartCoroutine(TransitionStatusColor(statusLightModeBackgroundColor, seconds, textMode, transitionIndexStatus));
		StartCoroutine(TransitionNavigationColor(navigationLightModeBackgroundColor, seconds, textMode, transitionIndexNavigation));
	}
	/// <summary>
	/// Color transition of Status bar to light mode.
	/// </summary>
	public void TransitionStatusLightMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
	{
		transitionIndexStatus++;
		if (seconds <= 0f)
		{
			SetStatusColor(statusLightModeBackgroundColor, textMode);
			return;
		}
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Start Status Bar Color Transition To Light Mode");
#endif
		StartCoroutine(TransitionStatusColor(statusLightModeBackgroundColor, seconds, textMode, transitionIndexStatus));
	}
	/// <summary>
	/// Color transition of Navigation bar to light mode.
	/// </summary>
	public void TransitionNavigationLightMode(float seconds = 0.5f, TextMode textMode = TextMode.Auto)
	{
		transitionIndexNavigation++;
		if (seconds <= 0f)
		{
			SetNavigationColor(navigationLightModeBackgroundColor, textMode);
			return;
		}
#if !NODEBUG
		if (debugMode)
			Debug.Log("MSANB: Start Navigation Bar Color Transition To Light Mode");
#endif
		StartCoroutine(TransitionNavigationColor(navigationLightModeBackgroundColor, seconds, textMode, transitionIndexNavigation));
	}
	private IEnumerator TransitionStatusColor(Color color, float seconds, TextMode textMode, byte initIndex)
	{
		Color a_status = statusBarColor;
		for (float i = 0; i < seconds; i += 0.025f)
		{
			yield return new WaitForSeconds(0.025f);
			if (initIndex != transitionIndexStatus)
				yield break;
			statusBarColor = Color.Lerp(a_status, color, i / seconds);
			UpdateStatusColor(textMode);
		}
		yield return new WaitForSeconds(0.025f);
		if (initIndex != transitionIndexStatus)
			yield break;
		statusBarColor = color;
		UpdateStatusColor(textMode);
	}
	private IEnumerator TransitionNavigationColor(Color color, float seconds, TextMode textMode, byte initIndex)
	{
		Color a_navigation = navigationBarColor;
		for (float i = 0; i < seconds; i += 0.025f)
		{
			yield return new WaitForSeconds(0.025f);
			if (initIndex != transitionIndexNavigation)
				yield break;
			navigationBarColor = Color.Lerp(a_navigation, color, i / seconds);
			UpdateNavigationColor(textMode);
		}
		yield return new WaitForSeconds(0.025f);
		if (initIndex != transitionIndexNavigation)
			yield break;
		navigationBarColor = color;
		UpdateNavigationColor(textMode);
	}
	public void CheckVisibility()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		((AndroidSystemBars)systemBars).CheckVisibility();
#endif
	}
}