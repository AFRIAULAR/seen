using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SaariTech.Demo
{
	public class HuePicker_Static : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public Color hueColor;
		public RectTransform pointer;
		public ColorPicker_Static colorPicker;

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

		public void GoToColor(Color targetColor)
		{
			float distance = float.MaxValue;
			Color target = new Color();
			float offset = 0f;
			for (float i = 0; i <= 1f; i += 0.0001f)
			{
				Color t = GetHueColor(i);

				if (Vector3.Distance(new Vector3(t.r, t.g, t.b), new Vector3(targetColor.r, targetColor.g, targetColor.b)) < distance)
				{
					distance = Vector3.Distance(new Vector3(t.r, t.g, t.b), new Vector3(targetColor.r, targetColor.g, targetColor.b));
					target = t;
					offset = i;
				}
			}
			hueColor = target;
			RectTransform self = GetComponent<RectTransform>();
			pointer.anchoredPosition = new Vector2(self.rect.width * offset, 0f);
			pointer.GetComponent<Image>().color = hueColor;
		}

		private void OnDown(PointerEventData eventData)
		{
			RectTransform self = GetComponent<RectTransform>();
			if (self.position.x - (self.rect.width * GetComponentInParent<Canvas>().transform.localScale.x) / 2f < eventData.position.x && self.position.x + (self.rect.width * GetComponentInParent<Canvas>().transform.localScale.x) / 2f > eventData.position.x)
			{
				pointer.position = new Vector3(eventData.position.x, self.position.y);
				float offset = (float)pointer.anchoredPosition.x / (float)self.rect.width;
				hueColor = GetHueColor(offset);
				pointer.GetComponent<Image>().color = hueColor;
				colorPicker.UpdateColor();
			}
		}

		private Color GetHueColor(float offset)
		{
			Color hueColor;
			if (offset < 0.2f)
				hueColor = Color.Lerp(Color.red, Color.yellow, Mathf.InverseLerp(0f, 0.2f, offset));
			else if (offset < 0.4f)
				hueColor = Color.Lerp(Color.yellow, Color.green, Mathf.InverseLerp(0.2f, 0.4f, offset));
			else if (offset < 0.6f)
				hueColor = Color.Lerp(Color.green, Color.cyan, Mathf.InverseLerp(0.4f, 0.6f, offset));
			else if (offset < 0.8f)
				hueColor = Color.Lerp(Color.cyan, Color.blue, Mathf.InverseLerp(0.6f, 0.8f, offset));
			else
				hueColor = Color.Lerp(Color.blue, Color.magenta, Mathf.InverseLerp(0.8f, 1f, offset));
			return hueColor;
		}
	}
}