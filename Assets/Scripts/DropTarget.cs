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
        Debug.Log($"{name} is Adding Target");
        
        if (target.snapTarget)
        {
            Debug.Log($"{target.snapTarget.name} is Removing Target in override");
            target.snapTarget.GetComponent<DropTarget>().dropped = null;
        }
        target.snapTarget = transform;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!enabled) return;
        DragAndDrop target = col.gameObject.GetComponent<DragAndDrop>();
        if (
            !target || 
            !target.isDragging || 
            dropped != col.gameObject
        ) return;
        
        Debug.Log($"{name} is Removing Target");
        dropped = null;
        target.snapTarget = null;
    }
}
