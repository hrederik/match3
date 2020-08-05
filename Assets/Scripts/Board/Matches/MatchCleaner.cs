using System.Collections.Generic;
using UnityEngine;

public class MatchCleaner : MonoBehaviour
{
    [SerializeField] private MatchChecker _checker;

    public void Clean(Tile target)
    {
        List<Tile> matches = _checker.CheckTile(target);

        foreach (Tile tile in matches)
        {
            tile.Remove();
        }
    }
}
