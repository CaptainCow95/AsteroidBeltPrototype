using System.Collections;
using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class MiningLaser : ShipComponent
    {
        public float yield;
        public float cooldown;
        public float range;
        public GameObject laser;

        bool canFire;
        float timeLeft;


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

        void spawnLaser(Vector2 target, float range)
        {

        }

        void Update()
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
