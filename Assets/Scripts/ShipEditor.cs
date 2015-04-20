using AsteroidBelt.Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidBelt
{
    public class ShipEditor : Singleton<ShipEditor>
    {
        public GameObject CurrentPart;
        public bool DropValid;
        public GameObject EditorTilePrefab;
        public int TilesPerSide;

        public List<ShipComponents.ShipPart> getShip()
        {
            List<ShipComponents.ShipPart> shipPartList = new List<ShipComponents.ShipPart>();
            GameObject[] EditorTiles = GameObject.FindGameObjectsWithTag("EditorTile");
            foreach (GameObject EditorTile in EditorTiles)
            {
                ShipEditorTile tileComponent = EditorTile.GetComponent<ShipEditorTile>();
                if (tileComponent.Part != null)
                {
                    ShipEditorPart shipPartComponent = tileComponent.Part.GetComponent<ShipEditorPart>();

                    Vector2 position = EditorTile.GetComponent<RectTransform>().anchoredPosition;
                    float recWidth = EditorTile.GetComponent<RectTransform>().rect.width;
                    position.x = position.x / recWidth;
                    position.y = position.y / recWidth;

                    ShipComponents.ShipComponentType shipComponentType = shipPartComponent.ComponentType;
                    ShipComponents.ShipComponent.Direction direction = tileComponent.Direction;

                    shipPartList.Add(new ShipComponents.ShipPart(shipComponentType, position, direction));
                }
            }

            return shipPartList;
        }

        public void LevelButton()
        {
            GameManager.Instance.SetShipToLoad(getShip());
            Application.LoadLevel(1);
        }

        private void Start()
        {
            //instantiate all the editor tiles
            for (int x = -TilesPerSide / 2; x < TilesPerSide / 2; ++x)
            {
                for (int y = -TilesPerSide / 2; y < TilesPerSide / 2; ++y)
                {
                    GameObject newEditorTile = Instantiate(EditorTilePrefab) as GameObject;
                    GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
                    newEditorTile.transform.SetParent(canvas.transform);
                    Rect editorRect = newEditorTile.GetComponent<RectTransform>().rect;
                    newEditorTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * editorRect.width, y * editorRect.height);
                    newEditorTile.tag = "EditorTile";
                }
            }
        }

        private void Update()
        {
            if (CurrentPart != null)
            {
                CurrentPart.GetComponent<RectTransform>().position = Input.mousePosition;

                if (!Input.GetMouseButton(0))
                {
                    if (!DropValid)
                    {
                        Destroy(CurrentPart);
                    }

                    CurrentPart = null;

                    DropValid = false;
                }
            }
        }
    }
}