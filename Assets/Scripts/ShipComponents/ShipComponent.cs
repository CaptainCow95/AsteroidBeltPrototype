using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
	public class ShipComponent : MonoBehaviour
	{
		public Direction ComponentDirection;
		public float Mass;
		public GameObject ParentShip;

		public enum Direction
		{
			Up = 0,
			Right = 1,
			Down = 2,
			Left = 3
		}

		protected virtual void Start()
		{
			if (ParentShip == null)
			{
				return;
			}

			gameObject.transform.SetParent(ParentShip.transform, false);

			switch (ComponentDirection)
			{
				case Direction.Up:
					gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
					break;

				case Direction.Left:
					gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
					break;

				case Direction.Down:
					gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
					break;

				case Direction.Right:
					gameObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
					break;
			}
		}

		protected virtual void Update()
		{
		}
	}
}