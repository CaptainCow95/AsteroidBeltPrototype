using UnityEngine;
using UnityEngine.EventSystems;

public class ShipEditorTile : MonoBehaviour, IDropHandler
{
	public GameObject Part;

	public void OnDrop(PointerEventData eventData)
	{
		if (Part != null)
		{
			Destroy(Part);
			Part = null;
		}

		Part = ShipEditor.Instance.CurrentPart;
		ShipEditor.Instance.DropValid = true;
		ShipEditor.Instance.CurrentPart = null;

		Part.transform.position = transform.position;
	}
}