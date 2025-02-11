using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 startPos;
    RectTransform rectTransform;
    Canvas canvas;

    Image image;

    [HideInInspector]
    public Transform initalParent;

    public DropSource source;

    private void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.pressPosition;
        initalParent = transform.parent;
        transform.SetParent(initalParent.parent);
        //Debug.Log($"Transform root = {transform.parent.name}");
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDragPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPos;
        transform.SetParent(initalParent);
        image.raycastTarget = true;
    }

    public virtual void Action() { }

    private void SetDragPosition(PointerEventData eventData)
    {
        Vector3 globalMousePosition;
        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePosition))
        {
            rectTransform.position = globalMousePosition;
        }
    }
}
