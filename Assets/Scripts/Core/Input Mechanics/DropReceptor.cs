using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropReceptor : MonoBehaviour, IDropHandler
{
    public DropSource validSource;
    
    public void OnDrop(PointerEventData eventData)
    {
        GameObject drop = eventData.pointerDrag;
        DragSource draggable = drop.GetComponent<DragSource>();

        if(draggable.source == validSource)
        {
            draggable.Action();
        }

        Debug.Log($"Dropped {draggable.name} on {gameObject.name}");
    }
}

public enum DropSource
{
    KEY,
    IDCARD
}

