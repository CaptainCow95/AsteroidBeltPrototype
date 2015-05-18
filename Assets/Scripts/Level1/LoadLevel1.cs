using AsteroidBelt.ShipComponents;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidBelt.Level1
{
    public class LoadLevel1 : MonoBehaviour
    {
        public List<int> asteroidRarities;
        public ShipComponentType[] stationComponents;
        public ShipComponent.Direction[] stationDirections;
        public Vector2 stationPosition;
        public Vector2[] stationPositions;
        private static bool firstTime = true;

        public void Reset()
        {
            firstTime = true;
        }

        private void Awake()
        {
            if (GameManager.Instance.ShipToLoad == null || GameManager.Instance.ShipToLoad.Count == 0)
            {
                GameManager.Instance.SpawnDefaultPlayerShip();
            }

            if (firstTime)
            {
                var station = GameManager.Instance.CreateStation(stationPosition, stationPositions, stationDirections, stationComponents);
                GameManager.Instance.CreateWaypoint(station.GetComponent<Rigidbody2D>().worldCenterOfMass);
                GameManager.Instance.GenerateRandomAsteroids(asteroidRarities, 5000, 1000, new Vector2(0f, 0f));
                firstTime = false;
            }
        }

        private void start()
        {
        }
    }
}