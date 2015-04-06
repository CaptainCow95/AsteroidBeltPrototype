using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public List<ShipComponent> shipComponents;
    private Rigidbody2D rigidBody;

    public void addShipComponent(ShipComponent shipComponent)
    {
        shipComponents.Add(shipComponent);
        Rigidbody2D rigidBody = gameObject.GetComponent<Rigidbody2D>();
        rigidBody.mass += shipComponent.mass;
    }

    // Use this for initialization
    private void Start()
    {
        rigidBody = gameObject.GetComponent("Rigidbody2D") as Rigidbody2D;
        rigidBody.gravityScale = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        Vector2 vec = Input.mousePosition - Camera.main.WorldToScreenPoint(gameObject.transform.position);
        var angle = Mathf.Atan2(-vec.x, vec.y) * Mathf.Rad2Deg;

        rigidBody.rotation = angle;

        foreach (var item in shipComponents.Where(e => e is Thruster))
        {
            Vector2 force = ((Thruster)item).GetThrust(new Vector2(horizontal, vertical));
            rigidBody.AddForce(transform.rotation * (Vector3)force);
        }
    }
}