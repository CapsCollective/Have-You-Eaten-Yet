using System;
using NaughtyAttributes;
using UnityEngine;

public enum DraggableType
{
    MeatBall,
    Dumpling,
    FaultyWrapper
}
public class DragAndDrop : MonoBehaviour
{
    public static Action<DraggableType> OnPlaced;

    [SerializeField] private Transform target;
    public DraggableType type;
    
    [ReadOnly] public Transform snapTarget;
    [ReadOnly] public bool isDragging;
    
    private Collider2D _collider;
    private Vector2 _startScale;

    private Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void Awake()
    {
        _startScale = transform.localScale;
        _collider = GetComponent<Collider2D>();
    }

    public void Select()
    {
        isDragging = true;
        transform.localScale = _startScale * 1.2f;
    }
    
    private void Update()
    {
        if (!enabled) return;

        if (Input.GetMouseButtonDown(0) && !snapTarget)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_collider.OverlapPoint(mousePosition))
            {
                isDragging = true;
                transform.localScale = _startScale * 1.2f;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            transform.localScale = _startScale;
        
            if (snapTarget)
            {
                transform.SetParent(snapTarget);
                target.SetParent(snapTarget);
                target.position = snapTarget.position;
                OnPlaced?.Invoke(type);
            }
        }
        
        if (isDragging)
        {
            target.position = MousePos;
        }
        else if (!snapTarget && Vector3.Distance(target.position, transform.position) > 0.5f)
        {
            target.position = Vector3.Lerp(target.position, transform.position, Time.deltaTime * 10);
        }
    }
}
