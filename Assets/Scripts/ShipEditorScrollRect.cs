using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipEditorScrollRect : ScrollRect
{
	public override void OnBeginDrag(PointerEventData eventData)
	{
		foreach (var component in GetComponentsInChildren<Component>().Except(GetComponents<Component>()))
		{
			if (component is IBeginDragHandler)
			{
				((IBeginDragHandler)component).OnBeginDrag(eventData);
			}
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
	}
}