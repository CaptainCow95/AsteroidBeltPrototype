using UnityEngine;
using UnityEngine.UI;

namespace AsteroidBelt
{
    public class UIShipValue : MonoBehaviour
    {
        // Update is called once per frame
        private void Update()
        {
            gameObject.GetComponent<Text>().text = "Ship Value:\n" + ShipEditor.Instance.GetShipValue();
        }
    }
}