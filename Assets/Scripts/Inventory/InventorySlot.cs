namespace AsteroidBelt
{
    internal class InventorySlot
    {
        private int _amount;
        private InventoryItem _item;

        public InventorySlot(InventoryItem item, int amount)
        {
            _item = item;
            _amount = amount;
        }

        public int Amount
        {
            get { return _amount; }
        }

        public InventoryItem Item
        {
            get { return _item; }
        }

        public float Size
        {
            get { return _amount * _item.size; }
        }

        public void add(int amountToAdd)
        {
            _amount += amountToAdd;
        }
    }
}