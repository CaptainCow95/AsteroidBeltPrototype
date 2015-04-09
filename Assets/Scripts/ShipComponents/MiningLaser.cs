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

                GameObject newLaser = Instantiate(laser, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
                Laser laserComponent = newLaser.GetComponent<Laser>();
            }
        }

        private void spawnLaser(Vector2 target, float range)
        {
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