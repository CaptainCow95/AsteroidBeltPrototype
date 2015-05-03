using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsteroidBelt
{
    public class Inventory
    {
        public float Capacity;
        private List<InventorySlot> inventory;

        public Inventory(float capacity)
        {
            Capacity = capacity;
            inventory = new List<InventorySlot>();
        }

        public Inventory()
        {
            Capacity = 0;
            inventory = new List<InventorySlot>();
        }

        private float UsedCapacity
        {
            get
            {
                return inventory.Sum(e => e.Size);
            }
        }

        public void AddCapacity(float capacity)
        {
            Capacity += capacity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns>The number of items that successfully were added to the inventory.</returns>
        public int PutItem(InventoryItem item, int amount)
        {
            if (item.size * amount > Capacity - UsedCapacity)
            {
                amount = (int)((Capacity - UsedCapacity) / item.size);//add as many as can fit
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
                Debug.Log("Adding " + amount + " " + slots[0].Item.name);
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

        public string ToString()
        {
            string text = "";

            foreach (var slot in inventory)
            {
                text += slot.Item.name + ": " + slot.Amount + "\n";
            }

            return text;
        }
    }
}