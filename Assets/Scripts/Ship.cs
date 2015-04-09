using AsteroidBelt.ShipComponents;
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
                        rigidBody.AddForce(transform.rotation * force * Time.deltaTime);
                    }
                    else if (item is Gryoscope)
                    {
                        maxTorque += ((Gryoscope)item).torque;
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
            }
        }
    }
}