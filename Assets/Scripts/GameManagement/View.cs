using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AsteroidBelt.GameManagement
{
    internal class View : MonoBehaviour
    {
        public int maxZoomOut;
        public List<GameObject> asteroidPrefabs;

        private Dictionary<int, List<GameObject>> gameObjectsByGrid;
        private Model gameModel;
        private int oldGrid;
        public int worldSideLength;

        private static string filename = "testSaveFile.sav";

        public void SaveLevel()
        {
            Model.SaveModel(filename);
        }

        public void LoadLevel()
        {
            gameModel = null;
            Model.LoadModel(filename);
            Refresh();
        }

        private void Awake()
        {
            gameObjectsByGrid = new Dictionary<int, List<GameObject>>();
            oldGrid = -1;
        }

        private void Start()
        {
            Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            float height = maxZoomOut * 2;
            float width = height * camera.aspect;
            gameModel = Model.GetInstance(UnityEngine.Random.Range(int.MinValue, int.MaxValue), width, height, worldSideLength);
        }

        public void Refresh()
        {
            List<int> keys = gameObjectsByGrid.Keys.ToList();
            foreach (var key in keys)
            {
                unloadGrid(key);
            }
            gameModel = Model.GetInstance();
            int currentGrid = gameModel.WorldToGridIndex(gameObject.transform.position);
            LoadNewAsteroids();
            oldGrid = currentGrid;
        }

        private void CreateAsteroid(int grid, int id, AsteroidData data)
        {
            GameObject asteroidPrefab = asteroidPrefabs[(int)data.oreType];
            GameObject asteroidObject = (GameObject)Instantiate(asteroidPrefab, new Vector2(data.xPosition, data.yPostition), Quaternion.identity);
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.InitValues(grid, id, 16, data.mineralRating);
            if (!gameObjectsByGrid.ContainsKey(grid)) { gameObjectsByGrid.Add(grid, new List<GameObject>()); }
            gameObjectsByGrid[grid].Add(asteroidObject);
        }

        private void LoadNewAsteroids()
        {
            var loadedGrids = gameObjectsByGrid.Keys;
            var nearbyAsteroids = gameModel.getNearbyAsteroids(gameObject.transform.position);
            List<int> gridsToLoad = nearbyAsteroids.Keys.ToList();
            List<int> gridsToUnload = new List<int>();
            foreach (int i in gameObjectsByGrid.Keys)
            {
                if (!nearbyAsteroids.ContainsKey(i))
                {
                    gridsToUnload.Add(i);
                }
                else
                {
                    gridsToLoad.Remove(i);
                }
            }

            foreach (int grid in gridsToUnload)
            {
                unloadGrid(grid);
            }

            foreach (int grid in gridsToLoad)
            {
                gameObjectsByGrid.Add(grid, new List<GameObject>());
                var asteroidDataDictionary = nearbyAsteroids[grid];
                foreach (int id in asteroidDataDictionary.Keys)
                {
                    CreateAsteroid(grid, id, asteroidDataDictionary[id]);
                }
            }
        }

        private void unloadGrid(int grid)
        {
            foreach (GameObject asteroid in gameObjectsByGrid[grid])
            {
                Destroy(asteroid);
            }
            gameObjectsByGrid.Remove(grid); //TODO: do i need to destroy the held object seperately? we will find out.
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

            //Check if we need to load more grids
            int currentGrid = gameModel.WorldToGridIndex(gameObject.transform.position);
            if (currentGrid != oldGrid)
            {
                LoadNewAsteroids();
                oldGrid = currentGrid;
            }
        }
    }
}