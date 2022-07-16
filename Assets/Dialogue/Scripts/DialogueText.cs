using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro textMeshPro;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _speed = 10;

    private bool _startFloat = false;

    private void Awake()
    {
        RestaurantDialogueView.OnNewRestaurantDialogue += OnNewRestaurantDialogue;
    }

    private void OnNewRestaurantDialogue()
    {
        if(!_startFloat)
            _startFloat = true;
    }

    public void SetSpriteFlip(bool x, bool y)
    {
        _spriteRenderer.flipX = x;
        _spriteRenderer.flipY = y;
    }

    public void SetText(string text)
    {
        textMeshPro.text = text;
    }

    public void Update()
    {
        if(_startFloat)
            transform.Translate(Vector2.up * Time.deltaTime * _speed);
    }
}
