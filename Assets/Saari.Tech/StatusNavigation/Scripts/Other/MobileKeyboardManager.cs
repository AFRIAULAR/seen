using UnityEngine;

namespace SaariTech.UI
{
	public class MobileKeyboardManager : MonoBehaviour
	{
		float height;
		float width;
		internal static float Height = 0f;
		internal static float Width = 0f;
#if UNITY_ANDROID && !UNITY_EDITOR
		bool isBusy = false;
		private static AndroidJavaObject androidKeyboard;
		void Start()
		{
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass keyboardClass = new AndroidJavaClass("com.saaritech.keyboard.SaariTechKeyboard");
			androidKeyboard = keyboardClass;
			androidKeyboard.CallStatic("initialize", activity);
		}
		void FixedUpdate()
		{
			GetSize(UpdateSize);
			RunAfterFrame();
		}
		void UpdateSize(float height, float width)
		{
			Height = height;
			Width = width;
		}
		delegate void Callback(float height, float width);
		Callback _GetSizeCallback; 
		bool SimpleMode;
		void GetSize(Callback callback)
		{
			if(isBusy)
				return;
			isBusy = true;
			if(height != 0 && width != 0)
			{
				if(TouchScreenKeyboard.visible)
					callback(height, width);
				else
					callback(0f, 0f);
				isBusy = false;
				return;
			}
			if(!TouchScreenKeyboard.visible)
			{
				callback(0f, 0f);
				isBusy = false;
				MobileStatusNavigationBar.Instance.Refresh();
				Refresh();
				return;
			}
			_GetSizeCallback = callback;
			SimpleMode = UIStatusNavigationBarAndroid.SimpleMode;
			UIStatusNavigationBarAndroid.SimpleMode = true; // Force to use Simple Mode for accessing keyboard data, if not already in simple mode, it will cause glitch.
			Screen.fullScreen = false;
			UIStatusNavigationBarAndroid.UpdateDisplay(StatusNavigation.instance.displayStatusBar, StatusNavigation.instance.displayNavigtionBar, GetSizeCallback, true);
		}
		void GetSizeCallback()
		{
			doRunAfterFrame = 0;
			Refresh();
		}
		void Refresh()
		{
			AndroidSystemBars.updateLaterList.Add(new AndroidSystemBars.UpdateLater
			{
				Action = () =>
				{
					MobileStatusNavigationBar.Instance.Refresh();
				},
				seconds = (float)UIStatusNavigationBarAndroid.UpdateLaterTime / 1000f
			});
		}
		int doRunAfterFrame = -1;
		void RunAfterFrame()
		{
			if(doRunAfterFrame == -1)
				return;
			if(doRunAfterFrame < 1)
			{
				doRunAfterFrame++;
				return;
			}
			if(androidKeyboard?.CallStatic<int>("Height") == 0)
				return;
			doRunAfterFrame = -1;
			MobileStatusNavigationBar mobileStatusNavigation = FindFirstObjectByType<MobileStatusNavigationBar>(FindObjectsInactive.Exclude);
			float scale = Screen.height / mobileStatusNavigation.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.y;
			float _height = (float)(androidKeyboard?.CallStatic<int>("Height")) / scale - MobileStatusNavigationBar.KeyboardExcludeNavigationBarHeight;
			float _width = (float)androidKeyboard?.CallStatic<int>("Width") / scale;
			if(_height > 0)
				height = _height;
			if(_width > 0)
				width = _width;
			_GetSizeCallback(height, width);
			_GetSizeCallback = null;
			UIStatusNavigationBarAndroid.SimpleMode = SimpleMode;
			Screen.fullScreen = false;
			UIStatusNavigationBarAndroid.UpdateDisplay(StatusNavigation.instance.displayStatusBar, StatusNavigation.instance.displayNavigtionBar, CleanUp, true);
		}
		void CleanUp()
		{
			isBusy = false;
		}
#elif UNITY_IOS && !UNITY_EDITOR
		void Update()
		{
			//MobileStatusNavigationBar mobileStatusNavigation = FindFirstObjectByType<MobileStatusNavigationBar>(FindObjectsInactive.Exclude);
			//float scale = Screen.height / mobileStatusNavigation.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.y;
			
			Height = TouchScreenKeyboard.area.height; // if outputs wrong data, you might want to change this line to Height = TouchScreenKeyboard.area.height / scale; 
			Width = TouchScreenKeyboard.area.width;
		}
#endif
	}
}