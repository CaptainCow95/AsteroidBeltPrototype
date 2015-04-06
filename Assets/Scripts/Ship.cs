using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public List<ShipComponent> shipComponents;

    public void addShipComponent(ShipComponent shipComponent)
    {
        shipComponents.Add(shipComponent);
        Rigidbody2D rigidBody = gameObject.GetComponent("Rigidbody2D") as Rigidbody2D;
        rigidBody.mass += shipComponent.mass;
    }

    // Use this for initialization
    private void Start()
    {
        Rigidbody2D rigidBody = gameObject.GetComponent("Rigidbody2D") as Rigidbody2D;
        rigidBody.gravityScale = 0;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}