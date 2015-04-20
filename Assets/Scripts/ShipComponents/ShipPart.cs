using UnityEngine;

namespace AsteroidBelt.ShipComponents
{
    public struct ShipPart
    {
        public ShipComponent.Direction Direction;
        public Vector2 Location;
        public ShipComponentType ShipComponent;

        public ShipPart(ShipComponentType shipComponentType, Vector2 location, ShipComponent.Direction direction)
        {
            ShipComponent = shipComponentType;
            Location = location;
            Direction = direction;
        }
    }
}