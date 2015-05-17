using AsteroidBelt.ShipComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsteroidBelt
{
    public class ShipEditorTile : MonoBehaviour, IBeginDragHandler, IDropHandler
    {
        public GameObject Part;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (Part == null)
            {
                return;
            }

            bool onObject = true;

            var rect = GetComponent<RectTransform>().rect;
            rect.x += transform.position.x;
            rect.y += transform.position.y;
            if (eventData.position.x < rect.x ||
                eventData.position.x > rect.xMax ||
                eventData.position.y < rect.y ||
                eventData.position.y > rect.yMax)
            {
                onObject = false;
            }

            if (onObject)
            {
                GameManager.Instance.totalCredits += Part.GetComponent<ShipEditorPart>().Part.GetComponent<ShipComponent>().value;
                ShipEditor.Instance.CurrentPart = Part;
                ShipEditor.Instance.CurrentPart.GetComponent<RectTransform>().SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>());
                Part = null;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (ShipEditor.Instance.CurrentPart == null)
            {
                return;
            }

            if (Part != null)
            {
                GameManager.Instance.totalCredits += Part.GetComponent<ShipEditorPart>().Part.GetComponent<ShipComponent>().value;
                Destroy(Part);
                Part = null;
            }

            int value = ShipEditor.Instance.CurrentPart.GetComponent<ShipEditorPart>().Part.GetComponent<ShipComponent>().value;
            if (value > GameManager.Instance.totalCredits)
            {
                return;
            }

            GameManager.Instance.totalCredits -= value;
            Part = ShipEditor.Instance.CurrentPart;
            Part.GetComponent<RectTransform>().SetParent(ShipEditor.Instance.EditorTileParent.GetComponent<RectTransform>());
            ShipEditor.Instance.CurrentPart = null;

            Part.GetComponent<RectTransform>().position = transform.position;
        }

        public void UpdateDirection()
        {
            switch (Part.GetComponent<ShipEditorPart>().Direction)
            {
                case ShipComponent.Direction.Up:
                    Part.GetComponent<RectTransform>().rotation = Quaternion.identity;
                    break;

                case ShipComponent.Direction.Right:
                    Part.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 270);
                    break;

                case ShipComponent.Direction.Down:
                    Part.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
                    break;

                case ShipComponent.Direction.Left:
                    Part.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }
        }

        private void Update()
        {
            if (Part == null)
            {
                return;
            }

            var rect = GetComponent<RectTransform>().rect;
            rect.x += transform.position.x;
            rect.y += transform.position.y;
            if (Input.GetKeyDown(KeyCode.R) &&
                Input.mousePosition.x > rect.x &&
                Input.mousePosition.x < rect.xMax &&
                Input.mousePosition.y > rect.y &&
                Input.mousePosition.y < rect.yMax)
            {
                Part.GetComponent<ShipEditorPart>().Direction++;
                if ((int)Part.GetComponent<ShipEditorPart>().Direction > 3)
                {
                    Part.GetComponent<ShipEditorPart>().Direction = 0;
                }

                UpdateDirection();
            }
        }
    }
}