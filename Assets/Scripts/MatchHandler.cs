using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        InputManager.OnGemTapped += OnGemTapped;
        GameManager.OnPositionFilled += TryMatch;
    }



    void OnGemTapped(Gem gem)
    {
        if (tappedGem == null)
        {
            tappedGem = gem;
            return;
        }

        if (tappedGem != null && otherGem == null)
        {
            if (!tappedGem.GetAdjacentGems().Contains(gem))
            {
                tappedGem = gem;
                return;
            }
            otherGem = gem;
        }



        Swap(() =>
        {
            if(CheckMatch(tappedGem) + CheckMatch(otherGem) == 0)
                Swap();
            
            tappedGem = null;
            otherGem = null;
            
        });
        
        

    }

    private void Swap(Action action = null)
    {   
        tappedPos = tappedGem.transform.position;
        var tappedGridPos = tappedGem.gridPosition;
        otherPos = otherGem.transform.position;
        var otherGridPos = otherGem.gridPosition;
        
        GameManager.Instance.Swap(tappedGem, otherGem);

        Sequence seq = DOTween.Sequence();
        seq.Join(tappedGem.transform.DOMove(otherPos, 0.2f).SetEase(Ease.InOutQuart));
        seq.Join(otherGem.transform.DOMove(tappedPos, 0.2f).SetEase(Ease.InOutQuart));

        seq.Play();

        if(action != null)
            seq.OnComplete(action.Invoke);
    }

    private int CheckMatch(Gem gem)
    {
        Gem controlGem;
        List<Gem> matches = new List<Gem>();
        List<Gem> verticalMatches = new List<Gem>();
        List<Gem> horizontalMatches = new List<Gem>();

        controlGem = gem;
        
        while (controlGem.Left().DoesMatch(gem))
        {
            controlGem = controlGem.Left();
            horizontalMatches.Add(controlGem);
        }

        controlGem = gem;
        while (controlGem.Right().DoesMatch(gem))
        {
            controlGem = controlGem.Right();
            horizontalMatches.Add(controlGem);
        }

        controlGem = gem;
        while (controlGem.Up().DoesMatch(gem))
        {
            controlGem = controlGem.Up();
            verticalMatches.Add(controlGem);
        }
        
        controlGem = gem;
        while (controlGem.Down().DoesMatch(gem))
        {
            controlGem = controlGem.Down();
            verticalMatches.Add(controlGem);
        }

        if (horizontalMatches.Count >= 2)
        {
            if (!matches.Contains(gem))
            {
                matches.Add(gem);
            }
            for (int i = 0; i < horizontalMatches.Count; i++)
            {
                matches.Add(horizontalMatches[i]);
            }
        }

        if (verticalMatches.Count >= 2)
        {
            if (!matches.Contains(gem))
            {
                matches.Add(gem);
            }
            for (int i = 0; i < verticalMatches.Count; i++)
            {
                matches.Add(verticalMatches[i]);
            }
        }
        
        Debug.Log("Checking for: " + gem.gameObject.name + ". Found " + matches.Count + " matches.");
        
        if (matches.Count > 0)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                matches[i].YieldPoint();
                GemPool.Instance.Dispose(matches[i]);
            }

            List<Vector2Int> vacantPositions = new List<Vector2Int>(matches.Select(x => x.gridPosition));
            GameManager.Instance.HandleVacantGridPositions(vacantPositions);
            return matches.Count;
        }

        return matches.Count;

    }
    
    private void TryMatch(Gem gem)
    {
        CheckMatch(gem);
    }
    
    
    
    
    
}
