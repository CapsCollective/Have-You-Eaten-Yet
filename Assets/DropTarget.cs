using UnityEngine;

public class DropTarget : MonoBehaviour
{
    [SerializeField] private DragAndDrop.DraggableType acceptType;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        DragAndDrop target = col.gameObject.GetComponent<DragAndDrop>();
        if(!target || target.type != acceptType) return;

        target.snapTarget = transform;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        DragAndDrop target = col.gameObject.GetComponent<DragAndDrop>();
        Debug.Log(target.snapTarget);
        if (!target) return;
        target.snapTarget = null;
    }
}
