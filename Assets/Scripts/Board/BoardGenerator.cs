using UnityEngine;
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private MatchChecker _checker;
    [SerializeField] private BoardUpdater _updater;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private int _holesCount;
    [SerializeField] private Hole _holePrefab;
    [SerializeField] private Dot[] _dotPrefabs;

    private void Awake()
    {
        Tile[,] board = CreateBoard();
        _board.Initialize(board);
        _checker.Initialize(board);
        _updater.Initialize(board);
    }

    private Tile[,] CreateBoard()
    {
        Tile[,] tiles = FillTableByTails(_tilePrefab.Size);

        AddHoles(tiles);
        AddDots(tiles);

        return tiles;
    }

    private Tile[,] FillTableByTails(Vector2 tileSize)
    {
        Tile[,] tiles = new Tile[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile newTile = Instantiate(_tilePrefab, Vector2.zero, _tilePrefab.transform.rotation);
                newTile.transform.SetParent(transform);
                newTile.gameObject.name = $"( {x} ; {y} )";
                newTile.Initialize(new Vector2(x, y));

                Vector2 newPosition = new Vector2(tileSize.x * x, tileSize.y * y);
                newTile.transform.localPosition = newPosition;

                tiles[x, y] = newTile;
            }
        }

        return tiles;
    }

    private void AddHoles(Tile[,] tiles)
    {
        for (int i = 0; i < _holesCount; i++)
        {
            int x = Random.Range(0, _width);
            int y = Random.Range(0, _height);

            Hole newHole = Instantiate(_holePrefab, Vector2.zero, Quaternion.identity);
            tiles[x, y].SetContent<Hole>(newHole);
        }
    }

    private void AddDots(Tile[,] tiles)
    {
        Dot[] previousColumn = new Dot[_height];
        Dot previous = null;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (tiles[x, y].Content == null)
                {
                    Dot newDot = GetRandomDot(previousColumn[y], previous);
                    previousColumn[y] = newDot;
                    previous = newDot;

                    newDot = Instantiate(newDot, Vector2.zero, Quaternion.identity);
                    tiles[x, y].SetContent<Dot>(newDot);
                }
            }
        }
    }

    private Dot GetRandomDot(params Dot[] exceptions)
    {
        List<Dot> possibleCharacters = new List<Dot>();
        possibleCharacters.AddRange(_dotPrefabs);

        foreach (Dot exception in exceptions)
        {
            possibleCharacters.Remove(exception);
        }

        return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
    }
}
