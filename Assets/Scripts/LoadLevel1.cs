using AsteroidBelt.ShipComponents;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidBelt
{
    public class LoadLevel1 : MonoBehaviour
    {
        public List<int> asteroidRarities;
        public ShipComponentType[] sampleComponents;
        public ShipComponent.Direction[] sampleDirections;
        public Vector2 samplePosition;
        public Vector2[] samplePositions;

        private void Awake()
        {
            // GameManager.Instance.CreateShip(samplePosition, samplePositions, sampleDirections, sampleComponents, true);

            //GameManager.Instance.CreateAsteroid(new Vector2(5.0f, 0f), .01f, 16, 300);
            GameManager.Instance.generateRandomAsteroids(asteroidRarities, 5000, 1000, new Vector2(0f, 0f));
        }
    }
}