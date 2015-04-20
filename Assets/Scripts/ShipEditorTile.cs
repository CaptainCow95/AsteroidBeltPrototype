using AsteroidBelt.ShipComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsteroidBelt
{
	public class ShipEditorTile : MonoBehaviour, IBeginDragHandler, IDropHandler
	{
		public ShipComponent.Direction Direction;
		public GameObject Part;

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (Part == null)
			{
				return;
			}

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
				ShipEditor.Instance.CurrentPart = Part;
				Part = null;
			}
		}

		public void OnDrop(PointerEventData eventData)
		{
			if (Part != null)
			{
				Destroy(Part);
				Part = null;
			}

			Direction = ShipComponent.Direction.Up;

			Part = ShipEditor.Instance.CurrentPart;
			ShipEditor.Instance.DropValid = true;
			ShipEditor.Instance.CurrentPart = null;

			Part.GetComponent<RectTransform>().position = transform.position;
		}

		private void Update()
		{
			if (Part == null)
			{
				return;
			}

			var rect = GetComponent<RectTransform>().rect;
			rect.x += transform.position.x;
			rect.y += transform.position.y;
			if (Input.GetMouseButtonDown(1) &&
				Input.mousePosition.x > rect.x &&
				Input.mousePosition.x < rect.xMax &&
				Input.mousePosition.y > rect.y &&
				Input.mousePosition.y < rect.yMax)
			{
				Direction++;
				if ((int)Direction > 3)
				{
					Direction = 0;
				}

				UpdateDirection();
			}
		}

		private void UpdateDirection()
		{
			switch (Direction)
			{
				case ShipComponent.Direction.Up:
					Part.GetComponent<RectTransform>().rotation = Quaternion.identity;
					break;

				case ShipComponent.Direction.Right:
					Part.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 270);
					break;

				case ShipComponent.Direction.Down:
					Part.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
					break;

				case ShipComponent.Direction.Left:
					Part.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
					break;
			}
		}
	}
}