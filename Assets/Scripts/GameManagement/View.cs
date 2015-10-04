using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AsteroidBelt.GameManagement
{
    internal class View : MonoBehaviour
    {
        public int maxZoomOut;

        private Dictionary<int, GameObject> gameObjectByGrid;
        private Model gameModel;

        private void start()
        {
        }

        private void Update()
        {
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            var playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
            if (playerShip != null)
            {
                var pos = mainCamera.transform.position;
                Vector3 centerOfMass = playerShip.transform.rotation * (Vector3)playerShip.GetComponent<Rigidbody2D>().centerOfMass;

                pos.x = centerOfMass.x + playerShip.transform.position.x;
                pos.y = centerOfMass.y + playerShip.transform.position.y;
                mainCamera.transform.position = pos;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 1, maxZoomOut);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 3);
            }
        }
    }
}