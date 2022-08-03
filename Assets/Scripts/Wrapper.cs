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

    public static int DumplingsMade;

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

    private void FoldDown()
    {
        if (!_dropTarget.dropped) return;
        Destroy(_dropTarget.dropped);
        _dropTarget.enabled = false;
        _animator.Play("Fold Down");
        _folded = true;
        GetComponent<Collider2D>().isTrigger = false;
        _sprite.sortingOrder = 2;
    }
    
    private void TiltUp()
    {
        if (!_folded) return;
        _animator.Play("Tilt Up");
        _tilted = true;
        Services.DialogueStarter.StartTutorialDialogue(3);
    }
    
    private void FoldRight()
    {
        if (!_tilted || _rightFolded) return;
        _rightFolded = true;
        _animator.Play(_leftFolded ? "Close Right" : "Fold Right");
        if (_leftFolded) _dragAndDrop.enabled = true;
    }
    
    private void FoldLeft()
    {
        if (!_tilted || _leftFolded) return;
        _animator.Play(_rightFolded ? "Close Left" : "Fold Left");
        _leftFolded = true;
        if (_rightFolded) Complete();
    }

    private void Complete()
    {
        Services.DialogueStarter.StartTutorialDialogue(4);
        _dragAndDrop.enabled = true;
        Services.DialogueStorage.SetValue("$dumplings_made", ++DumplingsMade);
    }
}
