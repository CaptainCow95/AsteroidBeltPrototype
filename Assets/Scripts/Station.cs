using AsteroidBelt.ShipComponents;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidBelt
{
    public class Station : MonoBehaviour
    {
        public static bool HandlingInteraction = false;
        public List<ShipComponent> stationComponents;
        private Rigidbody2D rigidBody;
        private GameObject stationInteraction;

        public void AddStationComponent(ShipComponent component)
        {
            stationComponents.Add(component);
        }

        // Use this for initialization
        private void Awake()
        {
            rigidBody = gameObject.GetComponent<Rigidbody2D>();
            stationInteraction = GameObject.FindGameObjectWithTag("StationInteraction");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!HandlingInteraction && collision.gameObject.tag == "PlayerShip")
            {
                HandlingInteraction = true;
                Time.timeScale = 0;
                int earned = 0;
                var inventory = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Ship>().inventory.GetInventory();
                foreach (var item in inventory)
                {
                    earned += item.Key.value * item.Value;
                }

                GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Ship>().inventory.ClearInventory();

                GameManager.Instance.totalCredits += earned;

                string text = "You have docked at the station. You have earned " + earned +
                              " credits from your cargo. You now have " + GameManager.Instance.totalCredits +
                              " credits available. Would you like to modify your ship?";
                stationInteraction.SetActive(true);
                GameObject.FindGameObjectWithTag("StationText").GetComponent<Text>().text = text;
            }
        }
    }
}