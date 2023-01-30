using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private Transform firstGemPlaceholder;
    [SerializeField] private Transform lastGemPlaceholder;
    [SerializeField] private Transform gemsParent;
    public Vector2Int gridSize;
    [SerializeField] private List<Gem> gems;

    public Dictionary<Vector2Int, Gem> generatedGems;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;
        
        generatedGems = new Dictionary<Vector2Int, Gem>(gridSize.x * gridSize.y);
        Spawn();
        

    }

    private void Spawn()
    {
        Vector2 firstGemPosition = firstGemPlaceholder.position;
        Vector2 lastGemPosition = lastGemPlaceholder.position;
        
        float startingXPos = firstGemPosition.x;
        float startingYPos = firstGemPosition.y;

        float incrementalX = (lastGemPosition - firstGemPosition).x / (gridSize.x - 1);
        float incrementalY = (lastGemPosition - firstGemPosition).y / (gridSize.y - 1);

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                
                Gem randomGem;
                do
                {
                    randomGem = gems[UnityEngine.Random.Range(0, gems.Count)];
                    randomGem.Initialize();
                    randomGem.gridPosition = new Vector2Int(i + 1, j + 1);

                } while (randomGem.HasMatch(randomGem.GetAdjacentGems()));
                
                Gem generatedGem = Instantiate(randomGem,
                    new Vector2(firstGemPosition.x + (i * incrementalX), firstGemPosition.y + (j * incrementalY)),
                    Quaternion.identity, gemsParent);
                generatedGem.Initialize();
                generatedGem.gridPosition = new Vector2Int(i + 1, j + 1);
                generatedGems.Add(generatedGem.gridPosition, generatedGem);
            }
        }
    }

    
    
    
}
