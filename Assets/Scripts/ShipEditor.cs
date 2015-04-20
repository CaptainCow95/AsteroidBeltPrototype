using AsteroidBelt.Assets.Scripts;
using UnityEngine;

namespace AsteroidBelt
{
	public class ShipEditor : Singleton<ShipEditor>
	{
		public GameObject CurrentPart;
		public bool DropValid;

		private void Update()
		{
			if (CurrentPart != null)
			{
				CurrentPart.GetComponent<RectTransform>().position = Input.mousePosition;

				if (!Input.GetMouseButton(0))
				{
					if (!DropValid)
					{
						//Destroy(CurrentPart);
					}

					CurrentPart = null;

					DropValid = false;
				}
			}
		}
	}
}