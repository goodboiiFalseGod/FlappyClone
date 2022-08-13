using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextChanger : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private PlayerMovement _playerMovement;
    private int _score = 0;

    private void Start()
    {
        _scoreText.text = _score.ToString();
        _playerMovement.ScoreZoneEntered += AddScore;
    }

    private void AddScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }
}
