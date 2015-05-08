using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class ShipComponent : MonoBehaviour
    {
        public Direction ComponentDirection;
        public ShipComponentType componentType;
        public float Mass;
        public GameObject ParentShip;
        public PossibleConnection[] possibleConnections;
        public float powerCapacity = 0;
        public float powerSupply = 0;
        public int value;

        public enum Direction
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3
        }

        public enum PossibleConnection
        {
            No = 0,//will not make a connection
            Yes = 1,//can make a connection
            MustBeEmpty = 2//will fail if any object is placed next to it
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
            Ship ship = ParentShip.GetComponent<Ship>();
            if (ship != null)
            {
                ship.addPower(powerSupply * Time.deltaTime);
            }
        }
    }
}