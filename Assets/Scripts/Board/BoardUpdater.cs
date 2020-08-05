using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUpdater : MonoBehaviour
{
    private Tile[,] _board;

    public void Initialize(Tile[,] board)
    {
        _board = board;
    }

    public IEnumerator Refresh()
    {
        for (int x = 0; x < _board.GetLength(0); x++)
        {
            for (int y = _board.GetLength(1) - 1; y >= 0; y--)
            {
                if (_board[x, y].IsEmpty)
                {
                    yield return StartCoroutine(CollapseColumn(x, y));
                }

                //Debug.Log("1");
                //if (_board[x, y].IsEmpty == true && _board[x, y].Dot != null)
                //{
                //    Debug.Log("2");
                //    if (_board[x, y - 1].Dot != null)
                //    {
                //        Debug.Log("3.1");
                //        _board[x, y].SetContent<Dot>(_board[x, y - 1].Dot);
                //        yield return StartCoroutine(_board[x, y].Translate());
                //    }
                //    else
                //    {
                //        Debug.Log("3.2");
                //    }
                //}
            }
        }
    }

    private IEnumerator CollapseColumn(int column, int yStart)
    {
        List<Tile> tiles = new List<Tile>();
        int nullCount = 0;

        for (int y = yStart; y >= 0; y--)
        {
            tiles.Add(_board[column, y]);
            if (_board[column, y].IsEmpty)
            {
                nullCount++;
            }
        }

        for (int i = 0; i < nullCount; i++)
        {
            for (int j = 0; j < tiles.Count - 1; j++)
            {
                if (tiles[j + 1].IsEmpty == false)
                {
                    tiles[j].SetContent<Dot>(tiles[j + 1].Dot);
                    yield return StartCoroutine(tiles[j].Translate());
                    tiles[j + 1].Clear();
                }
            }
        }
    }
}
