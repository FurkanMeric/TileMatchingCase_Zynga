using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Utilities
{
    [CanBeNull]
    public static Gem Left(this Gem gem)
    {
        return gem.GetGemWithOffset( -1,0);
    }
    [CanBeNull]
    public static Gem Right(this Gem gem)
    {
        return gem.GetGemWithOffset( +1,0);
    }
    [CanBeNull]
    public static Gem Up(this Gem gem)
    {
        return gem.GetGemWithOffset( 0,-1);
    }
    [CanBeNull]
    public static Gem Down(this Gem gem)
    {
        return gem.GetGemWithOffset( 0,+1);
    }

    [CanBeNull]
    public static Gem GetGemWithOffset(this Gem gem, int offsetX, int offsetY)
    {
        var offset = new Vector2Int(offsetX, offsetY);
        var targetGridPos = (gem.gridPosition + offset);
        if (!GameManager.Instance.generatedGems.ContainsKey(targetGridPos)) return null;
        return GameManager.Instance.generatedGems[targetGridPos];
    }

    public static List<Gem> GetAdjacentGems(this Gem gem)
    {
        return new List<Gem>() { gem.Left(), gem.Right(), gem.Up(), gem.Down() };
    }


    public static bool DoesMatch(this Gem gem, Gem otherGem)
    {
        try
        {
            if (otherGem != null && gem != null && gem.sprite == otherGem.sprite) 
                return true;
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
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
