using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Common
{
    public class PEListener : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler,IPointerClickHandler
    {
        public Action<PointerEventData> onClickDown;
        public Action<PointerEventData> onClickUp;
        public Action<PointerEventData> onClickDrag;
        public Action<object> onClick;

        public object args;
        
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData!=null)
            {
                onClick?.Invoke(eventData);
            }
        }
    }
}