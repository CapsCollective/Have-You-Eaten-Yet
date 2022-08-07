using System;
using UnityEngine;

public class NightSpriteToggle : MonoBehaviour
{
    public static Action<bool> OnSpriteToggle;

    [SerializeField] private Sprite lightsOnSprite, lightsOffSprite;
    private SpriteRenderer _renderer;
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        OnSpriteToggle += SpriteToggle;
    }

    private void SpriteToggle(bool toggle)
    {
        _renderer.sprite = toggle ? lightsOnSprite : lightsOffSprite;
    }
}
