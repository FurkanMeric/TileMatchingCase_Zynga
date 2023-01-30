using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Sprite sprite;
    public Vector2Int gridPosition;
    [SerializeField] private int point = 10;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        sprite = spriteRenderer.sprite;
    }
    
    
}
