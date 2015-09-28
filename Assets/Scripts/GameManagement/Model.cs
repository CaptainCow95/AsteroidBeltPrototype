using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidBelt.MVC
{
    public class Model
    {
        private float gridsPerScreenSize = .25f;
        private int _randomSeed;
        private Grid[] grids;
        private float _maxViewWidth;
        private float _maxViewHeight;
        private float _gridWidth;
        private float _gridHeight;
        private float _worldSize;
        private int _numberOfHorizontalGrids;
        private int _numberOfVerticalGrids;
        private int maxAsteroidDenisty;

        public Model(int randomSeed, float maxViewWidth, float maxViewHeight, float worldSize)
        {
            _randomSeed = randomSeed;
            _maxViewHeight = maxViewHeight;
            _maxViewWidth = maxViewWidth;
            _gridHeight = gridsPerScreenSize * _maxViewHeight;
            _gridWidth = gridsPerScreenSize * _maxViewWidth;
            _worldSize = worldSize;

            int _numberOfHorizontalGrids = (int)Math.Ceiling(_worldSize / _gridWidth);
            int _numberOfVerticalGrids = (int)Math.Ceiling(_worldSize / _gridHeight);

            grids = new Grid[_numberOfHorizontalGrids * _numberOfVerticalGrids];
        }

        public int WorldToGridIndex(Vector2 worldCoordinates)
        {
            int x = (int)(worldCoordinates.x / _gridWidth);
            int y = (int)(worldCoordinates.y / _gridHeight);
            return coordsToIndex(x, y);
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
            return worldCoords;
        }

        public void generateGrid(int index)
        {
            int n = 4;
            int randomSeed = _randomSeed + (index * 522);
            UnityEngine.Random.seed = randomSeed;
            int max = maxAsteroidDenisty / n;
            int numberOfAsteroids = 1;
            for (int i = 0; n < 4; ++i)
            {
                numberOfAsteroids *= UnityEngine.Random.Range(0, max + 1);
            }

            float rangeX = _gridWidth / 2f;
            float rangeY = _gridHeight / 2f;
            Vector2 origin = gridToWorldCoords(index);

            grids[index].asteroidDataDictionary = new Dictionary<int, AsteroidData>();
            for (int i = 0; i < numberOfAsteroids; ++i)
            {
                AsteroidData data;
                data.mineralRating = UnityEngine.Random.Range(50f, 300f);
                data.xPosition = UnityEngine.Random.Range(-rangeX, +rangeX) + origin.x;
                data.yPostition = UnityEngine.Random.Range(-rangeY, +rangeY) + origin.y;
                data.oreType = InventoryItem.ItemType.IronOre; //TODO: change once asteroids are multi
                grids[index].asteroidDataDictionary.Add(i, data);
            }
        }

        public Dictionary<int, Dictionary<int, AsteroidData>> getNearbyAsteroids(Vector2 worldCoords)
        {
            Dictionary<int, Dictionary<int, AsteroidData>> resultSet = new Dictionary<int, Dictionary<int, AsteroidData>>();
            int x = (int)(worldCoords.x / _gridWidth);
            int y = (int)(worldCoords.y / _gridHeight);
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

    internal struct Grid
    {
        public bool visited;
        public bool generated;
        public bool inMemory;
        public Dictionary<int, AsteroidData> asteroidDataDictionary;
    }
}