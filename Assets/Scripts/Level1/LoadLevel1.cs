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

        private void Awake()
        {
            GameManager.Instance.GenerateRandomAsteroids(asteroidRarities, 5000, 1000, new Vector2(0f, 0f));
            GameManager.Instance.CreateStation(stationPosition, stationPositions, stationDirections, stationComponents);
        }
    }
}