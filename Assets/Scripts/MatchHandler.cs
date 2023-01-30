using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    private Gem tappedGem;
    private Gem otherGem;
    private Vector2 tappedPos;
    private Vector2 otherPos;

    private void Awake()
    {
        InputManager.OnGemTapped = OnGemTapped;
    }

    void OnGemTapped(Gem gem)
    {
        if (tappedGem == null) tappedGem = gem;
        else if (tappedGem != null && otherGem == null) otherGem = gem;
        else
        {
            tappedGem = null;
            otherGem = null;
            OnGemTapped(gem);
        }
        
        CheckMatch();
    }

    private void CheckMatch()
    {
        
        if (tappedGem == null || otherGem == null) return;
        tappedPos = tappedGem.transform.position;
        otherPos = otherGem.transform.position;
        tappedGem.transform.DOMove(otherPos, 0.2f).SetEase(Ease.InOutQuart);
        otherGem.transform.DOMove(tappedPos, 0.2f).SetEase(Ease.InOutQuart);

    }
    
}
