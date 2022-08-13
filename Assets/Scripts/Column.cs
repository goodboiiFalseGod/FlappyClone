using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Column : MonoBehaviour
{
    [SerializeField] GameObject _upperColumn;
    [SerializeField] GameObject _lowerColumn;
    [SerializeField] GameObject _innerColumnPrefab;
    [SerializeField] GameObject _ColumnWindowPrefab;

    private List<GameObject> _columnParts;

    private GameSettings _gameSettings;
    [SerializeField] private int _currentWindowsCount = 1;

    private void Update()
    {
        transform.Translate(-_gameSettings.GameSpeed * Time.deltaTime, 0, 0);
    }

    public void Init(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        _columnParts = new List<GameObject>(_gameSettings.MaximumWindowsCount * 2);
        ConstructColumn();
        ReconstuructColumn();

        transform.position = RandomStartPos();
    }

    private float CalculateMaxHeight(int _currentWindowsCount)
    {
        return _gameSettings.InnerColumnHeight * (_currentWindowsCount - 1) + _gameSettings.WindowSize * _currentWindowsCount;
    }

    private void ConstructColumn()
    {
        GameObject instance = Instantiate(_ColumnWindowPrefab, this.transform);  
        _columnParts.Add(instance);
        instance.transform.localScale = new Vector3(1, _gameSettings.WindowSize, 0);

        for (int i = 0; i <= _gameSettings.MaximumWindowsCount; i++)
        {
            instance = Instantiate(_innerColumnPrefab, this.transform);
            instance.transform.localScale = new Vector3(1, _gameSettings.InnerColumnHeight, 0);
            _columnParts.Add(instance);

            instance = Instantiate(_ColumnWindowPrefab, this.transform);
            instance.transform.localScale = new Vector3(1, _gameSettings.WindowSize, 0);
            _columnParts.Add(instance);
        }
    }

    public void ReconstuructColumn()
    {
        _currentWindowsCount = Random.Range(_gameSettings.MinimumWindowsCount, _gameSettings.MaximumWindowsCount + 1);
        _upperColumn.transform.localPosition = new Vector3(0, CalculateMaxHeight(_currentWindowsCount) / 2 + _upperColumn.transform.localScale.y / 2, 0);
        _lowerColumn.transform.localPosition = new Vector3(0, -CalculateMaxHeight(_currentWindowsCount) / 2 - _lowerColumn.transform.localScale.y / 2, 0);

        Vector3[] positions = CalculatePartsPositions(_currentWindowsCount);

        foreach(var part in _columnParts)
        {
            part.SetActive(false);
        }

        for (int i = 0; i < positions.Length; i++)
        {
            _columnParts[i].SetActive(true);
            _columnParts[i].transform.localPosition = positions[i];
        }
    }

    private Vector3[] CalculatePartsPositions (int _currentWindowsCount)
    {
        Vector3[] positions = new Vector3[_currentWindowsCount * 2 - 1];
        Vector3 upperWindowPosition = new Vector3(0, (_upperColumn.transform.localPosition - (_upperColumn.transform.localScale / 2)).y - _gameSettings.WindowSize / 2);

        positions[0] = upperWindowPosition;

        if (_currentWindowsCount == 1)
            return positions;

        Vector3 InnerColumnPosition = new Vector3(0, (upperWindowPosition.y - (_gameSettings.WindowSize / 2)) - _gameSettings.InnerColumnHeight / 2);
        Vector3 nextWindowPosition = new Vector3(0, (InnerColumnPosition.y - (_gameSettings.InnerColumnHeight / 2)) - _gameSettings.WindowSize / 2);

        positions[1] = InnerColumnPosition;
        positions[2] = nextWindowPosition;

        if (_currentWindowsCount == 2)
            return positions;

        for (int i = 3; i < (_currentWindowsCount * 2 - 1);)
        {
            positions[i] = new Vector3(0, (positions[i - 1].y - (_gameSettings.WindowSize / 2)) - _gameSettings.InnerColumnHeight / 2);
            positions[i + 1] = new Vector3(0, (positions[i].y - (_gameSettings.InnerColumnHeight / 2)) - _gameSettings.WindowSize / 2);

            i += 2;
        }
        
        return positions;
    }

    private Vector3 RandomStartPos()
    {
        Vector3 pos = new Vector3(this.transform.position.x, UnityEngine.Random.Range(-1f, 1f) * _gameSettings.GameDifficulty);
        return pos;
    }
}
