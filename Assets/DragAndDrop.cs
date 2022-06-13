using System;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Transform target;

    [HideInInspector] public Transform snapTarget;
    
    private bool _isDragging;
    private bool _isGrounded;

    private Vector2 _startScale;

    public enum DraggableType
    {
        MeatBall,
        Dumpling
    }

    public DraggableType type;
    
    private Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void Awake()
    {
        _startScale = transform.localScale;
    }

    void OnMouseUp()
    {
        if (!enabled) return;
        
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
        if (!enabled) return;

        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            transform.localScale = _startScale * 1.2f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            transform.localScale = _startScale;
        
            if (snapTarget)
            {
                target.position = snapTarget.position;
            } 
        }
        
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
