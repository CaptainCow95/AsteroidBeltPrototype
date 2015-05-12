using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class Thruster : ShipComponent
    {
        public float force;
        private Animator animator;

        public void FireThruster(float forceApplied)
        {
            float percentage = Mathf.Abs(forceApplied) / (force * Time.deltaTime);
            GameObject particleSystem = gameObject.transform.FindChild("ThrustParticles").gameObject;
            if (percentage > float.Epsilon)
            {
                //particleSystem.GetComponent<ParticleSystem>().Play();
                //  animator.SetBool("ThrusterActive", true);
                ParentShip.GetComponent<Ship>().drawPower(-powerSupply * percentage * Time.deltaTime);
            }
            else
            {
                // animator.SetBool("ThrusterActive", false);

                //particleSystem.GetComponent<ParticleSystem>().Pause();
            }

            var ps = particleSystem.GetComponent<ParticleSystem>();
            ps.emissionRate = percentage * 50;
        }

        public Vector2 GetThrust(Vector2 dir)
        {
            float availableThrustPercentage = Mathf.Clamp(ParentShip.GetComponent<Ship>().CurrentPower / (-this.powerSupply * Time.deltaTime), 0, 1);
            if ((ComponentDirection == Direction.Down && dir.y < 0) || (ComponentDirection == Direction.Up && dir.y > 0))
            {
                return new Vector2(0, dir.y * force * availableThrustPercentage * Time.deltaTime);
            }

            if ((ComponentDirection == Direction.Left && dir.x < 0) || (ComponentDirection == Direction.Right && dir.x > 0))
            {
                return new Vector2(dir.x * force * availableThrustPercentage * Time.deltaTime, 0);
            }

            // animator.SetBool("ThrusterActive", false);

            return Vector2.zero;
        }

        protected override void Start()
        {
            base.Start();

            animator = gameObject.GetComponent<Animator>();
            animator.SetBool("ThrusterActive", false);
        }
    }
}