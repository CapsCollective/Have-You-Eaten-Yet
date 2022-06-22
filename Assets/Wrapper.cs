using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(DropTarget))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(DragAndDrop))]
public class Wrapper : MonoBehaviour
{

    private Animator _animator;
    private DropTarget _dropTarget;
    private DragAndDrop _dragAndDrop;
    private bool _folded;
    private bool _tilted;
    private bool _leftFolded;
    private bool _rightFolded;

    [SerializeField] private int center;
    private Vector2 StartPos = new Vector2(-10, 55);

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _dropTarget = GetComponent<DropTarget>();
        _dragAndDrop = GetComponent<DragAndDrop>();
        _dragAndDrop.enabled = false;
    }

    [Button]
    private void Throw()
    {
        Transform t = transform;
        t.position = StartPos;
        t.DOMove(new Vector3(center + Random.Range(-3,3), Random.Range(-25, -15), 0), 1f);
        t.eulerAngles = new Vector3(0, 0, 180);
        t.DORotate(new Vector3(0, 0, 360), 1f);
    }

    [Button]
    private void FoldDown()
    {
        if (!_dropTarget.Dropped) return;
        Destroy(_dropTarget.Dropped);
        _dropTarget.enabled = false;
        _animator.Play("Fold Down");
        _folded = true;
        GetComponent<Collider2D>().isTrigger = false;
    }
    
    [Button]
    private void TiltUp()
    {
        if (!_folded) return;
        _animator.Play("Tilt Up");
        _tilted = true;
    }
    
    [Button]
    private void FoldRight()
    {
        if (!_tilted) return;
        _rightFolded = true;
        _animator.Play("Fold Right");
    }
    
    [Button]
    private void FoldLeft()
    {
        if (!_rightFolded) return;
        _animator.Play("Fold Left");
        _leftFolded = true;
        _dragAndDrop.enabled = true;
    }
}
