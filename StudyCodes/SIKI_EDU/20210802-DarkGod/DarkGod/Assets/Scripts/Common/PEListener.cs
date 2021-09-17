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
            if (eventData!=null)
            {
                onClickDown?.Invoke(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData!=null)
            {
                onClickUp?.Invoke(eventData);
            }
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData!=null)
            {
                onClickDrag?.Invoke(eventData);
            }
        }
    }
}