using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    [SerializeField] private BoardUpdater _updater;
    [SerializeField] private MatchCleaner _cleaner;
    [SerializeField] private Scores _scores;
    private Tile _previousTile;
    private Tile[,] _tiles;    

    public int Width => _tiles.GetLength(0);
    public int Height => _tiles.GetLength(1);

    private void Start()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _tiles[x, y].TileSelected += OnTileSelected;
            }
        }
    }

    private void OnDisable()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _tiles[x, y].TileSelected -= OnTileSelected;
            }
        }
    }

    public void Initialize(Tile[,] tiles)
    {
        _tiles = tiles;
    }

    public void OnTileSelected(Tile selected)
    {
        List<Tile> adjacentTiles = GetAdjacentTiles(selected);

        if (_previousTile == null)
        {
            _previousTile = selected;
        }
        else
        {
            _previousTile.Deselect();

            if (adjacentTiles.Contains(_previousTile))
            {
                _scores.Add(10);

                SwapDots(selected, _previousTile);

                StartCoroutine(ShiftTiles(_previousTile, selected));

                selected.Deselect();
                _previousTile = null;
            }
            else
            {
                _previousTile = selected;
            }
        }
    }

    private IEnumerator ShiftTiles(Tile previous, Tile target)
    {
        StartCoroutine(target.Translate());
        yield return StartCoroutine(previous.Translate());

        _cleaner.Clean(previous);
        _cleaner.Clean(target);

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(_updater.Refresh());
    }

    private void SwapDots(Tile current, Tile target)
    {
        Dot temp = current.Dot;
        current.SetContent<Dot>(target.Dot);
        target.SetContent<Dot>(temp);
    }

    private List<Tile> GetAdjacentTiles(Tile targetTile)
    {
        List<Tile> adjacentTiles = new List<Tile>();

        if (targetTile.X > 0)
        {
            adjacentTiles.Add(_tiles[targetTile.X - 1, targetTile.Y]);
        }

        if (targetTile.X < Width - 1)
        {
            adjacentTiles.Add(_tiles[targetTile.X + 1, targetTile.Y]);
        }

        if (targetTile.Y > 0)
        {
            adjacentTiles.Add(_tiles[targetTile.X, targetTile.Y - 1]);
        }

        if (targetTile.Y < Height - 1)
        {
            adjacentTiles.Add(_tiles[targetTile.X, targetTile.Y + 1]);
        }

        return adjacentTiles;
    }
}
