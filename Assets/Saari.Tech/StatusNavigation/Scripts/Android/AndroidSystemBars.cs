#if UNITY_ANDROID && !UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if USE_INPUTSYSTEM
using UnityEngine.InputSystem;
#endif
namespace SaariTech.UI
{
	internal class AndroidSystemBars : SystemBarsBase
	{
		static MobileStatusNavigationBar instance
		{
			get
			{
				return MobileStatusNavigationBar.Instance;
			}
		}
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
				switch(Screen.orientation)
				{
					case ScreenOrientation.Portrait:
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
						break;
					case ScreenOrientation.LandscapeLeft:
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
						break;
					case ScreenOrientation.LandscapeRight:
						instance.UpdateStatusBarRect(
							0f,
							0f,
							instance.canvasRectTransform.sizeDelta.y - StatusBarHeight,
							0f
						);
						instance.UpdateNavigationBarRect(
							StatusBarHeight,
							instance.canvasRectTransform.sizeDelta.x - NavigationBarHeight,
							0f,
							0f
						);
						instance.UpdateMainRect(
							StatusBarHeight,
							0f,
							0f,
							float.NaN
						);
						break;
					case ScreenOrientation.PortraitUpsideDown:
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
						break;
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
				switch(Screen.orientation)
				{
					case ScreenOrientation.Portrait:
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
						break;
					case ScreenOrientation.LandscapeLeft:
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
						break;
					case ScreenOrientation.LandscapeRight:
						instance.UpdateNavigationBarRect(
							StatusBarHeight,
							instance.canvasRectTransform.sizeDelta.x - NavigationBarHeight,
							0f,
							0f
						);
						instance.UpdateMainRect(
							float.NaN,
							0f,
							0f,
							NavigationBarHeight
						);
						break;
					case ScreenOrientation.PortraitUpsideDown:
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
						break;
				}
			}
		}
		internal static float NavigationBarHeight_STATIC { get; private set; }
		internal static List<UpdateLater> updateLaterList = new List<UpdateLater>();
		internal class UpdateLater
		{
			internal DelegateAction Action;
			internal float seconds;
		}
		static float HideAndroidStatusSpeed;
		static float HideAndroidNavigationSpeed;
		static float HideAndroidStatusWaitSeconds;
		static float HideAndroidNavigationWaitSeconds;
		static float ShowAndroidStatusSpeed;
		static float ShowAndroidNavigationSpeed;
		static float ShowAndroidStatusWaitSeconds;
		static float ShowAndroidNavigationWaitSeconds;
		static Vector3 cursorDirection = Vector3.zero;
		static Vector3 oldCursor = Vector3.zero;
		static Vector3 firstCursor = Vector3.zero;
		internal bool inited = false;
		internal override void OnApplicationFocus(bool focus)
		{
			if (focus)
				UIStatusNavigationBarAndroid.UpdateDisplay(instance.displayStatusBar, instance.displayNavigtionBar, true);
		}
		internal override void Awake() {}
		internal override void OnDisable()
		{
#if USE_INPUTSYSTEM
			if(backAction != null)
			{
				backAction.Disable();
				backAction.Dispose();
			}
			if(touchAction != null)
			{
				touchAction.Disable();
				touchAction.Dispose();
			}
#endif
		}
#if USE_INPUTSYSTEM
		private void OnBackPressed()
		{
#if !NODEBUG
			if (instance.debugMode)
				Debug.Log("MSANB: Back Button Clicked");
#endif
			instance.backButtonOnClick.Invoke();
		}
		private void OnTouchDown()
		{
			if(touchAction != null)
			{
				firstCursor = Touchscreen.current.primaryTouch.position.ReadValue();
				oldCursor = Touchscreen.current.primaryTouch.position.ReadValue();
				CheckVisibility();
			}
		}
#endif
		internal override void Refresh(bool force = false)
		{
			float scaleY = Screen.height / instance.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.y;
			float scaleX = Screen.width / instance.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.x;
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
			AndroidJavaClass statusNavigationBar = new AndroidJavaClass("com.saaritech.uistatusnavigationbar.UIStatusNavigationBar");
			switch(Screen.orientation)
			{
				case ScreenOrientation.Portrait:
					StatusBarHeight_STATIC = statusNavigationBar.CallStatic<int>("GetStatusBarHeight", context) / scaleY;
					MobileStatusNavigationBar.KeyboardExcludeNavigationBarHeight = statusNavigationBar.CallStatic<int>("GetNavigationBarHeight", context) / scaleY;
					if(UIStatusNavigationBarAndroid.GetSDKLevel() == 30 || Mobile.Config.SimpleMode)
						NavigationBarHeight_STATIC = 0f;
					else
						NavigationBarHeight_STATIC = statusNavigationBar.CallStatic<int>("GetNavigationBarHeight", context) / scaleY;
					break;
				case ScreenOrientation.PortraitUpsideDown:
					StatusBarHeight_STATIC = statusNavigationBar.CallStatic<int>("GetStatusBarHeight", context) / scaleY;
					MobileStatusNavigationBar.KeyboardExcludeNavigationBarHeight = statusNavigationBar.CallStatic<int>("GetNavigationBarHeight", context) / scaleY;
					NavigationBarHeight_STATIC = 0f;
					break;
				case ScreenOrientation.LandscapeLeft:
				case ScreenOrientation.LandscapeRight:
					StatusBarHeight_STATIC = statusNavigationBar.CallStatic<int>("GetStatusBarHeight", context) / scaleY;
					MobileStatusNavigationBar.KeyboardExcludeNavigationBarHeight = 0f;
					if(UIStatusNavigationBarAndroid.GetSDKLevel() == 30 || Mobile.Config.SimpleMode)
						NavigationBarHeight_STATIC = 0f;
					else
						NavigationBarHeight_STATIC = statusNavigationBar.CallStatic<int>("GetNavigationBarHeight", context) / scaleX;
					break;					
			}
			UpdateNavigationBar(true);
			UpdateStatusBar(true);
			UpdateAndroidBars();
		}
		internal override void OnScreenOrientationChange()
		{
			updateLaterList.Add(new UpdateLater
			{
				Action = () =>
				{
					Refresh();
				},
				seconds = (float)UIStatusNavigationBarAndroid.UpdateLaterTime / 1000f
			});
		}
		internal override void Update()
		{
			if (0 < updateLaterList.Count)
			{
				if (updateLaterList[0].seconds <= 0f)
				{
					updateLaterList[0].Action();
					updateLaterList.RemoveAt(0);
				}
				else
					updateLaterList[0].seconds -= Time.deltaTime;
			}
#if !USE_INPUTSYSTEM
			if (Input.GetKeyUp(KeyCode.Escape))
			{
#if !NODEBUG
				if (instance.debugMode)
					Debug.Log("MSANB: Back Button Clicked");
#endif
				backButtonOnClick.Invoke();
			}
			if (Input.GetMouseButtonDown(0))
			{
				firstCursor = Input.mousePosition;
				oldCursor = Input.mousePosition;
			}
			else if (Input.GetMouseButton(0))
			{
				cursorDirection = (Input.mousePosition - oldCursor).normalized;
				oldCursor = Input.mousePosition;
			}
#else
			if (touchAction != null && touchAction.ReadValue<float>() > 0f)
			{
				Vector3 cursor = Touchscreen.current.primaryTouch.position.ReadValue();
				cursorDirection = (cursor - oldCursor).normalized;
				oldCursor = cursor;
			}
#endif
		}
#if USE_INPUTSYSTEM
		private InputAction backAction;
		private InputAction touchAction;
#endif
		internal void OnEnable()
		{
#if USE_INPUTSYSTEM
			backAction = new InputAction(name: "Back", type: InputActionType.Button, binding: "<Keyboard>/escape");
			touchAction = new InputAction(name: "PrimaryTouch", type: InputActionType.Button, binding: "<Touchscreen>/primaryTouch/press");
			backAction.performed += ctx => OnBackPressed();
			touchAction.started += ctx => OnTouchDown();
			backAction.Enable();
			touchAction.Enable();
#endif
		}
		public void CheckVisibility()
		{
#if USE_INPUTSYSTEM
			if(touchAction != null && Touchscreen.current.primaryTouch.position.ReadValue().y < NavigationBarHeight_STATIC)
#else
			if(Input.GetMouseButtonDown(0) && Input.mousePosition.y < NavigationBarHeight_STATIC)
#endif
			{
				if (iCheckVisibility != null)
					instance.StopCoroutine(iCheckVisibility);
				iCheckVisibility = instance.StartCoroutine(ICheckVisibility());
			}
		}
		Coroutine iCheckVisibility = null;
		bool onceSlideUp = false;
		void UpdateSlideUp()
		{
			if(instance.colorizeAndoridSlideUpNavigationBar)
			{
#if USE_INPUTSYSTEM
				if (!instance.displayNavigtionBar && Touchscreen.current.primaryTouch.position.ReadValue().y < NavigationBarHeight_STATIC)
#else
				if (!displayNavigtionBar && Input.mousePosition.y < NavigationBarHeight_STATIC)
#endif
				{
					if(!onceSlideUp)
					{
						instance.ShowNavigation();
						UIStatusNavigationBarAndroid.UpdateDisplay(instance.displayStatusBar, true, true);
						instance.androidNavigationBarUserSlideUp.Invoke();
					}
					onceSlideUp = true;
				}
			}
			else
			{
				if(!instance.displayNavigtionBar && firstCursor.y < NavigationBarHeight_STATIC && cursorDirection.y > 0f)
				{
					if(!onceSlideUp)
						instance.androidNavigationBarUserSlideUp.Invoke();
					onceSlideUp = true;
				}
			}
		}
		void UpdateHide()
		{
			onceSlideUp = false;
			instance.HideNavigation();
			UIStatusNavigationBarAndroid.UpdateDisplay(instance.displayStatusBar, false, true);
			instance.androidNavigationBarAutoHidden.Invoke();
		}
		private IEnumerator ICheckVisibility()
		{
			if(!instance.colorizeAndoridSlideUpNavigationBar)
				yield return new WaitForSeconds(0.25f);
			using (AndroidJavaClass statusNavigationBar = new AndroidJavaClass("com.saaritech.uistatusnavigationbar.StatusNavigationBarO"))
			{
				int visibility = statusNavigationBar.CallStatic<int>("GetVisibility");
				switch (visibility)
				{
					case 2:
					case 6:
						UpdateSlideUp();
						break;
				}
				yield return new WaitForSeconds(instance.androidAutoHideNavigationBarDelay + (float)UIStatusNavigationBarAndroid.UpdateLaterTime / 1000f);
#if !NODEBUG
				visibility = statusNavigationBar.CallStatic<int>("GetVisibility");
				Debug.Log("MSANB: Check Visibility - " + visibility.ToString());
#endif
				if(onceSlideUp)
					UpdateHide();
				else
				{
					switch (visibility)
					{
						case 0:
							if (!instance.displayStatusBar)
								instance.HideStatus(true);
							if (!instance.displayNavigtionBar)
								instance.HideNavigation(true);
							break;
						case 2:
							if (!instance.displayStatusBar)
								instance.HideStatus(true);
							if (instance.displayNavigtionBar)
								instance.ShowNavigation(true);
							break;
						case 4:
							if (instance.displayStatusBar)
								instance.ShowStatus(true);
							if (!instance.displayNavigtionBar)
								instance.HideNavigation(true);
							break;
						case 6:
							if (instance.displayStatusBar)
								instance.ShowStatus(true);
							if (instance.displayNavigtionBar)
								instance.ShowNavigation(true);
							break;
					}
				}
			}
		}
		internal override void Init(bool force = false)
		{
			if (inited && !force)
				return;
			UIStatusNavigationBarAndroid.SimpleMode = Mobile.Config.SimpleMode;
			UIStatusNavigationBarAndroid.RunOnAndroidUiThread(() => {
				AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow");
				AndroidJavaClass statusNavigationBar = new AndroidJavaClass("com.saaritech.uistatusnavigationbar.StatusNavigationBarO");
				statusNavigationBar.CallStatic("InitWindow", window);
			});
			if(UIStatusNavigationBarAndroid.GetSDKLevel() < 30)
			{
				HideAndroidStatusSpeed = 3.5f;
				HideAndroidNavigationSpeed = 3.5f;
				HideAndroidStatusWaitSeconds = 0.25f;
				HideAndroidNavigationWaitSeconds = 0.25f;
				ShowAndroidStatusSpeed = 10f;
				ShowAndroidNavigationSpeed = 10f;
				ShowAndroidStatusWaitSeconds = 0f;
				ShowAndroidNavigationWaitSeconds = 0f;
			}
			else
			{
				HideAndroidStatusSpeed = Mathf.Pow(1.337f, 2.5f) * Mathf.PI;
				HideAndroidNavigationSpeed = Mathf.Pow(1.337f, 2.5f) * Mathf.PI;
				HideAndroidStatusWaitSeconds = 0.25f;
				HideAndroidNavigationWaitSeconds = 0.25f;
				ShowAndroidStatusSpeed = 7.5f;
				ShowAndroidNavigationSpeed = 7.5f;
				ShowAndroidStatusWaitSeconds = 0f;
				ShowAndroidNavigationWaitSeconds = 0f;
			}
			instance.statusBar.gameObject.SetActive(instance.displayStatusBar);
			instance.navigationBar.gameObject.SetActive(instance.displayNavigtionBar);
#if !NODEBUG
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
			AndroidSystemBars.updateLaterList.Add(new AndroidSystemBars.UpdateLater
			{
				Action = () =>
				{
					Refresh(true);
				},
				seconds = (float)UIStatusNavigationBarAndroid.UpdateLaterTime / 1000f
			});
			inited = true;
			MobileStatusNavigationBar.IsReadyStatusBar = true;
			MobileStatusNavigationBar.IsReadyNavigationBar = true;
		}
		internal override bool Hide(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((instance.displayStatusBar && MobileStatusNavigationBar.IsReadyStatusBar) || (instance.displayNavigtionBar && MobileStatusNavigationBar.IsReadyNavigationBar) || force)
			{
#if !NODEBUG
				if (instance.debugMode)
					Debug.Log("MSANB: Hide Bars");
#endif
				bool _displayStatusBar = instance.displayStatusBar;
				bool _displayNavigtionBar = instance.displayNavigtionBar;
				instance.displayStatusBar = false;
				instance.displayNavigtionBar = false;
				MobileStatusNavigationBar.IsReadyStatusBar = false;
				MobileStatusNavigationBar.IsReadyNavigationBar = false;
				if (_displayStatusBar || force)
				{
					instance.indexStatus++;
					if (_displayStatusBar && !force)
					{
						startStatus = 1f;
						targetStatus = 0f;
						instance.StartCoroutine(UpdateStatusBarLater(0f, instance.indexStatus));
						instance.StartCoroutine(AnimateStatus(HideAndroidStatusSpeed, HideAndroidStatusWaitSeconds, instance.indexStatus, completeCallback, instance.statusBarColor, useColorTransition ? toColorTransition : instance.statusBarColor, useColorTransition));
					}
					else if (force)
					{
						UpdateStatusBar(force);
						MobileStatusNavigationBar.IsReadyStatusBar = true;
					}
					else
						MobileStatusNavigationBar.IsReadyStatusBar = true;
				}
				else
					MobileStatusNavigationBar.IsReadyStatusBar = true;
				if (_displayNavigtionBar || force)
				{
					instance.indexNavigation++;
					if (_displayNavigtionBar && !force)
					{
						startNavigation = 1f;
						targetNavigation = 0f;
						instance.StartCoroutine(UpdateNavigationBarLater(0f, instance.indexNavigation));
						instance.StartCoroutine(AnimateNavigation(HideAndroidNavigationSpeed, HideAndroidNavigationWaitSeconds, instance.indexNavigation, completeCallback, instance.navigationBarColor, useColorTransition ? toColorTransition : instance.navigationBarColor, useColorTransition));
					}
					else if (force)
					{
						UpdateNavigationBar(force);
						MobileStatusNavigationBar.IsReadyNavigationBar = true;
					}
					else
						MobileStatusNavigationBar.IsReadyNavigationBar = true;
				}
				else
					MobileStatusNavigationBar.IsReadyNavigationBar = true;
				return true;
			}
			return false;
		}
		internal override bool HideStatus(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((instance.displayStatusBar && MobileStatusNavigationBar.IsReadyStatusBar) || force)
			{
#if !NODEBUG
				if (instance.debugMode)
					Debug.Log("MSANB: Hide Status Bar");
#endif
				bool _displayStatusBar = instance.displayStatusBar;
				instance.displayStatusBar = false;
				MobileStatusNavigationBar.IsReadyStatusBar = false;
				if (_displayStatusBar || force)
				{
					instance.indexStatus++;
					if (_displayStatusBar && !force)
					{
						startStatus = 1f;
						targetStatus = 0f;
						instance.StartCoroutine(UpdateStatusBarLater(0f, instance.indexStatus));
						instance.StartCoroutine(AnimateStatus(HideAndroidStatusSpeed, HideAndroidStatusWaitSeconds, instance.indexStatus, completeCallback, instance.statusBarColor, useColorTransition ? toColorTransition : instance.statusBarColor, useColorTransition));
					}
					else if (force)
					{
						UpdateStatusBar(force);
						MobileStatusNavigationBar.IsReadyStatusBar = true;
					}
					else
						MobileStatusNavigationBar.IsReadyStatusBar = true;
				}
				else
					MobileStatusNavigationBar.IsReadyStatusBar = true;
				return true;
			}
			return false;
		}
		internal override bool HideNavigation(Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((instance.displayNavigtionBar && MobileStatusNavigationBar.IsReadyNavigationBar) || force)
			{
#if !NODEBUG
				if (instance.debugMode)
					Debug.Log("MSANB: Hide Navigation Bar");
#endif
				bool _displayNavigtionBar = instance.displayNavigtionBar;
				instance.displayNavigtionBar = false;
				MobileStatusNavigationBar.IsReadyNavigationBar = false;
				if (_displayNavigtionBar || force)
				{
					instance.indexNavigation++;
					if (_displayNavigtionBar && !force)
					{
						startNavigation = 1f;
						targetNavigation = 0f;
						instance.StartCoroutine(UpdateNavigationBarLater(0f, instance.indexNavigation));
						instance.StartCoroutine(AnimateNavigation(HideAndroidNavigationSpeed, HideAndroidNavigationWaitSeconds, instance.indexNavigation, completeCallback, instance.navigationBarColor, useColorTransition ? toColorTransition : instance.navigationBarColor, useColorTransition));
					}
					else if (force)
					{
						UpdateNavigationBar(force);
						MobileStatusNavigationBar.IsReadyNavigationBar = true;
					}
					else
						MobileStatusNavigationBar.IsReadyNavigationBar = true;
				}
				else
					MobileStatusNavigationBar.IsReadyNavigationBar = true;
				return true;
			}
			return false;
		}
		internal override bool Show(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((!instance.displayStatusBar && MobileStatusNavigationBar.IsReadyStatusBar) || (!instance.displayNavigtionBar && MobileStatusNavigationBar.IsReadyNavigationBar) || force)
			{
#if !NODEBUG
				if (instance.debugMode)
					Debug.Log("MSANB: Show Bars");
#endif
				bool _displayStatusBar = instance.displayStatusBar;
				bool _displayNavigtionBar = instance.displayNavigtionBar;
				instance.displayNavigtionBar = true;
				instance.displayStatusBar = true;
				MobileStatusNavigationBar.IsReadyStatusBar = false;
				MobileStatusNavigationBar.IsReadyNavigationBar = false;
				if (!_displayStatusBar || force)
				{
					instance.indexStatus++;
					if (!_displayStatusBar && !force)
					{
						startStatus = 0f;
						targetStatus = 1f;
						instance.StartCoroutine(UpdateStatusBarLater(0.25f, instance.indexStatus));
						instance.StartCoroutine(AnimateStatus(ShowAndroidStatusSpeed, ShowAndroidStatusWaitSeconds, instance.indexStatus, completeCallback, fromColorTransition, useColorTransition ? toColorTransition : instance.statusBarColor, useColorTransition));
					}
					else if (force)
					{
						UpdateStatusBar(force);
						MobileStatusNavigationBar.IsReadyStatusBar = true;
					}
					else
						MobileStatusNavigationBar.IsReadyStatusBar = true;
				}
				else
					MobileStatusNavigationBar.IsReadyStatusBar = true;
				if (!_displayNavigtionBar || force)
				{
					instance.indexNavigation++;
					if (!_displayNavigtionBar && !force)
					{
						startNavigation = 0f;
						targetNavigation = 1f;
						instance.StartCoroutine(UpdateNavigationBarLater(0.25f, instance.indexNavigation));
						instance.StartCoroutine(AnimateNavigation(ShowAndroidNavigationSpeed, ShowAndroidNavigationWaitSeconds, instance.indexNavigation, completeCallback, fromColorTransition, useColorTransition ? toColorTransition : instance.navigationBarColor, useColorTransition));
					}
					else if (force)
					{
						UpdateNavigationBar(force);
						MobileStatusNavigationBar.IsReadyNavigationBar = true;
					}
					else
						MobileStatusNavigationBar.IsReadyNavigationBar = true;
				}
				else
					MobileStatusNavigationBar.IsReadyNavigationBar = true;
				return true;
			}
			return false;
		}
		internal override bool ShowStatus(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((!instance.displayStatusBar && MobileStatusNavigationBar.IsReadyStatusBar) || force)
			{
#if !NODEBUG
				if (instance.debugMode)
					Debug.Log("MSANB: Show Status Bar");
#endif
				bool _displayStatusBar = instance.displayStatusBar;
				instance.displayStatusBar = true;
				MobileStatusNavigationBar.IsReadyStatusBar = false;
				if (!_displayStatusBar || force)
				{
					instance.indexStatus++;
					if (!_displayStatusBar && !force)
					{
						startStatus = 0f;
						targetStatus = 1f;
						instance.StartCoroutine(UpdateStatusBarLater(0.25f, instance.indexStatus));
						instance.StartCoroutine(AnimateStatus(ShowAndroidStatusSpeed, ShowAndroidStatusWaitSeconds, instance.indexStatus, completeCallback, fromColorTransition, useColorTransition ? toColorTransition : instance.statusBarColor, useColorTransition));
					}
					else if (force)
					{
						UpdateStatusBar(force);
						MobileStatusNavigationBar.IsReadyStatusBar = true;
					}
					else
						MobileStatusNavigationBar.IsReadyStatusBar = true;
				}
				else
					MobileStatusNavigationBar.IsReadyStatusBar = true;
				return true;
			}
			return false;
		}
		internal override bool ShowNavigation(Color fromColorTransition, Color toColorTransition, DelegateAction completeCallback = null, bool useColorTransition = true, bool force = false)
		{
			if ((!instance.displayNavigtionBar && MobileStatusNavigationBar.IsReadyNavigationBar) || force)
			{
#if !NODEBUG
				if (instance.debugMode)
					Debug.Log("MSANB: Show Navigation Bar");
#endif
				bool _displayNavigtionBar = instance.displayNavigtionBar;
				instance.displayNavigtionBar = true;
				MobileStatusNavigationBar.IsReadyNavigationBar = false;
				if(!_displayNavigtionBar || force)
				{
					instance.indexNavigation++;
					if (!_displayNavigtionBar && !force)
					{
						startNavigation = 0f;
						targetNavigation = 1f;
						instance.StartCoroutine(UpdateNavigationBarLater(0.25f, instance.indexNavigation));
						instance.StartCoroutine(AnimateNavigation(ShowAndroidNavigationSpeed, ShowAndroidNavigationWaitSeconds, instance.indexNavigation, completeCallback, fromColorTransition, useColorTransition ? toColorTransition : instance.navigationBarColor, useColorTransition));
					}
					else if (force)
					{
						UpdateNavigationBar(force);
						MobileStatusNavigationBar.IsReadyNavigationBar = true;
					}
					else
						MobileStatusNavigationBar.IsReadyNavigationBar = true;
				}
				else
					MobileStatusNavigationBar.IsReadyNavigationBar = true;
				return true;
			}
			return false;
		}
		private IEnumerator UpdateStatusBarLater(float waitSeconds, byte initIndex)
		{
			yield return new WaitForSeconds(waitSeconds);
			if (instance.indexStatus != initIndex)
				yield break;
			UpdateStatusBar();
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
			if (!useColorTransition)
				instance.UpdateStatusColor(TextMode.Auto);
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
			MobileStatusNavigationBar.IsReadyStatusBar = true;
			if (completeCallback != null)
				completeCallback();
		}
		private IEnumerator UpdateNavigationBarLater(float waitSeconds, byte initIndex)
		{
			yield return new WaitForSeconds(waitSeconds);
			if (instance.indexNavigation != initIndex)
				yield break;
			UpdateNavigationBar();
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
			if (!useColorTransition)
				instance.UpdateNavigationColor(TextMode.Auto);
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
			MobileStatusNavigationBar.IsReadyNavigationBar = true;
			if (completeCallback != null)
				completeCallback();
		}
		internal override void UpdateStatusBar(bool force = false)
		{
#if !NODEBUG
			if (instance.debugMode)
				Debug.Log("MSANB: Update Status Bar");
#endif
			if(force)
				StatusBarHeight = instance.displayStatusBar ? StatusBarHeight_STATIC : 0f;
			UpdateAndroidBars();
			instance.UpdateScrollbars();
		}
		internal override void UpdateNavigationBar(bool force = false)
		{
#if !NODEBUG
			if (instance.debugMode)
				Debug.Log("MSANB: Update Navigation Bar");
#endif
			if(force)
				NavigationBarHeight = instance.displayNavigtionBar ? NavigationBarHeight_STATIC : 0f;
			UpdateAndroidBars();
			instance.UpdateScrollbars();
		}
		void UpdateAndroidBars()
		{
			UIStatusNavigationBarAndroid.UpdateDisplay(instance.displayStatusBar, instance.displayNavigtionBar);
		}
		internal override void UpdateStatusColor(Color c, TextMode statusBackgroundMode = TextMode.Auto)
		{
			instance.statusBarColor = c;
			switch (statusBackgroundMode)
			{
				case TextMode.Auto:
					UIStatusNavigationBarAndroid.LightModeStatus = MobileStatusNavigationBar.LightBackground(c);
					UIStatusNavigationBarAndroid.UpdateMode();
					break;
				case TextMode.Dark:
					UIStatusNavigationBarAndroid.LightModeStatus = true;
					UIStatusNavigationBarAndroid.UpdateMode();
					break;
				case TextMode.Light:
					UIStatusNavigationBarAndroid.LightModeStatus = false;
					UIStatusNavigationBarAndroid.UpdateMode();
					break;
			}
		}
		internal override void UpdateNavigationColor(Color c, TextMode navigationTextMode = TextMode.Auto)
		{
			instance.navigationBarColor = c;
			switch (navigationTextMode)
			{
				case TextMode.Auto:
					UIStatusNavigationBarAndroid.LightModeNavigation = MobileStatusNavigationBar.LightBackground(c);
					UIStatusNavigationBarAndroid.UpdateMode();
					break;
				case TextMode.Dark:
					UIStatusNavigationBarAndroid.LightModeNavigation = true;
					UIStatusNavigationBarAndroid.UpdateMode();
					break;
				case TextMode.Light:
					UIStatusNavigationBarAndroid.LightModeNavigation = false;
					UIStatusNavigationBarAndroid.UpdateMode();
					break;
			}
		}
	}
}
#endif