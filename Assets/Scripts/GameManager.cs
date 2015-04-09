using AsteroidBelt.Assets.Scripts;
using AsteroidBelt.ShipComponents;
using UnityEngine;

namespace AsteroidBelt
{
    public class GameManager : Singleton<GameManager>
    {
        public GameObject asteroid;
        public GameObject emptyShip;
        public GameObject[] possibleShipComponents;

        public void CreateAsteroid(Vector2 position, float radius, int numberOfVertices, float mineralRating)
        {
            GameObject newAsteroid = Instantiate(asteroid, position, Quaternion.identity) as GameObject;
            Asteroid asteroidComponent = newAsteroid.GetComponent<Asteroid>();
            asteroidComponent.radius = radius;
            asteroidComponent.numberOfVertices = numberOfVertices;
            asteroidComponent.mineralRating = mineralRating;
        }

        public void CreateShip(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, int[] shipComponents, bool playerControlled)
        {
            GameObject newShip = Instantiate(emptyShip, position, Quaternion.identity) as GameObject;
            Ship ship = newShip.GetComponent<Ship>();
            ship.playerControlled = playerControlled;
            for (int i = 0; i < shipComponents.Length; ++i)
            {
                GameObject newShipComponent = Instantiate(possibleShipComponents[shipComponents[i]], componentPositions[i], Quaternion.identity) as GameObject;
                ShipComponent comp = newShipComponent.GetComponent<ShipComponent>();
                comp.parentShip = newShip;
                comp.direction = componentDirections[i];
                ship.addShipComponent(comp);
            }
        }
    }
}