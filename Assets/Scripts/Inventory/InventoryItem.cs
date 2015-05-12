using UnityEngine;

namespace AsteroidBelt
{
    public class InventoryItem : MonoBehaviour
    {
        public string name;
        public float size;
        public ItemType type;
        public int value;

        public enum ItemType
        {
            IronOre = 0,
            CopperOre = 1,
            GoldOre = 2
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}