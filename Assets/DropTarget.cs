using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DropTarget : MonoBehaviour
{
    [SerializeField] private DragAndDrop.DraggableType acceptType;
    
    public GameObject Dropped { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled) return;
        
        DragAndDrop target = col.gameObject.GetComponent<DragAndDrop>();
        if(!target || target.type != acceptType) return;

        Dropped = target.gameObject;
        target.snapTarget = transform;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!enabled) return;

        DragAndDrop target = col.gameObject.GetComponent<DragAndDrop>();
        if (!target) return;
        target.snapTarget = null;
    }
}
