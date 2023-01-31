using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GemPool : MonoBehaviour
{

    public static GemPool Instance;
    
    [SerializeField] private List<Gem> gems = new List<Gem>();
    private List<Gem> readyGems;
    
    [SerializeField] private Vector2 disposalPoint;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
    }

    private void Start()
    {
        if ((GameManager.Instance.gridSize.x * GameManager.Instance.gridSize.y) > gems.Count)
        {
            Debug.LogError("You need to populate pool to match your grid needs.");
            return;
        } 
        readyGems = new List<Gem>(gems);
    }

    public Gem Instantiate(Vector2Int gridPosition, Sprite gemSprite = null)
    {
        var gemToUse = readyGems[0];
        readyGems.Remove(gemToUse);
        gemToUse.gridPosition = gridPosition;
        gemToUse.transform.position = GameManager.Instance.GridToWorldPosition(gridPosition);
        if(gemSprite != null) gemToUse.ChangeGem(gemSprite);
        return gemToUse;
    }
    
    public Gem Recycle(Gem gem, Vector2Int position, Sprite gemSprite = null)
    {
        Dispose(gem);
        return Instantiate(position, gemSprite);
    }

    public void Dispose(Gem gem)
    {
        gem.transform.position = disposalPoint;
        GameManager.Instance.generatedGems.Remove(gem.gridPosition);
        readyGems.Add(gem);
    }

}
