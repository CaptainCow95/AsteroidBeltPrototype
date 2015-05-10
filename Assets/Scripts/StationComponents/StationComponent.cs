using AsteroidBelt.ShipComponents;
using UnityEngine;

namespace AsteroidBelt.StationComponents
{
    public class StationComponent : MonoBehaviour
    {
        public ShipComponent.Direction ComponentDirection;
        public GameObject ParentStation;

        protected virtual void Start()
        {
            if (ParentStation == null)
            {
                return;
            }

            gameObject.transform.SetParent(ParentStation.transform, false);

            switch (ComponentDirection)
            {
                case ShipComponent.Direction.Up:
                    gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;

                case ShipComponent.Direction.Left:
                    gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    break;

                case ShipComponent.Direction.Down:
                    gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    break;

                case ShipComponent.Direction.Right:
                    gameObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
                    break;
            }
        }
    }
}