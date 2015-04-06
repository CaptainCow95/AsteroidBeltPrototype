using AsteroidBelt.ShipComponents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsteroidBelt
{
    public class Ship : MonoBehaviour
    {
        public bool playerControlled;
        public List<ShipComponent> shipComponents;
        private readonly FloatPid headingPid = new FloatPid(1f, 0f, 0.5f);
        private Rigidbody2D rigidBody;

        public void addShipComponent(ShipComponent shipComponent)
        {
            shipComponents.Add(shipComponent);
            Rigidbody2D rigidBody = gameObject.GetComponent<Rigidbody2D>();
            rigidBody.mass += shipComponent.mass;
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
                foreach (var item in shipComponents.Where(e => e is Thruster))
                {
                    Vector2 force = ((Thruster)item).GetThrust(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
                    rigidBody.AddForce(transform.rotation * force);
                }

                float maxTorque = shipComponents.Where(e => e is Gryoscope).Sum(item => ((Gryoscope)item).torque);

                Vector2 vec = Input.mousePosition - Camera.main.WorldToScreenPoint(gameObject.transform.position);
                var angle = Mathf.Atan2(-vec.x, vec.y) * Mathf.Rad2Deg;
                angle = rigidBody.rotation - angle;
                angle += angle > 180 ? -360 : angle < -180 ? 360 : 0;

                var headingError = -angle;
                var headingCorrection = headingPid.Update(headingError);

                float totalTorque = Mathf.Clamp(headingCorrection, -maxTorque, maxTorque);
                rigidBody.AddTorque(totalTorque);
            }
        }
    }
}