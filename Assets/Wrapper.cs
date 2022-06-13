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


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _dropTarget = GetComponent<DropTarget>();
        _dragAndDrop = GetComponent<DragAndDrop>();
        _dragAndDrop.enabled = false;

    }

    [Button]
    private void FoldDown()
    {
        if (!_dropTarget.Dropped) return;
        Destroy(_dropTarget.Dropped);
        _dropTarget.enabled = false;
        _animator.Play("Fold Down");
        _folded = true;
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
