using System;
using UnityEngine;

public class Scores : MonoBehaviour
{
    private int _value;
    public event Action<int> ScoresUpdated;

    public int Value => _value;
    public void Add(int amount)
    {
        _value += amount;
        ScoresUpdated?.Invoke(_value);
    }
}
