using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace SaariTech.UI
{
	internal class OtherSystemBars : SystemBarsBase
	{
		static MobileStatusNavigationBar instance
		{
			get
			{
				return MobileStatusNavigationBar.Instance;
			}
		}
		[SerializeField]
		private Text previewText;
		private Text previewTime;
		internal Button preveiwAndroidNavigationBack;
		internal Image preveiwAndroidNavigationHome;
		internal Image preveiwAndroidNavigationOverview;
		internal Image preveiwIOSHome;
		internal static bool _IsReadyStatusBar { get; private set; }
		internal static bool _IsReadyNavigationBar { get; private set; }
		internal static float StatusBarHeightValue;
		internal static float StatusBarHeight
		{ 
			get
			{
				return StatusBarHeightValue;
			} 
			private set
			{
				StatusBarHeightValue = value;
				if(instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Android
#if UNITY_ANDROID
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					if(instance.canvasRectTransform.sizeDelta.y > instance.canvasRectTransform.sizeDelta.x)
					{
						instance.UpdateStatusBarRect(
							0f,
							0f,
							instance.canvasRectTransform.sizeDelta.y - StatusBarHeight,
							0f
						);
						instance.UpdateMainRect(
							StatusBarHeight,
							0f,
							float.NaN,
							0f
						);
					}
					else
					{
						instance.UpdateStatusBarRect(
							0f,
							0f,
							instance.canvasRectTransform.sizeDelta.y - StatusBarHeight,
							0f
						);
						instance.UpdateNavigationBarRect(
							StatusBarHeight,
							0f,
							0f,
							instance.canvasRectTransform.sizeDelta.x - NavigationBarHeight
						);
						instance.UpdateMainRect(
							StatusBarHeight,
							float.NaN,
							0f,
							0f
						);
					}
				}
				else if(instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.iOS
#if UNITY_IOS
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					if(instance.canvasRectTransform.sizeDelta.y < instance.canvasRectTransform.sizeDelta.x)
						StatusBarHeightValue = 0f;

					instance.UpdateStatusBarRect(
						0f,
						0f,
						instance.canvasRectTransform.sizeDelta.y - StatusBarHeight,
						0f
					);
					instance.UpdateMainRect(
						StatusBarHeight,
						0f,
						float.NaN,
						0f
					);
				}
			} 
		}
		internal static float StatusBarHeight_STATIC { get; private set; }
		static float NavigationBarHeightValue;
		internal static float NavigationBarHeight 
		{
			get
			{
				return NavigationBarHeightValue;
			} 
			private set
			{
				NavigationBarHeightValue = value;
				
				 if(instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Android
#if UNITY_ANDROID
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationBack.GetComponent<RectTransform>().sizeDelta = new Vector2(NavigationBarHeight, NavigationBarHeight);
					((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationHome.rectTransform.sizeDelta = new Vector2(NavigationBarHeight, NavigationBarHeight);
					((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationOverview.rectTransform.sizeDelta = new Vector2(NavigationBarHeight, NavigationBarHeight);
					if(instance.canvasRectTransform.sizeDelta.y > instance.canvasRectTransform.sizeDelta.x)
					{
						instance.UpdateNavigationBarRect(
							instance.canvasRectTransform.sizeDelta.y - NavigationBarHeight,
							0f,
							0f,
							0f
						);
						instance.UpdateMainRect(
							float.NaN,
							0f,
							NavigationBarHeight,
							0f
						);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationBack.GetComponent<RectTransform>().anchoredPosition = new Vector2(
							-instance.canvasRectTransform.sizeDelta.x / 4f,
							0f
						);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationOverview.rectTransform.anchoredPosition = new Vector2(
							instance.canvasRectTransform.sizeDelta.x / 4f,
							0f
						);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationBack.gameObject.SetActive(true);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationHome.gameObject.SetActive(true);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationOverview.gameObject.SetActive(true);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwIOSHome.gameObject.SetActive(false);
					}
					else
					{
						instance.UpdateNavigationBarRect(
							StatusBarHeight,
							0f,
							0f,
							instance.canvasRectTransform.sizeDelta.x - NavigationBarHeight
						);
						instance.UpdateMainRect(
							float.NaN,
							NavigationBarHeight,
							0f,
							0f
						);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationBack.GetComponent<RectTransform>().anchoredPosition = new Vector2(
							0f,
							-instance.canvasRectTransform.sizeDelta.y / 4f
						);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationOverview.rectTransform.anchoredPosition = new Vector2(
							0f,
							instance.canvasRectTransform.sizeDelta.y / 4f
						);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationBack.gameObject.SetActive(true);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationHome.gameObject.SetActive(true);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationOverview.gameObject.SetActive(true);
						((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwIOSHome.gameObject.SetActive(false);
					}
				}
				else if(instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.iOS
#if UNITY_IOS
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					NavigationBarHeightValue *= (instance.iOSHideHomeBarInLandscape && instance.canvasRectTransform.sizeDelta.y < instance.canvasRectTransform.sizeDelta.x) ? 0f : 1f;
					NavigationBarHeightValue *= instance.iOSHideHomeBar ? 0f : 1f;

					instance.UpdateNavigationBarRect(
						instance.canvasRectTransform.sizeDelta.y - NavigationBarHeight,
						0f,
						0f,
						0f
					);
					instance.UpdateMainRect(
						float.NaN,
						0f,
						NavigationBarHeight,
						0f
					);
					RectTransform homeBarRectTransform = ((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwIOSHome.rectTransform;
					homeBarRectTransform.sizeDelta = new Vector2(
						(instance.canvasRectTransform.sizeDelta.y > instance.canvasRectTransform.sizeDelta.x 
							? instance.canvasRectTransform.sizeDelta.x * 0.5f
							: instance.canvasRectTransform.sizeDelta.y * 0.5f)
						- NavigationBarHeight,
						NavigationBarHeight * 0.1976f
					);
					homeBarRectTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(
						NavigationBarHeight * 0.1976f,
						NavigationBarHeight * 0.1976f
					);
					homeBarRectTransform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(
						NavigationBarHeight * 0.1976f,
						NavigationBarHeight * 0.1976f
					);
					((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationBack.gameObject.SetActive(false);
					((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationHome.gameObject.SetActive(false);
					((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwAndroidNavigationOverview.gameObject.SetActive(false);
					((OtherSystemBars)MobileStatusNavigationBar.systemBars).preveiwIOSHome.gameObject.SetActive(true);
				}
			}
		}
		internal static float NavigationBarHeight_STATIC { get; private set; }
		float HideStatusSpeed;
		float HideNavigationSpeed;
		float HideStatusWaitSeconds;
		float HideNavigationWaitSeconds;
		float ShowStatusSpeed;
		float ShowNavigationSpeed;
		float ShowStatusWaitSeconds;
		float ShowNavigationWaitSeconds;
		internal override void OnDisable() {}
		internal override void OnApplicationFocus(bool focus) {}
		Vector2 oldCanvasSize = Vector2.zero;
		MobileStatusNavigationBar.VirtualDevice virtualDevice = MobileStatusNavigationBar.VirtualDevice.Auto;
		internal override void Update()
		{
			previewTime.text = System.DateTime.Now.ToString("HH:mm:ss");
			if(oldCanvasSize != instance.canvasRectTransform.sizeDelta || virtualDevice != instance.virtualDevice)
			{
				oldCanvasSize = instance.canvasRectTransform.sizeDelta;
				virtualDevice = instance.virtualDevice;
				Refresh(true);
			}
		}
		internal override void Refresh(bool force = false)
		{
			previewText.fontSize = Mathf.RoundToInt(StatusBarHeight_STATIC / 2f);
			RectTransform previewTextRectTransform = previewText.GetComponent<RectTransform>();
			previewTextRectTransform.SetRight(previewTextRectTransform.parent.GetComponent<RectTransform>().rect.width / 2f);
			previewTextRectTransform.SetLeft(StatusBarHeight_STATIC / 2f - previewText.fontSize / 2f);
			previewTextRectTransform.localScale = Vector3.one;
			previewTextRectTransform.SetTop(0f);
			previewTextRectTransform.SetBottom(0f);
			previewTextRectTransform.localPosition = new Vector3(previewTextRectTransform.localPosition.x, previewTextRectTransform.localPosition.y, 0f);
			previewTime.fontSize = Mathf.RoundToInt(StatusBarHeight_STATIC / 2f);
			RectTransform previewTimeRectTransform = previewTime.GetComponent<RectTransform>();
			previewTimeRectTransform.SetLeft(previewTimeRectTransform.parent.GetComponent<RectTransform>().rect.width / 2f);
			previewTimeRectTransform.SetRight(StatusBarHeight_STATIC / 2f - previewTime.fontSize / 2f);
			previewTimeRectTransform.localScale = Vector3.one;
			previewTimeRectTransform.SetTop(0f);
			previewTimeRectTransform.SetBottom(0f);
			previewTimeRectTransform.localPosition = new Vector3(previewTimeRectTransform.localPosition.x, previewTimeRectTransform.localPosition.y, 0f);
			UpdateStatusBar(true);
			UpdateNavigationBar(true);
		}
		internal override void OnScreenOrientationChange() {}
		internal bool inited = false;
		internal override void Awake()
		{
			Init(true);
		}
		internal override void Init(bool force = false)
		{
			if (inited && !force)
				return;
			float scale = Screen.height / instance.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.y;
			HideStatusSpeed = Mathf.Pow(1.337f, 2.5f) * Mathf.PI;
			HideNavigationSpeed = Mathf.Pow(1.337f, 2.5f) * Mathf.PI;
			HideStatusWaitSeconds = 0.25f;
			HideNavigationWaitSeconds = 0.25f;
			ShowStatusSpeed = 7.5f;
			ShowNavigationSpeed = 7.5f;
			ShowStatusWaitSeconds = 0f;
			ShowNavigationWaitSeconds = 0f;
			StatusBarHeight_STATIC = 88f;
			NavigationBarHeight_STATIC = 88f;
			if (previewText == null)
			{
				previewText = new GameObject("Example Text", typeof(RectTransform)).AddComponent<Text>();
				previewText.gameObject.transform.SetParent(instance.statusBar.transform);
				previewText.font = Resources.Load<Font>("Saari.Tech/LiberationSans");
				previewText.text = string.IsNullOrWhiteSpace(instance.previewStatusLabel) ? "Editor" : instance.previewStatusLabel;
				previewText.alignment = TextAnchor.MiddleLeft;
				previewText.resizeTextForBestFit = true;
				RectTransform rectText = previewText.GetComponent<RectTransform>();
				rectText.anchorMin = new Vector2(0f, 0f);
				rectText.anchorMax = new Vector2(1f, 1f);
			}
			if (previewTime == null)
			{
				previewTime = new GameObject("Preview Time", typeof(RectTransform)).AddComponent<Text>();
				previewTime.gameObject.transform.SetParent(instance.statusBar.transform);
				previewTime.font = Resources.Load<Font>("Saari.Tech/LiberationSans");
				previewTime.text = "";
				previewTime.alignment = TextAnchor.MiddleRight;
				previewTime.resizeTextForBestFit = true;
				RectTransform rectText = previewTime.GetComponent<RectTransform>();
				rectText.anchorMin = new Vector2(0f, 0f);
				rectText.anchorMax = new Vector2(1f, 1f);
			}
			if (preveiwAndroidNavigationBack == null)
			{
				preveiwAndroidNavigationBack = new GameObject("Preview Android Back", typeof(RectTransform)).AddComponent<Button>();
				preveiwAndroidNavigationBack.gameObject.transform.SetParent(instance.navigationBar.transform);
				preveiwAndroidNavigationBack.gameObject.AddComponent<Image>();
				RectTransform rectButton = preveiwAndroidNavigationBack.GetComponent<RectTransform>();
				rectButton.anchorMin = new Vector2(0.5f, 0.5f);
				rectButton.anchorMax = new Vector2(0.5f, 0.5f);
				rectButton.localPosition = Vector3.zero;
				rectButton.localScale = Vector3.one;
				preveiwAndroidNavigationBack.onClick.AddListener(instance.backButtonOnClick.Invoke);
				preveiwAndroidNavigationBack.gameObject.GetComponent<Image>().sprite = 
					Resources.Load<Sprite>("Saari.Tech/Sprites/Android/MSANB_arrow");
			}
			if (preveiwAndroidNavigationHome == null)
			{
				preveiwAndroidNavigationHome = new GameObject("Preview Android Home", typeof(RectTransform)).AddComponent<Image>();
				preveiwAndroidNavigationHome.gameObject.transform.SetParent(instance.navigationBar.transform);
				RectTransform rectButton = preveiwAndroidNavigationHome.GetComponent<RectTransform>();
				rectButton.anchorMin = new Vector2(0.5f, 0.5f);
				rectButton.anchorMax = new Vector2(0.5f, 0.5f);
				rectButton.localPosition = Vector3.zero;
				rectButton.localScale = Vector3.one;
				preveiwAndroidNavigationHome.sprite = 
					Resources.Load<Sprite>("Saari.Tech/Sprites/Android/MSANB_circle");
			}
			if (preveiwAndroidNavigationOverview == null)
			{
				preveiwAndroidNavigationOverview = new GameObject("Preview Android Overview", typeof(RectTransform)).AddComponent<Image>();
				preveiwAndroidNavigationOverview.gameObject.transform.SetParent(instance.navigationBar.transform);
				RectTransform rectButton = preveiwAndroidNavigationOverview.GetComponent<RectTransform>();
				rectButton.anchorMin = new Vector2(0.5f, 0.5f);
				rectButton.anchorMax = new Vector2(0.5f, 0.5f);
				rectButton.localPosition = Vector3.zero;
				rectButton.localScale = Vector3.one;
				preveiwAndroidNavigationOverview.sprite = 
					Resources.Load<Sprite>("Saari.Tech/Sprites/Android/MSANB_square");
			}
			if (preveiwIOSHome == null)
			{
				preveiwIOSHome = new GameObject("Preview iOS Home", typeof(RectTransform)).AddComponent<Image>();
				preveiwIOSHome.gameObject.transform.SetParent(instance.navigationBar.transform);
				RectTransform rectButton = preveiwIOSHome.GetComponent<RectTransform>();
				rectButton.anchorMin = new Vector2(0.5f, 0.337f);
				rectButton.anchorMax = new Vector2(0.5f, 0.337f);
				rectButton.localPosition = Vector3.zero;
				rectButton.localScale = Vector3.one;
				rectButton.anchoredPosition = Vector2.zero;
				Image leftCircle = new GameObject("Left Half Circle", typeof(RectTransform)).AddComponent<Image>();
				leftCircle.gameObject.transform.SetParent(preveiwIOSHome.transform);
				RectTransform leftRectButton = leftCircle.GetComponent<RectTransform>();
				leftRectButton.anchorMin = new Vector2(0f, 0.5f);
				leftRectButton.anchorMax = new Vector2(0f, 0.5f);
				leftRectButton.localPosition = Vector3.zero;
				leftRectButton.localScale = Vector3.one;
				leftRectButton.anchoredPosition = Vector2.zero;
				leftCircle.sprite = 
					Resources.Load<Sprite>("Saari.Tech/Sprites/iOS/MSANB_halfcircle_left");
				Image rightCircle = new GameObject("Right Half Circle", typeof(RectTransform)).AddComponent<Image>();
				rightCircle.gameObject.transform.SetParent(preveiwIOSHome.transform);
				RectTransform rightRectButton = rightCircle.GetComponent<RectTransform>();
				rightRectButton.anchorMin = new Vector2(1f, 0.5f);
				rightRectButton.anchorMax = new Vector2(1f, 0.5f);
				rightRectButton.localPosition = Vector3.zero;
				rightRectButton.localScale = Vector3.one;
				rightRectButton.anchoredPosition = Vector2.zero;
				rightCircle.sprite = 
					Resources.Load<Sprite>("Saari.Tech/Sprites/iOS/MSANB_halfcircle_right");
			}
			Refresh();
#if !NODEBUG || UNITY_EDITOR
			if (instance.debugMode)
			{
				if (inited)
					Debug.Log("MSANB: Reinitialized");
				else
					Debug.Log("MSANB: Initialized");
			}
#endif
			if (inited)
				return;
			if (instance.startLightMode)
				instance.SetStatusLightMode();
			else
				instance.SetStatusDarkMode();
			if (instance.startLightMode)
				instance.SetNavigationLightMode();
			else
				instance.SetNavigationDarkMode();
			instance.UpdateColor();
			inited = true;
			_IsReadyStatusBar = true;
			_IsReadyNavigationBar = true;
		}
		internal override bool Hide(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((instance.displayStatusBar && _IsReadyStatusBar) || (instance.displayNavigtionBar && _IsReadyNavigationBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Hide Bars");
#endif
				bool _displayStatusBar = instance.displayStatusBar;
				bool _displayNavigtionBar = instance.displayNavigtionBar;
				instance.displayStatusBar = false;
				instance.displayNavigtionBar = false;
				_IsReadyStatusBar = false;
				_IsReadyNavigationBar = false;
				if (instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Android
#if UNITY_ANDROID
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					if (_displayStatusBar || force)
					{
						instance.indexStatus++;
						if (_displayStatusBar && !force)
						{
							startStatus = 1f;
							targetStatus = 0f;
							instance.StartCoroutine(AnimateStatus(HideStatusSpeed, HideStatusWaitSeconds, instance.indexStatus, completeCallback, instance.statusBarColor, useColorTransition ? toColorTransition : instance.statusBarColor, useColorTransition));
						}
						else if (force)
						{
							UpdateStatusBar(force);
							_IsReadyStatusBar = true;
						}
						else
							_IsReadyStatusBar = true;
					}
					else
						_IsReadyStatusBar = true;
					if (_displayNavigtionBar || force)
					{
						instance.indexNavigation++;
						if (_displayNavigtionBar && !force)
						{
							startNavigation = 1f;
							targetNavigation = 0f;
							instance.StartCoroutine(AnimateNavigation(HideNavigationSpeed, HideNavigationWaitSeconds, instance.indexNavigation, completeCallback, instance.navigationBarColor, useColorTransition ? toColorTransition : instance.navigationBarColor, useColorTransition));
						}
						else if (force)
						{
							UpdateNavigationBar(force);
							_IsReadyNavigationBar = true;
						}
						else
							_IsReadyNavigationBar = true;
					}
					else
						_IsReadyNavigationBar = true;
				}
				else
				{
					if (useColorTransition)
						instance.SetColor(toColorTransition);
					StatusBarHeight = 0f;
					NavigationBarHeight = 0f;
					UpdateStatusBar(force);
					UpdateNavigationBar(force);
					_IsReadyStatusBar = true;
					_IsReadyNavigationBar = true;
					if (completeCallback != null)
						completeCallback();
				}
				return true;
			}
			return false;
		}
		internal override bool HideStatus(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((instance.displayStatusBar && _IsReadyStatusBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Hide Status Bar");
#endif
				bool _displayStatusBar = instance.displayStatusBar;
				instance.displayStatusBar = false;
				_IsReadyStatusBar = false;
				if (instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Android
#if UNITY_ANDROID
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					if (_displayStatusBar || force)
					{
						instance.indexStatus++;
						if (_displayStatusBar && !force)
						{
							startStatus = 1f;
							targetStatus = 0f;
							instance.StartCoroutine(AnimateStatus(HideStatusSpeed, HideStatusWaitSeconds, instance.indexStatus, completeCallback, instance.statusBarColor, useColorTransition ? toColorTransition : instance.statusBarColor, useColorTransition));
						}
						else if (force)
						{
							UpdateStatusBar(force);
							_IsReadyStatusBar = true;
						}
						else
							_IsReadyStatusBar = true;
					}
					else
						_IsReadyStatusBar = true;
				}
				else
				{
					if (useColorTransition)
						instance.SetStatusColor(toColorTransition);
					StatusBarHeight = 0f;
					UpdateStatusBar(force);
					_IsReadyStatusBar = true;
					if (completeCallback != null)
						completeCallback();
				}
				return true;
			}
			return false;
		}
		internal override bool HideNavigation(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((instance.displayNavigtionBar && _IsReadyNavigationBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Hide Navigation Bar");
#endif
				bool _displayNavigtionBar = instance.displayNavigtionBar;
				instance.displayNavigtionBar = false;
				_IsReadyNavigationBar = false;
				if (instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Android
#if UNITY_ANDROID
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					if (_displayNavigtionBar || force)
					{
						instance.indexNavigation++;
						if (_displayNavigtionBar && !force)
						{
							startNavigation = 1f;
							targetNavigation = 0f;
							instance.StartCoroutine(AnimateNavigation(HideNavigationSpeed, HideNavigationWaitSeconds, instance.indexNavigation, completeCallback, instance.navigationBarColor, useColorTransition ? toColorTransition : instance.navigationBarColor, useColorTransition));
						}
						else if (force)
						{
							UpdateNavigationBar(force);
							_IsReadyNavigationBar = true;
						}
						else
							_IsReadyNavigationBar = true;
					}
					else
						_IsReadyNavigationBar = true;
				}
				else
				{
					if (useColorTransition)
						instance.SetNavigationColor(toColorTransition);
					NavigationBarHeight = 0f;
					UpdateNavigationBar(force);
					_IsReadyNavigationBar = true;
					if (completeCallback != null)
						completeCallback();
				}
				return true;
			}
			return false;
		}
		internal override bool Show(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((!instance.displayStatusBar && _IsReadyStatusBar) || (!instance.displayNavigtionBar && _IsReadyNavigationBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Show Bars");
#endif
				bool _displayStatusBar = instance.displayStatusBar;
				bool _displayNavigtionBar = instance.displayNavigtionBar;
				instance.displayNavigtionBar = true;
				instance.displayStatusBar = true;
				_IsReadyStatusBar = false;
				_IsReadyNavigationBar = false;
				if (instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Android
#if UNITY_ANDROID
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					if (!_displayStatusBar || force)
					{
						instance.indexStatus++;
						if (!_displayStatusBar && !force)
						{
							startStatus = 0f;
							targetStatus = 1f;
							instance.StartCoroutine(AnimateStatus(ShowStatusSpeed, ShowStatusWaitSeconds, instance.indexStatus, completeCallback, fromColorTransition, useColorTransition ? toColorTransition : instance.statusBarColor, useColorTransition));
						}
						else if (force)
						{
							UpdateStatusBar(force);
							_IsReadyStatusBar = true;
						}
						else
							_IsReadyStatusBar = true;
					}
					else
						_IsReadyStatusBar = true;
					if (!_displayNavigtionBar || force)
					{
						instance.indexNavigation++;
						if (!_displayNavigtionBar && !force)
						{
							startNavigation = 0f;
							targetNavigation = 1f;
							instance.StartCoroutine(AnimateNavigation(ShowNavigationSpeed, ShowNavigationWaitSeconds, instance.indexNavigation, completeCallback, fromColorTransition, useColorTransition ? toColorTransition : instance.navigationBarColor, useColorTransition));
						}
						else if (force)
						{
							UpdateNavigationBar(force);
							_IsReadyNavigationBar = true;
						}
						else
							_IsReadyNavigationBar = true;
					}
					else
						_IsReadyNavigationBar = true;
				}
				else
				{
					if (useColorTransition)
						instance.SetColor(toColorTransition);
					StatusBarHeight = StatusBarHeight_STATIC;
					NavigationBarHeight = NavigationBarHeight_STATIC;
					_IsReadyStatusBar = true;
					_IsReadyNavigationBar = true;
					UpdateStatusBar(force);
					UpdateNavigationBar(force);
					if (completeCallback != null)
						completeCallback();
				}
				return true;
			}
			return false;
		}
		internal override bool ShowStatus(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((!instance.displayStatusBar && _IsReadyStatusBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Show Status Bar");
#endif
				bool _displayStatusBar = instance.displayStatusBar;
				instance.displayStatusBar = true;
				_IsReadyStatusBar = false;
				if (instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Android
#if UNITY_ANDROID
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					if (!_displayStatusBar || force)
					{
						instance.indexStatus++;
						if (!_displayStatusBar && !force)
						{
							startStatus = 0f;
							targetStatus = 1f;
							instance.StartCoroutine(AnimateStatus(ShowStatusSpeed, ShowStatusWaitSeconds, instance.indexStatus, completeCallback, fromColorTransition, useColorTransition ? toColorTransition : instance.statusBarColor, useColorTransition));
						}
						else if (force)
						{
							UpdateStatusBar(force);
							_IsReadyStatusBar = true;
						}
						else
							_IsReadyStatusBar = true;
					}
					else
						_IsReadyStatusBar = true;
				}
				else
				{
					if (useColorTransition)
						instance.SetStatusColor(toColorTransition);
					StatusBarHeight = StatusBarHeight_STATIC;
					UpdateStatusBar(force);
					_IsReadyStatusBar = true;
					if (completeCallback != null)
						completeCallback();
				}
				return true;
			}
			return false;
		}
		internal override bool ShowNavigation(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((!instance.displayNavigtionBar && _IsReadyNavigationBar) || force)
			{
#if !NODEBUG || UNITY_EDITOR
				if (instance.debugMode)
					Debug.Log("MSANB: Show Navigation Bar");
#endif
				bool _displayNavigtionBar = instance.displayNavigtionBar;
				instance.displayNavigtionBar = true;
				_IsReadyNavigationBar = false;
				if (instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Android
#if UNITY_ANDROID
				|| instance.virtualDevice == MobileStatusNavigationBar.VirtualDevice.Auto
#endif
				)
				{
					if (!_displayNavigtionBar || force)
					{
						instance.indexNavigation++;
						if (!_displayNavigtionBar && !force)
						{
							startNavigation = 0f;
							targetNavigation = 1f;
							instance.StartCoroutine(AnimateNavigation(ShowNavigationSpeed, ShowNavigationWaitSeconds, instance.indexNavigation, completeCallback, fromColorTransition, useColorTransition ? toColorTransition : instance.navigationBarColor, useColorTransition));
						}
						else if (force)
						{
							UpdateNavigationBar(force);
							_IsReadyNavigationBar = true;
						}
						else
							_IsReadyNavigationBar = true;
					}
					else
						_IsReadyNavigationBar = true;
				}
				else
				{
					if (useColorTransition)
						instance.SetNavigationColor(toColorTransition);
					NavigationBarHeight = NavigationBarHeight_STATIC;
					_IsReadyNavigationBar = true;
					UpdateNavigationBar(force);
					if (completeCallback != null)
						completeCallback();
				}
				return true;
			}
			return false;
		}
		private static float startStatus;
		private static float targetStatus;
		private IEnumerator AnimateStatus(float speed, float waitSeconds, byte initIndex, DelegateAction completeCallback, Color fromColorTransition, Color toColorTransition, bool useColorTransition)
		{

			yield return new WaitForSeconds(waitSeconds);
			if (instance.indexStatus != initIndex)
				yield break;
			StatusBarHeight = Mathf.Lerp(0f, StatusBarHeight_STATIC, startStatus);
			if (targetStatus == 1f)
				instance.statusBar.gameObject.SetActive(true);
			bool isEnter = startStatus < targetStatus;
			while (Mathf.Abs(startStatus - targetStatus) != 0)
			{
				yield return new WaitForSeconds(1f / 120f);
				if (instance.indexStatus != initIndex)
					yield break;
				startStatus += isEnter ? speed / 120f : -speed / 120f;
				startStatus = Mathf.Clamp(startStatus, 0f, 1f);
				StatusBarHeight = Mathf.Lerp(0f, StatusBarHeight_STATIC, startStatus);
				instance.UpdateScrollbars();
				if (useColorTransition)
					instance.SetStatusColor(Color.Lerp(fromColorTransition, toColorTransition, isEnter ? startStatus : Mathf.Clamp01(1f - startStatus * 1.25f)), TextMode.Auto, true);
			}
			if (targetStatus == 0f)
				instance.statusBar.gameObject.SetActive(false);
			instance.UpdateScrollbars();
			yield return new WaitForSeconds(0.025f);
			_IsReadyStatusBar = true;
			if (completeCallback != null)
				completeCallback();
		}
		private static float startNavigation;
		private static float targetNavigation;
		private IEnumerator AnimateNavigation(float speed, float waitSeconds, byte initIndex, DelegateAction completeCallback, Color fromColorTransition, Color toColorTransition, bool useColorTransition)
		{
			yield return new WaitForSeconds(waitSeconds);
			if (instance.indexNavigation != initIndex)
				yield break;
			NavigationBarHeight = Mathf.Lerp(0f, NavigationBarHeight_STATIC, startNavigation);
			if (targetNavigation == 1f)
				instance.navigationBar.gameObject.SetActive(true);
			bool isEnter = startNavigation < targetNavigation;
			while (Mathf.Abs(startNavigation - targetNavigation) != 0)
			{
				yield return new WaitForSeconds(1f / 120f);
				if (instance.indexNavigation != initIndex)
					yield break;
				startNavigation += isEnter ? speed / 120f : -speed / 120f;
				startNavigation = Mathf.Clamp(startNavigation, 0f, 1f);
				NavigationBarHeight = Mathf.Lerp(0f, NavigationBarHeight_STATIC, startNavigation);
				instance.UpdateScrollbars();
				if (useColorTransition)
					instance.SetNavigationColor(Color.Lerp(fromColorTransition, toColorTransition, isEnter ? startNavigation : Mathf.Clamp01(1f - startNavigation * 1.25f)), TextMode.Auto, true);
			}
			if (targetNavigation == 0f)
				instance.navigationBar.gameObject.SetActive(false);
			instance.UpdateScrollbars();
			yield return new WaitForSeconds(0.025f);
			_IsReadyNavigationBar = true;
			if (completeCallback != null)
				completeCallback();
		}
		internal override void UpdateStatusBar(bool force = false)
		{
#if !NODEBUG || UNITY_EDITOR
			if (instance.debugMode)
				Debug.Log("MSANB: Update Status Bar");
#endif
			StatusBarHeight = instance.displayStatusBar ? StatusBarHeight_STATIC : 0f;
			instance.UpdateScrollbars();
		}
		internal override void UpdateNavigationBar(bool force = false)
		{
#if !NODEBUG || UNITY_EDITOR
			if (instance.debugMode)
				Debug.Log("MSANB: Update Navigation Bar");
#endif
			NavigationBarHeight = instance.displayNavigtionBar ? NavigationBarHeight_STATIC : 0f;
			
			instance.UpdateScrollbars();
		}
		internal override void UpdateStatusColor(Color c, TextMode statusBackgroundMode = TextMode.Auto)
		{
			instance.statusBarColor = c;
			
			switch (statusBackgroundMode)
			{
				case TextMode.Auto:
					if (!MobileStatusNavigationBar.LightBackground(c))
					{
						previewText.color = Color.white;
						previewTime.color = Color.white;
					}
					else
					{
						previewText.color = Color.black;
						previewTime.color = Color.black;
					}
					break;
				case TextMode.Dark:
					previewText.color = Color.black;
					previewTime.color = Color.black;
					break;
			}
		}
		static Color BLACK_BUTTONS = new Color(0f, 0f, 0f, 0.512f); 
		static Color WHITE_BUTTONS = new Color(1f, 1f, 1f, 0.88f); 
		internal override void UpdateNavigationColor(Color c, TextMode navigationTextMode = TextMode.Auto)
		{
			instance.navigationBarColor = c;
			switch (navigationTextMode)
			{
				case TextMode.Auto:
					if (!MobileStatusNavigationBar.LightBackground(c))
					{
						preveiwAndroidNavigationBack.gameObject.GetComponent<Image>().color = WHITE_BUTTONS;
						preveiwAndroidNavigationHome.color = WHITE_BUTTONS;
						preveiwAndroidNavigationOverview.color = WHITE_BUTTONS;
						preveiwIOSHome.color = WHITE_BUTTONS;
						preveiwIOSHome.transform.GetChild(0).GetComponent<Image>().color = WHITE_BUTTONS;
					preveiwIOSHome.transform.GetChild(1).GetComponent<Image>().color = WHITE_BUTTONS;
					}
					else
					{
						preveiwAndroidNavigationBack.gameObject.GetComponent<Image>().color = BLACK_BUTTONS;
						preveiwAndroidNavigationHome.color = BLACK_BUTTONS;
						preveiwAndroidNavigationOverview.color = BLACK_BUTTONS;
						preveiwIOSHome.color = BLACK_BUTTONS;
						preveiwIOSHome.transform.GetChild(0).GetComponent<Image>().color = BLACK_BUTTONS;
					preveiwIOSHome.transform.GetChild(1).GetComponent<Image>().color = BLACK_BUTTONS;
					}
					break;
				case TextMode.Dark:
					preveiwAndroidNavigationBack.gameObject.GetComponent<Image>().color = BLACK_BUTTONS;
					preveiwAndroidNavigationHome.color = BLACK_BUTTONS;
					preveiwAndroidNavigationOverview.color = BLACK_BUTTONS;
					preveiwIOSHome.color = BLACK_BUTTONS;
					preveiwIOSHome.transform.GetChild(0).GetComponent<Image>().color = BLACK_BUTTONS;
					preveiwIOSHome.transform.GetChild(1).GetComponent<Image>().color = BLACK_BUTTONS;
					break;
				case TextMode.Light:
					preveiwAndroidNavigationBack.gameObject.GetComponent<Image>().color = WHITE_BUTTONS;
					preveiwAndroidNavigationHome.color = WHITE_BUTTONS;
					preveiwAndroidNavigationOverview.color = WHITE_BUTTONS;
					preveiwIOSHome.color = WHITE_BUTTONS;
					preveiwIOSHome.transform.GetChild(0).GetComponent<Image>().color = WHITE_BUTTONS;
					preveiwIOSHome.transform.GetChild(1).GetComponent<Image>().color = WHITE_BUTTONS;
					break;
			}
		}
	}
}