using AsteroidBelt.ShipComponents;
using UnityEngine;

namespace AsteroidBelt
{
    public class LoadLevel1 : MonoBehaviour
    {
        public int[] sampleComponents;
        public ShipComponent.Direction[] sampleDirections;
        public Vector2 samplePosition;
        public Vector2[] samplePositions;

        private void Awake()
        {
            GameManager.Instance.CreateShip(samplePosition, samplePositions, sampleDirections, sampleComponents, true);
        }
    }
}