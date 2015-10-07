using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace AsteroidBelt.GameManagement
{
    [Serializable]
    public class Model
    {
        private static Model instance;
        private float gridsPerScreenSize = 1f; //for demonstrative purposes, should be set to 1 for final game. Can be set to a fraction to watch asteroids load and unload on-camera
        private int _randomSeed;
        private Grid[] grids;
        private float _maxViewWidth;
        private float _maxViewHeight;
        private float _gridWidth;
        private float _gridHeight;
        private float _worldSize;
        private int _numberOfHorizontalGrids;
        private int _numberOfVerticalGrids;
        private float maxNumberOfAsteroids = 20;

        public static void SaveModel(String filename)
        {
            if (instance == null)
            {
                throw new Exception("Model had no isntantiated instance when save was called.");
            }
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, instance);
            stream.Close();
        }

        public static void LoadModel(String filename)
        {
            GC.Collect();
            instance = null;
            using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                IFormatter formatter = new BinaryFormatter();
                instance = (Model)formatter.Deserialize(stream);
                stream.Close();
            }
            GC.Collect();
        }

        public void DeleteAsteroid(int grid, int id)
        {
            grids[grid].asteroidDataDictionary.Remove(id);
        }

        public void UpdateMineral(int grid, int id, float mineralRating)
        {
            grids[grid].asteroidDataDictionary[id].mineralRating = mineralRating;
        }

        public static Model GetInstance()
        {
            return instance;
        }

        public static Model GetInstance(int randomSeed, float maxViewWidth, float maxViewHeight, float worldSize)
        {
            if (instance == null)
            {
                instance = new Model(randomSeed, maxViewWidth, maxViewHeight, worldSize);
            }
            return instance;
        }

        private Model(int randomSeed, float maxViewWidth, float maxViewHeight, float worldSize)
        {
            _randomSeed = randomSeed;
            _maxViewHeight = maxViewHeight;
            _maxViewWidth = maxViewWidth;
            _gridHeight = gridsPerScreenSize * _maxViewHeight;
            _gridWidth = gridsPerScreenSize * _maxViewWidth;
            _worldSize = worldSize;

            _numberOfHorizontalGrids = (int)Math.Ceiling(_worldSize / _gridWidth);
            _numberOfVerticalGrids = (int)Math.Ceiling(_worldSize / _gridHeight);

            grids = new Grid[_numberOfHorizontalGrids * _numberOfVerticalGrids];
            Debug.Log("Size of grids array is " + sizeof(int) * 4 * _numberOfHorizontalGrids * _numberOfVerticalGrids / (1028 * 1028) + "MB");
        }

        public int WorldToGridIndex(Vector2 worldCoordinates)
        {
            IntVector2 coords = WorldToIntCoords(worldCoordinates);
            return coordsToIndex(coords.x, coords.y);
        }

        public IntVector2 WorldToIntCoords(Vector2 worldCoordinates)
        {
            IntVector2 coords;
            coords.x = (int)((worldCoordinates.x + (_worldSize / 2)) / _gridWidth);
            coords.y = (int)((worldCoordinates.y + (_worldSize / 2)) / _gridHeight);
            return coords;
        }

        public int coordsToIndex(int x, int y)
        {
            return _numberOfHorizontalGrids * y + x;
        }

        public Vector2 gridToWorldCoords(int index)
        {
            int x = index % _numberOfHorizontalGrids;
            int y = index / _numberOfHorizontalGrids;
            Vector2 worldCoords = new Vector2(x * _gridWidth + .5f * _gridWidth, y * _gridHeight + .5f * _gridHeight);
            worldCoords.x -= (_worldSize / 2);
            worldCoords.y -= (_worldSize / 2);
            return worldCoords;
        }

        public void generateGrid(int index)
        {
            int n = 4;
            int randomSeed = _randomSeed + (index * 522);
            UnityEngine.Random.seed = randomSeed;
            float numberOfAsteroids = 1;
            for (int i = 0; i < n; ++i)
            {
                numberOfAsteroids += UnityEngine.Random.Range(0f, maxNumberOfAsteroids / 4 + 1f);
            }

            float rangeX = _gridWidth / 2f;
            float rangeY = _gridHeight / 2f;
            Vector2 origin = gridToWorldCoords(index);

            grids[index].asteroidDataDictionary = new Dictionary<int, AsteroidData>();
            for (int i = 0; i < numberOfAsteroids; ++i)
            {
                AsteroidData data = new AsteroidData();
                data.mineralRating = UnityEngine.Random.Range(50f, 300f);
                data.xPosition = UnityEngine.Random.Range(-rangeX, +rangeX) + origin.x;
                data.yPostition = UnityEngine.Random.Range(-rangeY, +rangeY) + origin.y;
                data.oreType = InventoryItem.ItemType.IronOre; //TODO: change once asteroids are multi
                grids[index].asteroidDataDictionary.Add(i, data);
            }
            grids[index].generated = true;
        }

        public Dictionary<int, Dictionary<int, AsteroidData>> getNearbyAsteroids(Vector2 worldCoords)
        {
            Dictionary<int, Dictionary<int, AsteroidData>> resultSet = new Dictionary<int, Dictionary<int, AsteroidData>>();
            var coords = WorldToIntCoords(worldCoords);
            int x = coords.x;
            int y = coords.y;
            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    if (x > 0 && y > 0 && x < _numberOfHorizontalGrids && y < _numberOfVerticalGrids)
                    {
                        int curX = x + i;
                        int curY = y + j;
                        int index = coordsToIndex(curX, curY);
                        if (!grids[index].generated)
                        {
                            generateGrid(index);
                        }
                        resultSet.Add(index, grids[index].asteroidDataDictionary);
                    }
                }
            }
            return resultSet;
        }
    }

    public struct IntVector2
    {
        public int x;
        public int y;
    }

    [Serializable]
    internal struct Grid
    {
        public bool visited;
        public bool generated;
        public bool inMemory;
        public Dictionary<int, AsteroidData> asteroidDataDictionary;
    }
}