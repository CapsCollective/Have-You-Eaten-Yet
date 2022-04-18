using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    private bool _isDragging;
    private bool _isGrounded;

    private Vector2 _velocity;
    private Vector2 _prevMousePos;
    
    private Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
    void OnMouseDown()
    {
        _isDragging = true;
        _prevMousePos = MousePos;
        target.localScale = Vector3.one * 1.2f;
    }

    void OnMouseUp()
    {
        _isDragging = false;
        target.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDragging)
        {
            /*
            Vector3 diff = MousePos - _prevMousePos;
            _prevMousePos = MousePos;
            transform.position += diff;
            */
            transform.position = MousePos;
        }
    }
}
