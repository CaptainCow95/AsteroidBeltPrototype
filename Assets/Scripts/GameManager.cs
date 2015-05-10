using AsteroidBelt.ShipComponents;
using AsteroidBelt.StationComponents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsteroidBelt
{
    public class GameManager : Singleton<GameManager>
    {
        public List<GameObject> asteroidOptions;
        public GameObject asteroidPrefab;
        public float maxZoomOut;
        public GameObject[] shipComponentPrefabs;
        public GameObject shipPrefab;
        public GameObject[] stationComponentPrefabs;
        public GameObject stationPrefab;
        public int totalCredits;
        private List<ShipPart> ShipToLoad;

        public void CreateAsteroid(GameObject asteroidToInstantiate, Vector2 position, float radiusPerMineral, int numberOfVertices, float mineralRating)
        {
            GameObject asteroidObject = (GameObject)Instantiate(asteroidToInstantiate, position, Quaternion.identity);
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.radiusPerMineral = radiusPerMineral;
            asteroid.numberOfVertices = numberOfVertices;
            asteroid.MineralRating = mineralRating;
        }

        public void CreateAsteroid(Vector2 position, float radiusPerMineral, int numberOfVertices, float mineralRating)
        {
            CreateAsteroid(asteroidPrefab, position, radiusPerMineral, numberOfVertices, mineralRating);
        }

        public void CreateShip(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, ShipComponentType[] shipComponents, bool playerControlled)
        {
            GameObject shipObject = (GameObject)Instantiate(shipPrefab, position, Quaternion.identity);
            Ship ship = shipObject.GetComponent<Ship>();
            ship.playerControlled = playerControlled;
            for (int i = 0; i < shipComponents.Length; ++i)
            {
                GameObject newShipComponent = (GameObject)Instantiate(shipComponentPrefabs[(int)shipComponents[i]], componentPositions[i], Quaternion.identity);
                ShipComponent comp = newShipComponent.GetComponent<ShipComponent>();
                comp.ParentShip = shipObject;
                comp.ComponentDirection = componentDirections[i];
                ship.AddShipComponent(comp);
            }
        }

        public void CreateShip(Vector2 position, List<ShipPart> shipParts, bool playerControlled)
        {
            Vector2[] componentPositions = new Vector2[shipParts.Count];
            ShipComponent.Direction[] componentDirections = new ShipComponent.Direction[shipParts.Count];
            ShipComponentType[] shipComponents = new ShipComponentType[shipParts.Count];

            for (int i = 0; i < shipParts.Count; ++i)
            {
                componentPositions[i] = shipParts[i].Location;
                componentDirections[i] = shipParts[i].Direction;
                shipComponents[i] = shipParts[i].ShipComponent;
            }
            CreateShip(position, componentPositions, componentDirections, shipComponents, playerControlled);
        }

        public void CreateStation(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, ShipComponentType[] stationComponents)
        {
            GameObject stationObject = (GameObject)Instantiate(stationPrefab, position, Quaternion.identity);
            Station station = stationObject.GetComponent<Station>();
            for (int i = 0; i < stationComponents.Length; ++i)
            {
                GameObject newStationComponent = (GameObject)Instantiate(stationComponentPrefabs[(int)stationComponents[i]], componentPositions[i], Quaternion.identity);
                StationComponent component = newStationComponent.GetComponent<StationComponent>();
                component.ParentStation = stationObject;
                component.ComponentDirection = componentDirections[i];
                station.AddStationComponent(component);
            }
        }

        public void GenerateRandomAsteroids(List<int> asteroidRarities, int numberOfAsteroids, float range, Vector2 origin)
        {
            List<GameObject> weightedAsteroidList = new List<GameObject>();
            for (int i = 0; i < asteroidRarities.Count(); ++i)
            {
                for (int j = 0; j < asteroidRarities[i]; ++j)
                {
                    if (i < asteroidOptions.Count())
                    {
                        weightedAsteroidList.Add(asteroidOptions[i]);
                    }
                    else
                    {
                        Debug.LogError("More asteroid rarities that asteroids");
                    }
                }
            }
            for (int i = 0; i < numberOfAsteroids; ++i)
            {
                var randomAsteroid = weightedAsteroidList[Random.Range(0, weightedAsteroidList.Count())];
                Vector2 newPosition = new Vector2(Random.Range(-range, +range) + origin.x, Random.Range(-range, +range) + origin.y);
                CreateAsteroid(randomAsteroid, newPosition, .01f, 16, Random.Range(50f, 300f));
            }
        }

        public void SetShipToLoad(List<ShipPart> shipToLoad)
        {
            ShipToLoad = shipToLoad;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnLevelWasLoaded(int level)
        {
            if (level == 0 || level == 2) return;
            if (ShipToLoad != null)
            {
                CreateShip(new Vector2(0, 0), ShipToLoad, true);
                ShipToLoad = null;
            }
        }

        private void Update()
        {
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            var playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
            if (playerShip != null)
            {
                var pos = mainCamera.transform.position;
                pos.x = playerShip.transform.position.x;
                pos.y = playerShip.transform.position.y;
                mainCamera.transform.position = pos;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 1, maxZoomOut);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 3);
            }
        }
    }
}