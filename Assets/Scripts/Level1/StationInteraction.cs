using AsteroidBelt;
using AsteroidBelt.ShipComponents;
using System.Collections.Generic;
using UnityEngine;

public class StationInteraction : MonoBehaviour
{
    public void NoPressed()
    {
        GameObject.FindGameObjectWithTag("PlayerShip").transform.position = new Vector3(-20, -20, 0);
        GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Station.HandlingInteraction = false;
    }

    public void YesPressed()
    {
        List<ShipPart> shipParts = new List<ShipPart>();
        foreach (var item in GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Ship>().shipComponents)
        {
            shipParts.Add(new ShipPart(item.componentType, item.transform.localPosition, item.ComponentDirection));

            // GameManager.Instance.totalCredits += item.value;
        }

        GameManager.Instance.SetShipToLoad(shipParts);

        Time.timeScale = 1;
        Station.HandlingInteraction = false;
        Application.LoadLevel(2);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
}