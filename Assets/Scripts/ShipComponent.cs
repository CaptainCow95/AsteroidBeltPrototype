using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    public Direction direction;
   // public Vector2 offset;
    public GameObject parentShip;

    public enum Direction
    {
        Up,
        Left,
        Down,
        Right
    }

    // Use this for initialization
    private void Start()
    {
        gameObject.transform.SetParent(parentShip.transform, false);
        //gameObject.transform.localPosition = offset;

        switch (direction)
        {
            case Direction.Up:
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;

            case Direction.Left:
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;

            case Direction.Down:
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
                break;

            case Direction.Right:
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}