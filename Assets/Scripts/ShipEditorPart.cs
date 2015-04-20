using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipEditorPart : MonoBehaviour, IBeginDragHandler
{
	public GameObject Part;

	public void OnBeginDrag(PointerEventData eventData)
	{
		bool onObject = true;

		var rect = GetComponent<RectTransform>().rect;
		rect.x += transform.position.x;
		rect.y += transform.position.y;
		if (eventData.position.x < rect.x ||
			eventData.position.x > rect.xMax ||
			eventData.position.y < rect.y ||
			eventData.position.y > rect.yMax)
		{
			onObject = false;
		}

		if (onObject)
		{
			ShipEditor.Instance.CurrentPart = Instantiate(Part);
			ShipEditor.Instance.CurrentPart.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
			ShipEditor.Instance.CurrentPart.AddComponent<RectTransform>();

			// Disable drag events
			SetLayerRecursively(ShipEditor.Instance.CurrentPart, 2);
		}
	}

	private static void SetLayerRecursively(GameObject obj, int layer)
	{
		if (obj == null)
		{
			return;
		}

		obj.layer = layer;

		foreach (Transform child in obj.GetComponentsInChildren<Transform>(true).Except(obj.GetComponents<Transform>()))
		{
			if (child == null)
			{
				continue;
			}

			SetLayerRecursively(child.gameObject, layer);
		}
	}
}