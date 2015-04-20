using AsteroidBelt.ShipComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsteroidBelt
{
	public class ShipEditorTile : MonoBehaviour, IDropHandler
	{
		public ShipComponent.Direction Direction;
		public GameObject Part;

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

			if (Input.GetMouseButtonDown(1))
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