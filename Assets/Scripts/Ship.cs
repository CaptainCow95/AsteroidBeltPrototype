using UnityEngine;
using System.Collections.Generic;

public class Ship : MonoBehaviour
{

    public List<ShipComponent> shipComponents;

    public void addShipComponent(ShipComponent shipComponent)
    {
        shipComponents.Add(shipComponent);
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}