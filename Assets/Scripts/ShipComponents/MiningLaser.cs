using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class MiningLaser : ShipComponent
    {
        public float cooldown;
        public GameObject laser;
        public float range;
        public float yield;
        private bool canFire;
        private float timeLeft;

        public void Fire()
        {
            if (canFire)
            {
                canFire = false;
                timeLeft = cooldown;
                float distance = range;

                RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, gameObject.transform.rotation * new Vector3(0, 1));
                if (hit.collider != null)
                {
                    if (hit.collider == null)
                        Debug.Log("collider is null");
                    GameObject objectHit = hit.collider.gameObject;
                    distance = Vector2.Distance(gameObject.transform.position, hit.point);
                }
                Laser laserComponent = laser.GetComponent<Laser>();
                laserComponent.laserRange = Mathf.Min(range, distance);
                GameObject newLaser = Instantiate(laser, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            }
        }

        private void spawnLaser(Vector2 target, float range)
        {
        }

        private void start()
        {
            base.Start();
        }

        private void Update()
        {
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
    }
}