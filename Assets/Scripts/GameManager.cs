using AsteroidBelt.Assets.Scripts;
using AsteroidBelt.ShipComponents;
using UnityEngine;

namespace AsteroidBelt
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject asteroidPrefab;
        public GameObject[] shipComponentPrefabs;
        public GameObject shipPrefab;

        public void CreateAsteroid(Vector2 position, float radius, int numberOfVertices, float mineralRating)
        {
            GameObject asteroidObject = Instantiate(asteroidPrefab, position, Quaternion.identity) as GameObject;
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.radius = radius;
            asteroid.numberOfVertices = numberOfVertices;
            asteroid.mineralRating = mineralRating;
        }

        public void CreateShip(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, int[] shipComponents, bool playerControlled)
        {
            GameObject shipObject = Instantiate(shipPrefab, position, Quaternion.identity) as GameObject;
            Ship ship = shipObject.GetComponent<Ship>();
            ship.playerControlled = playerControlled;
            for (int i = 0; i < shipComponents.Length; ++i)
            {
                GameObject newShipComponent = Instantiate(shipComponentPrefabs[shipComponents[i]], componentPositions[i], Quaternion.identity) as GameObject;
                ShipComponent comp = newShipComponent.GetComponent<ShipComponent>();
                comp.parentShip = shipObject;
                comp.direction = componentDirections[i];
                ship.addShipComponent(comp);
            }
        }
    }
}