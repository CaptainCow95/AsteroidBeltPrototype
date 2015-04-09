using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class ShipComponent : MonoBehaviour
    {
        public Direction direction;
        public float mass;
        public GameObject parentShip;

        public enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }

        protected virtual void Start()
        {
            gameObject.transform.SetParent(parentShip.transform, false);

            switch (direction)
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