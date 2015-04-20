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

		private void Update()
		{
			var obj = GameObject.FindGameObjectWithTag("PlayerShip");
			if (obj != null)
			{
				var pos = transform.position;
				pos.x = obj.transform.position.x;
				pos.y = obj.transform.position.y;
				transform.position = pos;
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