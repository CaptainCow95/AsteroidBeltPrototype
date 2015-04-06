using UnityEngine;

public class LoadLevel1 : MonoBehaviour
{
    public int[] sampleComponents;
    public ShipComponent.Direction[] sampleDirections;
    public Vector2 samplePosition;
    public Vector2[] samplePositions;

    // Use this for initialization
    private void Awake()
    {
        GameManager.Instance.CreateShip(samplePosition, samplePositions, sampleDirections, sampleComponents);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}