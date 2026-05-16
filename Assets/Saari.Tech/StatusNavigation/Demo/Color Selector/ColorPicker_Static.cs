using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SaariTech.UI;

namespace SaariTech.Demo
{
	public class ColorPicker_Static : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public HuePicker_Static huePicker;
		public RectTransform pointer;
		public Color pickedColor;
		public RawImage image;
		public Image pickerIcon;

		public void OnBeginDrag(PointerEventData eventData)
		{
			OnDown(eventData);
		}

		public void OnDrag(PointerEventData eventData)
		{
			OnDown(eventData);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			OnDown(eventData);
		}

		private void OnDown(PointerEventData eventData)
		{
			RectTransform self = GetComponent<RectTransform>();

			pointer.position = new Vector2(
				Mathf.Clamp(eventData.position.x, self.position.x - (self.rect.width * GetComponentInParent<Canvas>().transform.localScale.x) / 2f, self.position.x + (self.rect.width * GetComponentInParent<Canvas>().transform.localScale.x) / 2f),
				Mathf.Clamp(eventData.position.y, self.position.y - (self.rect.height * GetComponentInParent<Canvas>().transform.localScale.y) / 2f, self.position.y + (self.rect.height * GetComponentInParent<Canvas>().transform.localScale.y) / 2f)
			);
			UpdateColor();
		}

		public void GoToColor(Color targetColor)
		{
			image.material.SetColor("_HueColor", huePicker.hueColor);
			float distance = float.MaxValue;
			Color target = new Color();
			float offsetX = 0f;
			float offsetY = 0f;
			for (float x = 0; x <= 1f; x += 0.001f)
				for (float y = 0; y <= 1f; y += 0.001f)
				{
					Color t = GetColor(x, y);

					if (Vector3.Distance(new Vector3(t.r, t.g, t.b), new Vector3(targetColor.r, targetColor.g, targetColor.b)) < distance)
					{
						distance = Vector3.Distance(new Vector3(t.r, t.g, t.b), new Vector3(targetColor.r, targetColor.g, targetColor.b));
						target = t;
						offsetX = x;
						offsetY = y;
					}
				}
			pickedColor = target;
			RectTransform self = GetComponent<RectTransform>();
			pointer.anchoredPosition = new Vector2(self.rect.width * offsetX, self.rect.height * offsetY);
			pointer.GetComponent<Image>().color = pickedColor;
			UpdateColor();
		}

		Color GetColor(float x, float y)
		{
			RectTransform self = GetComponent<RectTransform>();
			Color color = huePicker.hueColor * (1f - Vector2.Distance(new Vector2(1f, 1f), new Vector2(x, y)));
			Color white = Color.white * (1f - Vector2.Distance(new Vector2(0f, 1f), new Vector2(x, y)));
			Color pickedColor = color + white;
			pickedColor.a = 1;
			return pickedColor;
		}

		public void UpdateColor()
		{
			image.material.SetColor("_HueColor", huePicker.hueColor);
			RectTransform self = GetComponent<RectTransform>();

			pickedColor = GetColor(
				(float)pointer.anchoredPosition.x / (float)self.rect.width,
				(float)pointer.anchoredPosition.y / (float)self.rect.height
			);
			pointer.GetComponent<Image>().color = pickedColor;
			StatusNavigation.SetColor(pickedColor);
			pickerIcon.color = pickedColor;
		}
	}
}