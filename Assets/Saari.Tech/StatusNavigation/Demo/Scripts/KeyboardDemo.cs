using SaariTech.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SaariTech.Demo
{
	public class KeyboardDemo : MonoBehaviour
	{
		public RectTransform cuteRedPanda;
		public RectTransform sun;
		public RectTransform inputText;
		public Text text;
		float delayAnimation = 0f;
		const float OPEN_KEYBOARD_BUTTON_HEIGHT = 200f;

		public void OpenKeyboard()
		{
			TouchScreenKeyboard.hideInput = true;
			TouchScreenKeyboard.Open("");
		}

		void Update()
		{
			text.text = MobileKeyboardManager.Height.ToString();

			if(TouchScreenKeyboard.visible)
			{
				sun.anchoredPosition = Vector2.Lerp(
					sun.anchoredPosition,
					new Vector2(
						sun.anchoredPosition.x,
						MobileKeyboardManager.Height + 256f
					),
					5.0f * Time.deltaTime
				);

				if(delayAnimation > 0.5f)
				{
					cuteRedPanda.anchoredPosition = Vector2.Lerp(
						cuteRedPanda.anchoredPosition,
						new Vector2(
							cuteRedPanda.anchoredPosition.x,
							MobileKeyboardManager.Height
						),
						5.0f * Time.deltaTime
					);
				}
				else
				{
					delayAnimation += Time.deltaTime;
				}

				inputText.sizeDelta = new Vector2(
					inputText.sizeDelta.x,
					GetComponentInParent<Canvas>().pixelRect.height - MobileKeyboardManager.Height - StatusBar.Height - NavigationBar.Height - OPEN_KEYBOARD_BUTTON_HEIGHT - 96
				);
			}
			else
			{
				delayAnimation = 0f;

				sun.anchoredPosition = Vector2.Lerp(
					sun.anchoredPosition,
					new Vector2(
						sun.anchoredPosition.x,
						384f
					),
					5.0f * Time.deltaTime
				);

				cuteRedPanda.anchoredPosition = new Vector2(
					cuteRedPanda.anchoredPosition.x,
					0f
				);

				inputText.sizeDelta = new Vector2(
					inputText.sizeDelta.x,
					GetComponentInParent<Canvas>().pixelRect.height - StatusBar.Height - NavigationBar.Height - OPEN_KEYBOARD_BUTTON_HEIGHT - 96
				);
			}
		}
	}
}