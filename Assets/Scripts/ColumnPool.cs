using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColumnPool : MonoBehaviour
{
    private List<Column> _columns;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private GameSettings _gameSettings;
    private Text _scoreText;

    private int _score = 0;

    private bool _active = false;

    public void Init(GameSettings gameSettings, IEnumerable<Column> columns, Vector2 startPosition, Vector2 endPosition, Text scoreText)
    {
        _columns = new List<Column>(columns);
        _gameSettings = gameSettings;
        _startPosition = startPosition;
        _endPosition = endPosition;
        _scoreText = scoreText;
        _active = true;
    }

    public void Update()
    {   
        if(_active)
        {
            foreach (var column in _columns)
            {
                if (column.transform.position.x <= _endPosition.x)
                {
                    column.transform.position = RandomPos();
                    AddScore();
                }
            }
        }
    }

    private void AddScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    private Vector3 RandomPos()
    {
        Vector3 pos = new Vector3(_startPosition.x, Random.Range(-1f, 1f) * _gameSettings.GameDifficulty);
        return pos;
    }
}
