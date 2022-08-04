using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField] GameObject upperColumn;
    [SerializeField] GameObject lowerColumn;

    private GameSettings _gameSettings;
    private int _columnCount;

    public void Start()
    {
        transform.position = RandomPos();
    }

    public void Init(GameSettings gameSettings, int columnCount)
    {
        _gameSettings = gameSettings;
        _columnCount = columnCount;
        upperColumn.transform.Translate(Vector3.down * _gameSettings.WindowSize / 2);
        lowerColumn.transform.Translate(Vector3.up * _gameSettings.WindowSize / 2);
    }

    private void FixedUpdate()
    {
        transform.Translate(-_gameSettings.GameSpeed * Time.deltaTime, 0, 0);
        if (transform.position.x < -_gameSettings.ColumnInterval * _columnCount / 2 - 1)
        {
            transform.position = RandomPos();
        }
    }

    private Vector3 RandomPos()
    {
        Vector3 pos = new Vector3 (_gameSettings.ColumnInterval * _columnCount / 2, Random.Range(-1f, 1f) * _gameSettings.GameDifficulty);
        return pos;
    }
}
