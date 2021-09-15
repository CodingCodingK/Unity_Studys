using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Common
{
    public class PEListener : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
    {
        public Action<PointerEventData> onClickDown;
        public Action<PointerEventData> onClickUp;
        public Action<PointerEventData> onClickDrag;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            onClickDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onClickUp?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            onClickDrag?.Invoke(eventData);
        }
    }
}