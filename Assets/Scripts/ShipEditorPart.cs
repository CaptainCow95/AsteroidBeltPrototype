using AsteroidBelt.ShipComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AsteroidBelt
{
    public class ShipEditorPart : MonoBehaviour, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public ShipComponentType ComponentType;
        public ShipComponent.Direction Direction;
        public string HoverText;
        public GameObject Part;
        private bool popupEnabled = false;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
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
                ShipEditor.Instance.CurrentPart = Instantiate(Part);
                ShipEditor.Instance.CurrentPart.GetComponent<RectTransform>().SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>());
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UpdatePopupPosition();
            ShipEditor.Instance.PopupText.GetComponent<Text>().text = HoverText;
            ShipEditor.Instance.PopupTextPanel.SetActive(true);
            popupEnabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ShipEditor.Instance.PopupTextPanel.SetActive(false);
            popupEnabled = false;
        }

        public void Update()
        {
            if (popupEnabled)
            {
                UpdatePopupPosition();
            }
        }

        private void UpdatePopupPosition()
        {
            var popupTextPanelRectTransform = ShipEditor.Instance.PopupTextPanel.GetComponent<RectTransform>();
            popupTextPanelRectTransform.localPosition = new Vector3(popupTextPanelRectTransform.localPosition.x, GetComponent<RectTransform>().localPosition.y + ShipEditor.Instance.ShipComponentsPanel.GetComponent<RectTransform>().localPosition.y + 10, popupTextPanelRectTransform.localPosition.z);
        }
    }
}