using UnityEngine;
using UnityEngine.UI;

namespace AsteroidBelt
{
    public class Waypoint : MonoBehaviour
    {
        public Vector2 Location;

        public void Update()
        {
            var screenLocation = Camera.main.WorldToScreenPoint(Location);
            if (screenLocation.x < 0 || screenLocation.y < 0 || screenLocation.x > Screen.width || screenLocation.y > Screen.height)
            {
                GetComponent<Image>().enabled = true;
                float rotation = (180f / Mathf.PI) * Mathf.Atan2(Camera.main.transform.position.y - Location.y, Camera.main.transform.position.x - Location.x);
                GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, rotation);
                float circleRadius = Mathf.Min(Screen.width, Screen.height) * 0.3f;
                var vec = Quaternion.AngleAxis(rotation, Vector3.forward) * Vector3.left;
                vec.Normalize();
                vec *= circleRadius;
                GetComponent<RectTransform>().localPosition = vec;
            }
            else
            {
                GetComponent<Image>().enabled = false;
            }
        }
    }
}