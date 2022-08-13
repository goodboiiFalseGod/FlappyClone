using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnSpawner : MonoBehaviour
{
    [SerializeField] private Column _columnPrefab;
    [SerializeField] private ColumnPool _columnPoolPrefab;
    [SerializeField] private PickUpSpawner _pickUpSpawner;
    private ColumnPool _columnPoolInstance;
    private GameSettings _gameSettings;
    private int _columnCount = 7;

    private void Start()
    {
        StartCoroutine(SpawnerCoroutine());
    }

    public void Init(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;        
        _columnCount = CalculateColumnCount();
        _columnPoolInstance = Instantiate(_columnPoolPrefab);
    }

    private int CalculateColumnCount()
    {
        int result = 7;
        result = (int)Mathf.Ceil((CalculateStartPosition().x - Initilizer.GetCornerPosition(Corner.LeftUpper).x) / _gameSettings.ColumnInterval);
        return result;
    }

    private Vector2 CalculateStartPosition()
    {
        Vector2 result = new Vector2(Initilizer.GetCornerPosition(Corner.RightUpper).x + _gameSettings.ColumnInterval, 0);

        return result;
    }

    private Vector2 CalculateEndPosition()
    {
        Vector2 result = new Vector2(CalculateStartPosition().x - CalculateColumnCount() * _gameSettings.ColumnInterval, 0);

        return result;
    }

    private IEnumerator SpawnerCoroutine()
    {
        List<Column> columns = new List<Column>();
        Vector2 startPos = CalculateStartPosition();

        for (int i = 0; i < _columnCount; i++)
        {
            Column instance = Instantiate(_columnPrefab);
            instance.transform.position = startPos;
            instance.Init(_gameSettings);
            columns.Add(instance);
            yield return new WaitForSeconds(_gameSettings.ColumnInterval / _gameSettings.GameSpeed);
        }

        _columnPoolInstance.Init(_gameSettings, columns, startPos, CalculateEndPosition());
        _pickUpSpawner.Init(_columnPoolInstance);
    }
}
