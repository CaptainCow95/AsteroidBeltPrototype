using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsteroidBelt
{
    internal class Inventory
    {
        public float _capacity;
        private List<InventorySlot> inventory;

        public Inventory(float capacity)
        {
            _capacity = capacity;
        }

        public float Capacity
        {
            get { return _capacity; }
        }

        private float UsedCapacity
        {
            get
            {
                return inventory.Sum(e => e.Size);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns>The number of items that successfully were added to the inventory.</returns>
        public int PutItem(InventoryItem item, int amount)
        {
            if (item.size * amount > _capacity - UsedCapacity)
            {
                amount = (int)((_capacity - UsedCapacity) / item.size);//add as many as can fit
                if (amount == 0)
                {
                    return 0;
                }
            }

            //first check if we have enough capacity;
            var slots = inventory.Where(e => e.Item.type == item.type).ToList();
            if (slots.Count > 0)
            {
                slots[0].add(amount);
                if (slots.Count > 1)
                {
                    Debug.LogError("The inventory had more than one instance of the added item " + item.name);
                }
            }
            else if (slots.Count == 0)
            {
                inventory.Add(new InventorySlot(item, amount));
            }

            return amount;
        }
    }
}