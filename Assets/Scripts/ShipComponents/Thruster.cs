using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class Thruster : ShipComponent
    {
        public float force;

        public void FireThruster(float forceApplied)
        {
            float percentage = Mathf.Abs(forceApplied) / (force * Time.deltaTime);
            if (percentage > float.Epsilon)
            {
                ParentShip.GetComponent<Ship>().drawPower(-powerSupply * percentage * Time.deltaTime);
            }

            foreach (Transform child in transform)
            {
                ParticleSystem ps = child.gameObject.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.emissionRate = percentage * 100;
                }
            }

            AudioSource source = GetComponent<AudioSource>();
            source.volume = percentage / 2;
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

            return Vector2.zero;
        }
    }
}