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
    [SerializeField] private bool destructible;
    public DraggableType type;
    
    [ReadOnly] public Transform snapTarget;
    [ReadOnly] public bool isDragging;
    
    private Collider2D _collider;
    private Vector2 _startScale;

    private Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private const int XLimit = 80;
    private const int YLimit = 45;
    
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

        if (Input.GetMouseButtonDown(0) && !snapTarget && _collider.OverlapPoint(MousePos))
        {
            isDragging = true;
            transform.localScale = _startScale * 1.2f;
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
                if (type == DraggableType.MeatBall) Services.DialogueStarter.StartTutorialDialogue(2);
            }
            else
            {
                if (destructible && (target.position.x is < -XLimit or > XLimit || target.position.y is < -YLimit or > YLimit)) Destroy(gameObject);
            }
        }
        
        if (isDragging)
        {
            target.position = destructible ? 
                MousePos : 
                new Vector2 (Mathf.Clamp(MousePos.x, -XLimit, XLimit), Mathf.Clamp(MousePos.y, -YLimit, YLimit));
        }
        else if (!snapTarget && Vector3.Distance(target.position, transform.position) > 0.5f)
        {
            target.position = Vector3.Lerp(target.position, transform.position, Time.deltaTime * 10);
        }
    }
}
