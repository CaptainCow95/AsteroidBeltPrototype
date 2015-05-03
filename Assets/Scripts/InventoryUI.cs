using UnityEngine;

using UnityEngine.UI;

namespace AsteroidBelt
{
    public class InventoryUI : MonoBehaviour
    {
        private Inventory inventory;
        private GameObject playerShip;
        private Text text;

        // Update is called once per frame
        private void Update()
        {
            playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
            text = gameObject.GetComponentInChildren<Text>();
            inventory = playerShip.GetComponent<Ship>().inventory;
            text.text = "Inventory: \n" + inventory.ToString();
        }
    }
}