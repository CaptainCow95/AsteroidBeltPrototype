using UnityEngine;
using UnityEngine.EventSystems;

namespace AsteroidBelt
{
	public class ShipEditorPart : MonoBehaviour, IBeginDragHandler
	{
		public ShipComponents.ShipComponentType ComponentType;
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
				ShipEditor.Instance.CurrentPart.GetComponent<RectTransform>().SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>());
			}
		}
	}
}