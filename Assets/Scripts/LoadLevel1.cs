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
            GameManager.Instance.CreateAsteroid(new Vector2(5.0f, 0f), .01f, 16, 300);
        }
    }
}