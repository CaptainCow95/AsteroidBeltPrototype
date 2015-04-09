namespace AsteroidBelt.ShipComponents
{
    public class Cargo : ShipComponent
    {
        public float capacity;
        public float usedSpace;


        public float getRemainingSpace(){
            return capacity - usedSpace;
        }

        public void put(float size)//later should add an item to the cargo or increase the amount of an item already here
        {
            usedSpace += size;
        }
    }
}
