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
        public List<AudioClip> backgroundAudioClips;
        public float maxZoomOut;
        public GameObject[] shipComponentPrefabs;
        public GameObject shipPrefab;
        public List<ShipPart> ShipToLoad;
        public List<ShipPart> ShipToLoadDefault;
        public GameObject[] stationComponentPrefabs;
        public GameObject stationPrefab;
        public int totalCredits;
        public GameObject WaypointPrefab;
        private List<GameObject> persistingObjects = new List<GameObject>();
        private Queue<AudioClip> playedAudioClips = new Queue<AudioClip>();

        public GameObject CreateAsteroid(GameObject asteroidToInstantiate, Vector2 position, float radiusPerMineral, int numberOfVertices, float mineralRating)
        {
            GameObject asteroidObject = (GameObject)Instantiate(asteroidToInstantiate, position, Quaternion.identity);
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.radiusPerMineral = radiusPerMineral;
            asteroid.numberOfVertices = numberOfVertices;
            asteroid.MineralRating = mineralRating;
            persistingObjects.Add(asteroidObject);
            DontDestroyOnLoad(asteroidObject);
            return asteroidObject;
        }

        public GameObject CreateAsteroid(Vector2 position, float radiusPerMineral, int numberOfVertices, float mineralRating)
        {
            return CreateAsteroid(asteroidPrefab, position, radiusPerMineral, numberOfVertices, mineralRating);
        }

        public GameObject CreateShip(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, ShipComponentType[] shipComponents, bool playerControlled)
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

            return shipObject;
        }

        public GameObject CreateShip(Vector2 position, List<ShipPart> shipParts, bool playerControlled)
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
            return CreateShip(position, componentPositions, componentDirections, shipComponents, playerControlled);
        }

        public GameObject CreateStation(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, ShipComponentType[] stationComponents)
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

            DontDestroyOnLoad(stationObject);
            persistingObjects.Add(stationObject);
            return stationObject;
        }

        public void CreateWaypoint(Vector2 location)
        {
            GameObject waypointObject = Instantiate(WaypointPrefab);
            waypointObject.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
            waypoint.Location = location;
            persistingObjects.Add(waypointObject);
            DontDestroyOnLoad(waypointObject);
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
                        Debug.LogError("More asteroid rarities than asteroids");
                    }
                }
            }

            for (int i = 0; i < numberOfAsteroids; ++i)
            {
                var randomAsteroid = weightedAsteroidList[Random.Range(0, weightedAsteroidList.Count())];
                Vector2 newPosition = new Vector2(Random.Range(-range, +range) + origin.x, Random.Range(-range, +range) + origin.y);
                var asteroidObject = CreateAsteroid(randomAsteroid, newPosition, .01f, 16, Random.Range(50f, 300f));
                var colliders = Physics2D.OverlapCircleAll(asteroidObject.transform.position, 1.1f * asteroidObject.transform.localScale.x);
                if (colliders.Length > 1)
                {
                    persistingObjects.Remove(asteroidObject);
                    DestroyImmediate(asteroidObject);
                    --i;
                }
            }
        }

        public void SetShipToLoad(List<ShipPart> shipToLoad)
        {
            ShipToLoad = shipToLoad;
        }

        public void SpawnDefaultPlayerShip()
        {
            CreateShip(new Vector2(0, 0), ShipToLoadDefault, true);
        }

        private void Awake()
        {
            if (instance == null || instance == this)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            PlayBackgroundMusic();
        }

        private void OnLevelWasLoaded(int level)
        {
            if (level == 0 || level == 2)
            {
                return;
            }

            if (ShipToLoad != null && ShipToLoad.Any())
            {
                CreateShip(new Vector2(0, 0), ShipToLoad, true);
                ShipToLoad = null;
            }
        }

        private void PlayBackgroundMusic()
        {
            int index = Random.Range(0, backgroundAudioClips.Count);
            while (playedAudioClips.Contains(backgroundAudioClips[index]))
            {
                index = Random.Range(0, backgroundAudioClips.Count);
            }

            GetComponent<AudioSource>().clip = backgroundAudioClips[index];
            GetComponent<AudioSource>().Play();
            playedAudioClips.Enqueue(backgroundAudioClips[index]);
            while (playedAudioClips.Count > backgroundAudioClips.Count / 2)
            {
                playedAudioClips.Dequeue();
            }
        }

        private void Update()
        {
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            var playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
            if (playerShip != null)
            {
                var pos = mainCamera.transform.position;
                Vector3 centerOfMass = playerShip.transform.rotation * (Vector3)playerShip.GetComponent<Rigidbody2D>().centerOfMass;

                pos.x = centerOfMass.x + playerShip.transform.position.x;
                pos.y = centerOfMass.y + playerShip.transform.position.y;
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

            if (!GetComponent<AudioSource>().isPlaying)
            {
                PlayBackgroundMusic();
            }
        }
    }
}