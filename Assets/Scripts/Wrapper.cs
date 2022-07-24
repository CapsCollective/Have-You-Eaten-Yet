using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(DropTarget))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(DragAndDrop))]
public class Wrapper : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sprite;
    private DropTarget _dropTarget;
    private DragAndDrop _dragAndDrop;
    private Collider2D _collider;
    [SerializeField] private Collider2D leftFoldArea, rightFoldArea;
    [SerializeField] private bool isFaulty;

    private bool _folded;
    private bool _tilted;
    private bool _leftFolded;
    private bool _rightFolded;
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _dropTarget = GetComponent<DropTarget>();
        _dragAndDrop = GetComponent<DragAndDrop>();
        _collider = GetComponent<Collider2D>();
        _dragAndDrop.enabled = false;
    }
    
    private Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    [ReadOnly] public bool isSelected;
    [ReadOnly] public float startY;

    private void Update()
    {
        if (isFaulty) return;
        Vector2 mousePosition = MousePos;
        if (Input.GetMouseButtonDown(0))
        {
            if (_collider.OverlapPoint(mousePosition))
            {
                isSelected = true;
                startY = mousePosition.y;
            }
            if (rightFoldArea.OverlapPoint(mousePosition) && !_rightFolded) FoldRight();
            if (leftFoldArea.OverlapPoint(mousePosition) && !_leftFolded) FoldLeft();
        }

        if (Input.GetMouseButtonUp(0)) isSelected = false;

        if (!isSelected) return;
        if (!_folded && mousePosition.y < startY - 6) FoldDown();
        if (_folded && !_tilted && mousePosition.y > startY + 6) TiltUp();
    }
    
    public void Throw(Vector3 targetPos)
    {
        Transform t = transform;
        t.DOMove(targetPos, 1f);
        t.eulerAngles = new Vector3(0, 0, 180);
        t.DORotate(new Vector3(0, 0, 360), 1f);
    }

    private void FoldDown()
    {
        if (!_dropTarget.dropped) return;
        Destroy(_dropTarget.dropped);
        _dropTarget.enabled = false;
        _animator.Play("Fold Down");
        _folded = true;
        GetComponent<Collider2D>().isTrigger = false;
    }
    
    private void TiltUp()
    {
        if (!_folded) return;
        _animator.Play("Tilt Up");
        _tilted = true;
    }
    
    private void FoldRight()
    {
        if (!_tilted) return;
        _rightFolded = true;
        _animator.Play("Fold Right");
    }
    
    private void FoldLeft()
    {
        if (!_rightFolded) return;
        _animator.Play("Fold Left");
        _sprite.sortingOrder = 2;
        _leftFolded = true;
        _dragAndDrop.enabled = true;
    }
}
