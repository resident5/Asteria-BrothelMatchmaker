using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    public const string EXPRESSION = "\\d";

    protected void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();
        initalParent = transform.parent;
        //Debug.Log($"Sibling index is {transform.GetSiblingIndex()} on {transform.name}");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.pressPosition;
        transform.SetParent(initalParent.parent.parent);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDragPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"I dragged on top of... {eventData.pointerCurrentRaycast.gameObject.name}");

        transform.position = startPos;
        transform.SetParent(initalParent);
        Sort();
        image.raycastTarget = true;
    }

    public virtual void Action() { }

    public void Sort()
    {
        Regex regex = new Regex(EXPRESSION);
        Match match = regex.Match(transform.name);
        if(match.Success)
        {
            int.TryParse(match.Value, out int value);
            transform.SetSiblingIndex(value - 1);
        }
    }

    private void SetDragPosition(PointerEventData eventData)
    {
        Vector3 globalMousePosition;
        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePosition))
        {
            rectTransform.position = globalMousePosition;
        }
    }
}
