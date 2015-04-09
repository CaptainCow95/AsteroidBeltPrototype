using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class Thruster : ShipComponent
    {
        public float force;
        private Animator animator;

        public Vector2 GetThrust(Vector2 dir)
        {
            if ((direction == Direction.Down && dir.y < 0) || (direction == Direction.Up && dir.y > 0))
            {
                animator.SetBool("ThrusterActive", true);
                return new Vector2(0, dir.y * force);
            }

            if ((direction == Direction.Left && dir.x < 0) || (direction == Direction.Right && dir.x > 0))
            {
                animator.SetBool("ThrusterActive", true);
                return new Vector2(dir.x * force, 0);
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