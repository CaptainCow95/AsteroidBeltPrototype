using AsteroidBelt.ShipComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsteroidBelt
{
    public class Ship : MonoBehaviour
    {
        public Inventory inventory;
        public bool playerControlled;
        public List<ShipComponent> shipComponents;
        private float capacity;
        private float currentPower;
        private float powerCapacity;

        private Rigidbody2D rigidBody;

        public float CurrentPower
        {
            get { return currentPower; }
        }

        public float PowerCapacity
        {
            get { return powerCapacity; }
        }

        public bool addPower(float power)
        {
            if (power > 0)
            {
                currentPower = Math.Min(powerCapacity, currentPower + power);
            }
            return false;
        }

        public void AddShipComponent(ShipComponent shipComponent)
        {
            shipComponents.Add(shipComponent);
            gameObject.GetComponent<Rigidbody2D>().mass += shipComponent.Mass;
            powerCapacity += shipComponent.powerCapacity;
        }

        public bool drawPower(float power)
        {
            if (power > 0 && currentPower - power > 0)
            {
                currentPower -= power;
                return true;
            }
            return false;
        }

        private void InitInventory()
        {
            inventory = new Inventory
            {
                Capacity = shipComponents.OfType<Cargo>().Sum(e => e.capacity)
            };
        }

        private void Start()
        {
            InitInventory();
            rigidBody = gameObject.GetComponent<Rigidbody2D>();

            if (playerControlled)
            {
                gameObject.tag = "PlayerShip";
            }
        }

        private void Update()
        {
            if (playerControlled)
            {
                float shipMass = 0;
                float maxTorque = 0;
                foreach (var item in shipComponents)
                {
                    shipMass += item.Mass;

                    if (item is Thruster)
                    {
                        Vector2 force = ((Thruster)item).GetThrust(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
                        ((Thruster)item).FireThruster(force.magnitude);
                        rigidBody.AddRelativeForce(force);
                    }
                    else if (item is Gryoscope)
                    {
                        maxTorque += ((Gryoscope)item).torque;
                    }
                    else if (item is MiningLaser)
                    {
                        if (Input.GetMouseButton(1))
                        {
                            ((MiningLaser)item).Fire();
                        }
                    }
                }

                GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                Vector2 vec = Input.mousePosition - Camera.main.WorldToScreenPoint(mainCamera.transform.position);
                float angle = Mathf.Atan2(-vec.x, vec.y) * Mathf.Rad2Deg;
                angle = rigidBody.rotation % 360 - angle;
                angle += angle < -180 ? 360 : angle > 180 ? -360 : 0;

                // distance = (v0 + vf) / 2 * time where time is velocity / acceleration where acceleration is torque / mass
                float distance = (rigidBody.angularVelocity / 2) * Mathf.Abs(rigidBody.angularVelocity / (maxTorque / shipMass));

                if (Mathf.Abs(distance) > Mathf.Abs(angle))
                {
                    // slow down
                    rigidBody.AddTorque(maxTorque * Time.deltaTime * ((rigidBody.angularVelocity < 0) ? 1 : -1));
                }
                else
                {
                    // speed up
                    rigidBody.AddTorque(maxTorque * Time.deltaTime * ((angle < 0) ? 1 : -1));
                }

                if (Math.Abs(Input.GetAxisRaw("Horizontal")) < float.Epsilon && Math.Abs(Input.GetAxisRaw("Vertical")) < float.Epsilon)
                {
                    // No ship movement input detected, slowing ship down
                    Vector2 velocity = transform.InverseTransformDirection(rigidBody.velocity);

                    if (Math.Abs(velocity.x) < 0.00001f)
                    {
                        velocity.x = 0;
                        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                    }

                    if (Math.Abs(velocity.y) < 0.00001f)
                    {
                        velocity.y = 0;
                        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
                    }

                    Vector2 forceToFire = velocity * shipMass;
                    forceToFire = new Vector2(Mathf.Abs(forceToFire.x), Mathf.Abs(forceToFire.y));

                    foreach (var item in shipComponents)
                    {
                        if (!(item is Thruster))
                        {
                            continue;
                        }

                        if (item.ComponentDirection == ShipComponent.Direction.Up && velocity.y < -0.00001f)
                        {
                            float thrustForce = ((Thruster)item).GetThrust(new Vector2(0, 1)).y;
                            float force = Mathf.Min(thrustForce, forceToFire.y);
                            ((Thruster)item).FireThruster(force);
                            forceToFire.y -= force;
                            rigidBody.AddRelativeForce(new Vector2(0, force));
                        }
                        else if (item.ComponentDirection == ShipComponent.Direction.Down && velocity.y > 0.00001f)
                        {
                            float thrustForce = ((Thruster)item).GetThrust(new Vector2(0, -1)).y;
                            float force = Mathf.Min(Mathf.Abs(thrustForce), forceToFire.y);
                            ((Thruster)item).FireThruster(force);
                            forceToFire.y -= force;
                            rigidBody.AddRelativeForce(new Vector2(0, -force));
                        }
                        else if (item.ComponentDirection == ShipComponent.Direction.Left && velocity.x > 0.00001f)
                        {
                            float thrustForce = ((Thruster)item).GetThrust(new Vector2(-1, 0)).x;
                            float force = Mathf.Min(Mathf.Abs(thrustForce), forceToFire.x);
                            ((Thruster)item).FireThruster(force);
                            forceToFire.x -= force;
                            rigidBody.AddRelativeForce(new Vector2(-force, 0));
                        }
                        else if (item.ComponentDirection == ShipComponent.Direction.Right && velocity.x < -0.00001f)
                        {
                            float thrustForce = ((Thruster)item).GetThrust(new Vector2(1, 0)).x;
                            float force = Mathf.Min(thrustForce, forceToFire.x);
                            ((Thruster)item).FireThruster(force);
                            forceToFire.x -= force;
                            rigidBody.AddRelativeForce(new Vector2(force, 0));
                        }
                    }
                }
            }
        }
    }
}