using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public class MiningLaser : ShipComponent
    {
        public Vector2 castfromLocation;
        public float cooldown;
        public GameObject emptyObject;
        public AudioClip explosionSound;
        public GameObject laser;
        public int ParticlesPerBeamUnit;
        public float range;
        public AudioClip shootSound;
        public float yield;
        private bool canFire;
        private GameObject castfrom;
        private float highPitchRange = 1.25F;
        private float lowPitchRange = .85F;
        private AudioSource source;
        private float timeLeft;
        private float volHighRange = 1.0f;
        private float volLowRange = .5f;

        public void Fire()
        {
            if (canFire)
            {
                if (ParentShip.GetComponent<Ship>().drawPower(-powerSupply))
                {
                    canFire = false;
                    timeLeft = cooldown;
                    float distance = range;
                    float vol = Random.Range(volLowRange, volHighRange);
                    RaycastHit2D hit = Physics2D.Raycast(castfrom.transform.position, gameObject.transform.rotation * new Vector3(0, 1));
                    if (hit.collider != null)
                    {
                        if (hit.collider == null)
                            Debug.Log("collider is null");
                        GameObject objectHit = hit.collider.gameObject;
                        distance = Vector2.Distance(castfrom.transform.position, hit.point);

                        Asteroid asteroid = objectHit.GetComponent<Asteroid>();
                        if (asteroid != null && distance < range)
                        {
                            float amountForCargo = Mathf.Min(asteroid.MineralRating, yield);
                            asteroid.MineralRating -= yield;

                            Ore ore = asteroid.GetComponent<Ore>();
                            if (ore != null)
                            {
                                ParentShip.GetComponent<Ship>().inventory.PutItem(ore, (int)amountForCargo);
                            }

                            GameObject asteroidParticles = Instantiate(asteroid.particleSystemPrefab);

                            asteroidParticles.transform.position = hit.point;

                            asteroidParticles.transform.rotation = Quaternion.LookRotation((Vector3)hit.point - objectHit.transform.position, new Vector3(0, 0, 1));
                            asteroidParticles.GetComponent<ParticleSystem>().Emit((int)yield * 5);
                            source.pitch = Random.Range(lowPitchRange, highPitchRange);
                            source.PlayOneShot(explosionSound, vol);
                        }
                    }

                    //Laser laserComponent = laser.GetComponent<Laser>();
                    // laserComponent.laserRange = Mathf.Min(range, distance);
                    //  GameObject newLaser = Instantiate(laser, castfrom.transform.position, gameObject.transform.rotation) as GameObject;
                    float laserRange = Mathf.Min(range, distance);
                    GameObject particleSystem = gameObject.transform.FindChild("LaserParticleSysem").gameObject;
                    particleSystem.transform.localScale = new Vector3(particleSystem.transform.localScale.x, particleSystem.transform.localScale.y, laserRange);
                    particleSystem.GetComponent<ParticleSystem>().Emit((int)(ParticlesPerBeamUnit * laserRange));
                    vol = Random.Range(volLowRange, volHighRange);
                    source.pitch = Random.Range(lowPitchRange, highPitchRange);
                    source.PlayOneShot(shootSound, vol);
                }
            }
        }

        protected override void Start()
        {
            if (ParentShip == null)
            {
                return;
            }

            base.Start();
            castfrom = Instantiate(emptyObject);
            castfrom.transform.localPosition = castfromLocation;
            castfrom.transform.SetParent(gameObject.transform, false);
            source = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            base.Update();
            if (ParentShip == null)
            {
                return;
            }

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

        private void spawnLaser(Vector2 target, float range)
        {
        }
    }
}