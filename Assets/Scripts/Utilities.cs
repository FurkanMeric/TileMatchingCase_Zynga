using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Utilities
{
    [CanBeNull]
    public static Gem Left(this Gem gem)
    {
        if (!GameManager.Instance.generatedGems.ContainsKey(new Vector2Int(gem.gridPosition.x - 1, gem.gridPosition.y)))
            return null;
        return gem.gridPosition.x > 1 ? GameManager.Instance.generatedGems[new Vector2Int(gem.gridPosition.x - 1, gem.gridPosition.y)] : null;
    }
    [CanBeNull]
    public static Gem Right(this Gem gem)
    {
        if (!GameManager.Instance.generatedGems.ContainsKey(new Vector2Int(gem.gridPosition.x + 1, gem.gridPosition.y)))
            return null;
        return GameManager.Instance.gridSize.x < gem.gridPosition.x ? GameManager.Instance.generatedGems[new Vector2Int(gem.gridPosition.x + 1, gem.gridPosition.y)] : null;
    }
    [CanBeNull]
    public static Gem Up(this Gem gem)
    {
        if (!GameManager.Instance.generatedGems.ContainsKey(new Vector2Int(gem.gridPosition.x, gem.gridPosition.y - 1)))
            return null;
        return gem.gridPosition.y>1 ? GameManager.Instance.generatedGems[new Vector2Int(gem.gridPosition.x, gem.gridPosition.y - 1)] : null;
    }
    [CanBeNull]
    public static Gem Down(this Gem gem)
    {
        if (!GameManager.Instance.generatedGems.ContainsKey(new Vector2Int(gem.gridPosition.x, gem.gridPosition.y + 1)))
            return null;
        return GameManager.Instance.gridSize.y < gem.gridPosition.y ? GameManager.Instance.generatedGems[new Vector2Int(gem.gridPosition.x, gem.gridPosition.y + 1)] : null;
    }

    public static List<Gem> GetAdjacentGems(this Gem gem)
    {
        return new List<Gem>() { gem.Left(), gem.Right(), gem.Up(), gem.Down() };
    }


    public static bool DoesMatch(this Gem gem, Gem otherGem)
    {
        if (otherGem == null) return false;
        return gem.sprite == otherGem.sprite;
    }

    public static bool HasMatch(this Gem gem, List<Gem> gems)
    {
        for (int i = 0; i < gems.Count; i++)
        {
            if (gem.DoesMatch(gems[i])) return true;
        }

        return false;
    }
}
