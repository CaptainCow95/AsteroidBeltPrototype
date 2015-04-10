using AsteroidBelt.ShipComponents;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidBelt
{
    public class Ship : MonoBehaviour
    {
        public bool playerControlled;
        public List<ShipComponent> shipComponents;
        private Rigidbody2D rigidBody;

        public void addShipComponent(ShipComponent shipComponent)
        {
            shipComponents.Add(shipComponent);
            gameObject.GetComponent<Rigidbody2D>().mass += shipComponent.mass;
        }

        private void Start()
        {
            rigidBody = gameObject.GetComponent<Rigidbody2D>();
            rigidBody.gravityScale = 0;

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
                    shipMass += item.mass;

                    if (item is Thruster)
                    {
                        Vector2 force = ((Thruster)item).GetThrust(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
                        rigidBody.AddRelativeForce(force * Time.deltaTime);
                    }
                    else if (item is Gryoscope)
                    {
                        maxTorque += ((Gryoscope)item).torque;
                    }
                    else if (item is MiningLaser)
                    {
                        if (Input.GetMouseButtonDown(1))
                        {
                            ((MiningLaser)item).Fire();
                        }
                    }
                }

                Vector2 vec = Input.mousePosition - Camera.main.WorldToScreenPoint(gameObject.transform.position);
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

                        if (item.direction == ShipComponent.Direction.Up && velocity.y < -0.00001f)
                        {
                            float force = Mathf.Min(((Thruster)item).GetThrust(new Vector2(0, 1)).y * Time.deltaTime, forceToFire.y);
                            forceToFire.y -= force;
                            rigidBody.AddRelativeForce(new Vector2(0, force));
                        }
                        else if (item.direction == ShipComponent.Direction.Down && velocity.y > 0.00001f)
                        {
                            float force = Mathf.Min(((Thruster)item).GetThrust(new Vector2(0, -1)).y * Time.deltaTime, forceToFire.y);
                            forceToFire.y -= force;
                            rigidBody.AddRelativeForce(new Vector2(0, force));
                        }
                        else if (item.direction == ShipComponent.Direction.Left && velocity.x > 0.00001f)
                        {
                            float force = Mathf.Min(((Thruster)item).GetThrust(new Vector2(-1, 0)).x * Time.deltaTime, forceToFire.x);
                            forceToFire.x -= force;
                            rigidBody.AddRelativeForce(new Vector2(force, 0));
                        }
                        else if (item.direction == ShipComponent.Direction.Right && velocity.x < -0.00001f)
                        {
                            float force = Mathf.Min(((Thruster)item).GetThrust(new Vector2(1, 0)).x * Time.deltaTime, forceToFire.x);
                            forceToFire.x -= force;
                            rigidBody.AddRelativeForce(new Vector2(force, 0));
                        }
                    }
                }
            }
        }
    }
}