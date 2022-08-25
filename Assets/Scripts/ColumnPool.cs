using System;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour
{
    private List<Column> _columns;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private GameSettings _gameSettings;

    private bool _active = false;

    public event Action<Vector2> ColumnRestarted;

    public void Init(GameSettings gameSettings, IEnumerable<Column> columns, Vector2 startPosition, Vector2 endPosition)
    {
        _columns = new List<Column>(columns);
        _gameSettings = gameSettings;
        _startPosition = startPosition;
        _endPosition = endPosition;
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
                    if (column.Restart())
                    {
                        ColumnRestarted?.Invoke(column.transform.position);
                    }                   
                    
                }
            }
        }
    }
}
