using System;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Transform target;

    public Transform snapTarget;
    
    private bool _isDragging;
    private bool _isGrounded;

    private Vector2 _startScale;

    public enum DraggableType
    {
        MeatBall
    }

    public DraggableType type;
    
    private Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void Awake()
    {
        _startScale = transform.localScale;
    }

    public void OnMouseDown()
    {
        _isDragging = true;
        transform.localScale = _startScale * 1.2f;
    }

    void OnMouseUp()
    {
        _isDragging = false;
        transform.localScale = _startScale;

        if (snapTarget)
        {
            target.position = snapTarget.position;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDragging)
        {
            target.position = MousePos;
        }
        else if (!snapTarget && Vector3.Distance(target.position, transform.position) > 0.5f)
        {
            target.position = Vector3.Lerp(target.position, transform.position, Time.deltaTime * 10);
        }
    }
}
