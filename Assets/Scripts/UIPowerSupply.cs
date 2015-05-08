using UnityEngine;
using UnityEngine.UI;

namespace AsteroidBelt
{
    public class UIPowerSupply : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            gameObject.GetComponent<Text>().text = "Power Supply:\n" + GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Ship>().PowerCapacity;
        }
    }
}