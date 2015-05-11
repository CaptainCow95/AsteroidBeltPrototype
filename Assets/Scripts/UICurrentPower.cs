using UnityEngine;
using UnityEngine.UI;

namespace AsteroidBelt
{
    public class UICurrentPower : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            gameObject.GetComponent<Text>().text = "Current Power:\n" + GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Ship>().CurrentPower.ToString("#.0");
        }
    }
}