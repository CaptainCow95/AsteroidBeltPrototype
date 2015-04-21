using AsteroidBelt.Assets.Scripts;
using AsteroidBelt.ShipComponents;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidBelt
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject asteroidPrefab;
        public GameObject[] shipComponentPrefabs;
        public GameObject shipPrefab;
        private List<ShipPart> ShipToLoad;

        public void CreateAsteroid(Vector2 position, float radiusPerMineral, int numberOfVertices, float mineralRating)
        {
            GameObject asteroidObject = Instantiate(asteroidPrefab, position, Quaternion.identity) as GameObject;
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.radiusPerMineral = radiusPerMineral;
            asteroid.numberOfVertices = numberOfVertices;
            asteroid.mineralRating = mineralRating;
        }

        public void CreateShip(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, ShipComponentType[] shipComponents, bool playerControlled)
        {
            GameObject shipObject = Instantiate(shipPrefab, position, Quaternion.identity) as GameObject;
            Ship ship = shipObject.GetComponent<Ship>();
            ship.playerControlled = playerControlled;
            for (int i = 0; i < shipComponents.Length; ++i)
            {
                GameObject newShipComponent = Instantiate(shipComponentPrefabs[(int)shipComponents[i]], componentPositions[i], Quaternion.identity) as GameObject;
                ShipComponent comp = newShipComponent.GetComponent<ShipComponent>();
                comp.ParentShip = shipObject;
                comp.ComponentDirection = componentDirections[i];
                ship.addShipComponent(comp);
            }
        }

        public void CreateShip(Vector2 position, List<ShipComponents.ShipPart> shipParts, bool playerControlled)
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
                Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 1, 20);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 3);
            }
        }
    }
}