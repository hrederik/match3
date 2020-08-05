using UnityEngine;
using UnityEngine.UI;

public class IngamePanel : MonoBehaviour
{
    [SerializeField] private Text _scoresLabel;
    [SerializeField] private Scores _scores;

    private void OnEnable()
    {
        _scores.ScoresUpdated += OnScoresUpdate;
    }

    private void OnDisable()
    {
        _scores.ScoresUpdated -= OnScoresUpdate;
    }

    private void OnScoresUpdate(int value)
    {
        _scoresLabel.text = $"SCORES: {value}";
    }
}
