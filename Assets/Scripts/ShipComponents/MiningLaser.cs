using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
	public class MiningLaser : ShipComponent
	{
		public Vector2 castfromLocation;
		public float cooldown;
		public GameObject emptyObject;
		public GameObject laser;
		public float range;
		public float yield;
		private bool canFire;
		private GameObject castfrom;
		private float timeLeft;

		public void Fire()
		{
			if (canFire)
			{
				canFire = false;
				timeLeft = cooldown;
				float distance = range;

				RaycastHit2D hit = Physics2D.Raycast(castfrom.transform.position, gameObject.transform.rotation * new Vector3(0, 1));
				if (hit.collider != null)
				{
					if (hit.collider == null)
						Debug.Log("collider is null");
					GameObject objectHit = hit.collider.gameObject;
					distance = Vector2.Distance(castfrom.transform.position, hit.point);

					Asteroid asteroid = objectHit.GetComponent<Asteroid>();
					if (asteroid != null)
					{
						float amountForCargo = Mathf.Min(asteroid.mineralRating, yield);
						asteroid.mineralRating -= yield;
					}
				}
				Laser laserComponent = laser.GetComponent<Laser>();
				laserComponent.laserRange = Mathf.Min(range, distance);
				GameObject newLaser = Instantiate(laser, castfrom.transform.position, gameObject.transform.rotation) as GameObject;
			}
		}

		protected override void Start()
		{
			if (parentShip == null)
			{
				return;
			}

			base.Start();
			castfrom = Instantiate(emptyObject);
			castfrom.transform.localPosition = castfromLocation;
			castfrom.transform.SetParent(gameObject.transform, false);
		}

		protected override void Update()
		{
			if (parentShip == null)
			{
				return;
			}

			base.Update();
			if (!canFire)
			{
				timeLeft -= Time.deltaTime;
				if (timeLeft <= 0)
				{
					canFire = true;
				}
			}
		}

		private void spawnLaser(Vector2 target, float range)
		{
		}
	}
}