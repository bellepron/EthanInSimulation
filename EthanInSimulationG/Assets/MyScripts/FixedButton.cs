using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public bool pressed;
    public bool enter = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //clicked = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        enter = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        enter = false;
    }
}