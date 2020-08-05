using System.Collections.Generic;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{
    [SerializeField] private int _minMatchAmount;
    private Tile[,] _board;

    public void Initialize(Tile[,] board)
    {
        _board = board;
    }

    public List<Tile> CheckTile(Tile target)
    {
        List<Tile> totalMatches = new List<Tile>();
        List<Tile> horizontalMatches = CheckHorizontal(target);
        List<Tile> verticalMatches = CheckVertical(target);

        if (horizontalMatches.Count >= _minMatchAmount - 1)
        {
            totalMatches.AddRange(horizontalMatches);
        }
        if (verticalMatches.Count >= _minMatchAmount - 1)
        {
            totalMatches.AddRange(verticalMatches);
        }
        if (totalMatches.Count > 0)
        {
            totalMatches.Insert(0, target);
        }

        return totalMatches;
    }

    private List<Tile> CheckHorizontal(Tile target)
    {
        List<Tile> matches = new List<Tile>();
        int row = target.X;
        int column = target.Y;

        while (row - 1 >= 0 && target.Compare(_board[row - 1, column]))
        {
            row -= 1;
            matches.Add(_board[row, column]);            
        }

        row = target.X;

        while (row + 1 < _board.GetLength(0) && target.Compare(_board[row + 1, column]))
        {
            row += 1;
            matches.Add(_board[row, column]);
        }

        return matches;
    }

    private List<Tile> CheckVertical(Tile target)
    {
        List<Tile> matches = new List<Tile>();
        int row = target.X;
        int column = target.Y;

        while (column - 1 >= 0 && target.Compare(_board[row, column - 1]))
        {
            column -= 1;
            matches.Add(_board[row, column]);
        }

        column = target.Y;

        while (column + 1 < _board.GetLength(1) && target.Compare(_board[row, column + 1]))
        {
            column += 1;
            matches.Add(_board[row, column]);
        }

        return matches;
    }
}
