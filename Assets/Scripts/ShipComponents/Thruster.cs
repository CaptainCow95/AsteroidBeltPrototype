using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class Thruster : ShipComponent
    {
        public float force;
        private Animator animator;

        public Vector2 GetThrust(Vector2 dir)
        {
            switch (direction)
            {
                case Direction.Down:
                    if (dir.y < 0)
                    {
                        animator.SetBool("ThrusterActive", true);
                        return new Vector2(0, dir.y * force);
                    }
                    else
                    {
                        animator.SetBool("ThrusterActive", false);
                        return Vector2.zero;
                    }
                case Direction.Left:
                    if (dir.x < 0)
                    {
                        animator.SetBool("ThrusterActive", true);
                        return new Vector2(dir.x * force, 0);
                    }
                    else
                    {
                        animator.SetBool("ThrusterActive", false);
                        return Vector2.zero;
                    }
                case Direction.Right:
                    if (dir.x > 0)
                    {
                        animator.SetBool("ThrusterActive", true);
                        return new Vector2(dir.x * force, 0);
                    }
                    else
                    {
                        animator.SetBool("ThrusterActive", false);
                        return Vector2.zero;
                    }
                case Direction.Up:
                    if (dir.y > 0)
                    {
                        animator.SetBool("ThrusterActive", true);
                        return new Vector2(0, dir.y * force);
                    }
                    else
                    {
                        animator.SetBool("ThrusterActive", false);
                        return Vector2.zero;
                    }
                default:
                    animator.SetBool("ThrusterActive", false);
                    return Vector2.zero;
            }
        }

        protected override void Start()
        {
            base.Start();
            animator = gameObject.GetComponent<Animator>();
        }
    }
}