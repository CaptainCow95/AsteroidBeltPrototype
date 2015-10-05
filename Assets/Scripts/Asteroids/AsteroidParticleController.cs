using UnityEngine;

public class AsteroidParticleController : MonoBehaviour
{
    public float lifetime;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            GameObject.DestroyObject(gameObject);
        }
    }
}