using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class Thruster : ShipComponent
    {
        public float force;
        private Animator animator;

        public Vector2 GetThrust(Vector2 dir)
        {
            if ((ComponentDirection == Direction.Down && dir.y < 0) || (ComponentDirection == Direction.Up && dir.y > 0))
            {
                if (ParentShip.GetComponent<Ship>().drawPower(-powerSupply * Time.deltaTime))
                {
                    animator.SetBool("ThrusterActive", true);
                    return new Vector2(0, dir.y * force);
                }
            }

            if ((ComponentDirection == Direction.Left && dir.x < 0) || (ComponentDirection == Direction.Right && dir.x > 0))
            {
                if (ParentShip.GetComponent<Ship>().drawPower(-powerSupply * Time.deltaTime))
                {
                    animator.SetBool("ThrusterActive", true);
                    return new Vector2(dir.x * force, 0);
                }
            }

            animator.SetBool("ThrusterActive", false);
            return Vector2.zero;
        }

        protected override void Start()
        {
            base.Start();
            animator = gameObject.GetComponent<Animator>();
        }
    }
}