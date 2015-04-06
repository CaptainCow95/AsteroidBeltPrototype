using Assets.Scripts;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject emptyShip;
    public GameObject[] possibleShipComponents;

    public void CreateShip(Vector2 position, Vector2[] componentPositions, ShipComponent.Direction[] componentDirections, int[] shipComponents)
    {
        GameObject newShip = GameObject.Instantiate(emptyShip, position, Quaternion.identity) as GameObject;
        Ship ship = newShip.GetComponent<Ship>();
        for (int i = 0; i < shipComponents.Length; ++i)
        {
            GameObject newShipComponent = GameObject.Instantiate(possibleShipComponents[shipComponents[i]], componentPositions[i], Quaternion.identity) as GameObject;
            ShipComponent comp = newShipComponent.GetComponent<ShipComponent>();
            comp.parentShip = newShip;
            comp.direction = componentDirections[i];
            ship.addShipComponent(comp);
        }
    }
}