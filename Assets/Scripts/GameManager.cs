using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private Transform firstGemPlaceholder;
    [SerializeField] private Transform lastGemPlaceholder;
    [SerializeField] private Transform gemsParent;
    public Vector2Int gridSize;
    [SerializeField] private List<Sprite> gemSprites;

    public Dictionary<Vector2Int, Gem> generatedGems;
    public List<Gem> gems;
    
    public int point;

    public float timer = 0;
    private int roundTime = 60;

    public delegate void TimerChanged(int seconds);
    public static TimerChanged OnTimerChanged;
    public delegate void PointChanged(int point);
    public static PointChanged OnPointChanged;

    public void IncreasePoint(int increase)
    {
        point = point + increase;
        OnPointChanged(point);
    }

    public delegate void PositionFilled(Gem gem);

    public static PositionFilled OnPositionFilled;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
    }

    private void Start()
    {
        generatedGems = new Dictionary<Vector2Int, Gem>(gridSize.x * gridSize.y);
        gems = new List<Gem>();
        Spawn();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
            roundTime -= 1;
            OnTimerChanged(roundTime);
            if (roundTime <= 0)
            {
                #if !UNITY_EDITOR
                Application.Quit();
#else
                EditorApplication.isPlaying = false;
                #endif
            }

        }
    }

    private float startingXPos;
    private float startingYpos;
    private float incrementalX;
    private float incrementalY;

    private void Spawn()
    {
        Vector2 firstGemPosition = firstGemPlaceholder.position;
        Vector2 lastGemPosition = lastGemPlaceholder.position;
        
        startingXPos = firstGemPosition.x;
        startingYpos = firstGemPosition.y;

        incrementalX = (lastGemPosition - firstGemPosition).x / (gridSize.x - 1);
        incrementalY = (lastGemPosition - firstGemPosition).y / (gridSize.y - 1);

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Gem gem = GemPool.Instance.Instantiate(new Vector2Int(i + 1, j + 1));
                gem.gridPosition = new Vector2Int(i + 1, j + 1);
                generatedGems.Add(gem.gridPosition, gem);
                gems.Add(gem);

                 do
                    gem.ChangeGem(GetRandomGem());
                while (gem.HasMatch(gem.GetAdjacentGems()));
            }
        }
    }

    public Vector2 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector2(startingXPos + ((gridPosition.x - 1) * incrementalX), startingYpos+((gridPosition.y -1) * incrementalY));
    }

    public Vector2 GridToWorldPosition(int gridX, int gridY)
    {
        return GridToWorldPosition(new Vector2Int(gridX, gridY));
    }

    public void HandleVacantGridPositions(List<Vector2Int> positions)
    {
        DOTween.KillAll();
        
        Debug.Log("Handling Vacant Grid Positions");
        
        var descendingPositions = positions.OrderByDescending(a => a.y).ToList();

        foreach (var position in descendingPositions)
        {
            Vector2Int newPos = new Vector2Int(position.x, 0);
            while(generatedGems.ContainsKey(newPos))
            {
                newPos = new Vector2Int(position.x, newPos.y-1);
            }
            generatedGems.Add(newPos, GemPool.Instance.Instantiate(newPos,GetRandomGem())); 
        }

        var ascendingGems = gems.OrderByDescending(a => a.gridPosition.y).ToList();
        
        foreach (var gem in ascendingGems)
        {
            
            Vector2Int neededPosition = gem.gridPosition;
            if(neededPosition.y == gridSize.y) continue;
            
            while (!generatedGems.ContainsKey(new Vector2Int(neededPosition.x, neededPosition.y+1)))
            {
                neededPosition = new Vector2Int(neededPosition.x, neededPosition.y + 1);
                if(neededPosition.y == gridSize.y) break;
            }
            
            if(neededPosition == gem.gridPosition) continue;

            generatedGems.Remove(gem.gridPosition);
            gem.gridPosition = neededPosition;
            generatedGems.Add(neededPosition, gem);
            // gem.transform.position = GridToWorldPosition(neededPosition);
            // OnPositionFilled(gem);
            
            gem.tween = gem.transform.DOMove(GridToWorldPosition(neededPosition), 0.35f).SetEase(Ease.Linear).OnComplete(() => OnPositionFilled(gem));
        }
    }

    public Sprite GetRandomGem()
    {
        return gemSprites[UnityEngine.Random.Range(0, gemSprites.Count)];
    }

    public void Swap(Gem gemA, Gem gemB)
    {
        var posA = gemA.gridPosition;
        var posB = gemB.gridPosition;
        generatedGems[posA] = gemB;
        generatedGems[posB] = gemA;
        gemA.gridPosition = posB;
        gemB.gridPosition = posA;
    }

    
    
    
}
