using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldToClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onLongClickDown;
    public UnityEvent onLongClickUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        onLongClickDown.Invoke();
        //Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onLongClickUp.Invoke();
        //Debug.Log("OnPointerUp");
    }

}