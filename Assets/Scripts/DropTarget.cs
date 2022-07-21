using System;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DropTarget : MonoBehaviour
{
    [SerializeField] private DraggableType acceptType;
    [ReadOnly] public GameObject dropped;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled || dropped) return;
        DragAndDrop target = col.gameObject.GetComponent<DragAndDrop>();
        if(!target || !target.isDragging || target.type != acceptType) return;
        dropped = target.gameObject;
        if (target.snapTarget) target.snapTarget.GetComponent<DropTarget>().dropped = null;
        target.snapTarget = transform;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!enabled) return;
        DragAndDrop target = col.gameObject.GetComponent<DragAndDrop>();
        if (!target || !target.isDragging || dropped != col.gameObject) return;
        dropped = null;
        target.snapTarget = null;
    }
}
