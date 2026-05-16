using UnityEngine;
using UnityEngine.UI;
using SaariTech.UI;
using System.Collections;


namespace SaariTech.Demo
{
	[RequireComponent(typeof(RectTransform))]
	public class PngVideoDemo : MonoBehaviour
	{
		public Image targetImage;
		public Sprite[] frames;
		public float fps = 10f;
		public bool loop = true;
		public bool fitInside = false;

		private int currentFrame = 0;
		private float timer = 0f;

		private RectTransform imageRect;
		private RectTransform parentRect;
		private bool isLight;
		private Image backgroundImage;

		public Color targetLight;
		public Color targetDark;

		bool isOn = true;

		void Start()
		{
			isLight = MobileStatusNavigationBar.Instance.startLightMode;
			
			imageRect = targetImage.GetComponent<RectTransform>();
			parentRect = targetImage.transform.parent.parent.GetComponent<RectTransform>();
			backgroundImage = targetImage.transform.parent.parent.GetComponent<Image>();


			targetImage.preserveAspect = true;
			StartCoroutine(FoulToggle());
			Animate();
		}

		void Update()
		{
			if (frames == null || frames.Length == 0 || targetImage == null)
				return;

			Animate();
			FitToParent();
		}

		public void ToggleLightDark()
		{
			isLight = !isLight;

			if(isLight)
				StatusNavigation.TransitionLightMode();
			else
				StatusNavigation.TransitionDarkMode();
		}

		public void ToggleStatusNavigation()
		{
			isOn = !isOn;

			if(isOn)
			{
				StatusBar.Show();
				NavigationBar.Show();
			}
			else
				StatusNavigation.Hide();
		}

		IEnumerator FoulToggle()
		{
			while(Application.isPlaying)
			{
				backgroundImage.color = Color.Lerp(
					backgroundImage.color,
					isLight ? Color.gray9 : Color.gray2,
					0.125f
				);

				targetImage.color = Color.Lerp(
					targetImage.color,
					isLight ? targetLight : targetDark,
					0.125f
				);

				yield return new WaitForSeconds(1f / 30f);
			}
		}

		void Animate()
		{
			timer += Time.deltaTime;

			if (timer >= 0f)
			{
				timer -= 1f / fps;

				currentFrame++;

				if (currentFrame >= frames.Length)
				{
					if (loop)
						currentFrame = 0;
					else
						currentFrame = frames.Length - 1;
				}

				targetImage.sprite = frames[currentFrame];
			}
		}

		void FitToParent()
		{
			if (targetImage.sprite == null) return;

			float parentWidth = parentRect.rect.width;
			float parentHeight = parentRect.rect.height;

			float spriteWidth = targetImage.sprite.rect.width;
			float spriteHeight = targetImage.sprite.rect.height;

			float parentRatio = parentWidth / parentHeight;
			float spriteRatio = spriteWidth / spriteHeight;

			Vector2 newSize;

			if (fitInside)
			{
				if (spriteRatio > parentRatio)
				{
					// bredare → matcha bredd
					float scale = parentWidth / spriteWidth;
					newSize = new Vector2(parentWidth, spriteHeight * scale);
				}
				else
				{
					float scale = parentHeight / spriteHeight;
					newSize = new Vector2(spriteWidth * scale, parentHeight);
				}
			}
			else
			{
				if (spriteRatio > parentRatio)
				{
					float scale = parentHeight / spriteHeight;
					newSize = new Vector2(spriteWidth * scale, parentHeight);
				}
				else
				{
					float scale = parentWidth / spriteWidth;
					newSize = new Vector2(parentWidth, spriteHeight * scale);
				}
			}

			imageRect.sizeDelta = newSize;
			imageRect.anchoredPosition = Vector2.zero;
		}
	}
}