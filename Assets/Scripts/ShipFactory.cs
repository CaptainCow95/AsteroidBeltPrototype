using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFactory : MonoBehaviour
{
    public GameObject[] possibleShipComponents;
    public GameObject emptyShip;

    public Vector2[] samplePositions;
    public ShipComponent.Direction[] sampleDirections;
    public int[] sampleComponents;
    public Vector2 samplePosition;

    public void CreateShip(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, int[] shipComponents)
    {
        GameObject newShip;
        newShip = Instantiate(emptyShip, position, Quaternion.identity) as GameObject;
        for (int i = 0; i < shipComponents.Length; ++i)
        {
            GameObject newShipComponent = Instantiate(possibleShipComponents[shipComponents[i]], componentPositions[i], Quaternion.identity) as GameObject;
            ShipComponent comp = newShipComponent.GetComponent("ShipComponent") as ShipComponent;
            comp.parentShip = newShip;
            comp.direction = componentDirections[i];
        }
    }

    // Use this for initialization
    private void Awake()
    {
        CreateShip(samplePosition, samplePositions, sampleDirections, sampleComponents);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}