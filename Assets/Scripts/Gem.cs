using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Sprite sprite;
    public Vector2Int gridPosition;
    [SerializeField] private int point = 10;

    private Vector2 previousPosition;
    private Vector2 targetPosition;

    public Tween tween;

    public bool IsMoving
    {
        get
        {
            if (tween == null) return false;
            return tween.IsPlaying();
        }
    }
    
    public void YieldPoint()
    {
        GameManager.Instance.IncreasePoint(point);
    }

    public void ChangeGem(Sprite gem)
    {
        spriteRenderer.sprite = gem;
        sprite = gem;
    }
    
}
