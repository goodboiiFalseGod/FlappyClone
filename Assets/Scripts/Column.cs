using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Column : MonoBehaviour, IAffectedByTime
{
    [SerializeField] GameObject _upperColumn;
    [SerializeField] GameObject _lowerColumn;
    [SerializeField] GameObject _innerColumnPrefab;
    [SerializeField] GameObject _ColumnWindowPrefab;
    [SerializeField] private int _currentWindowsCount = 1;

    private List<GameObject> _columnParts;

    private GameSettings _gameSettings;
    private TimeManager _timeManager;

    private Vector2 _startPosition;

    private ulong _currentFrame = 0;
    private Dictionary<ulong, (Vector2, int)> _restartPreviousPositions;
    private Dictionary<ulong, (Vector2, int)> _restartNewPositions;

    private bool _isReversing = false;
    private bool _isReplay = false;

    private void Update()
    {
        if (_isReversing)            
        {
            transform.Translate(_gameSettings.GameSpeed * Time.deltaTime, 0, 0);
            return;
        }        
        transform.Translate(-_gameSettings.GameSpeed * Time.deltaTime, 0, 0);        
    }

    private void FixedUpdate()
    {
        if(_isReplay)
        {
            if(_restartNewPositions.ContainsKey(_currentFrame))
            {
                this.transform.position = _restartNewPositions[_currentFrame].Item1;
                this.ReconstuructColumn(_restartNewPositions[_currentFrame].Item2);
            }
            _currentFrame++;
            return;
        }

        if(_isReversing)
        {
            if (_restartPreviousPositions.ContainsKey(_currentFrame))
            {
                this.transform.position = _restartPreviousPositions[_currentFrame].Item1;
                this.ReconstuructColumn(_restartPreviousPositions[_currentFrame].Item2);
                _restartPreviousPositions.Remove(_currentFrame);
            }
            if (_restartNewPositions.ContainsKey(_currentFrame))
            {
                _restartNewPositions.Remove(_currentFrame);
            }
            _currentFrame--;
            if (_currentFrame < 0)
                SetReversing(false);
            return;
        }

        _currentFrame++;
    }

    public void Init(GameSettings gameSettings, TimeManager timeManager, Vector2 startPosition)
    {
        _gameSettings = gameSettings;
        _timeManager = timeManager;
        AddToTimeManager();
        _startPosition = startPosition;
        _columnParts = new List<GameObject>(_gameSettings.MaximumWindowsCount * 2);
        _restartPreviousPositions = new Dictionary<ulong, (Vector2, int)>();
        _restartNewPositions = new Dictionary<ulong, (Vector2, int)>();
        ConstructColumn();
        Restart();
        _restartNewPositions.Clear();
    }

    public void AddStartPosition(Vector2 startPosition)
    {
        _restartNewPositions.Add(_currentFrame, (startPosition, _currentWindowsCount));
    }

    private float CalculateCurrentHeight(int _currentWindowsCount)
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

    private void ReconstuructColumn()
    {
        _currentWindowsCount = Random.Range(_gameSettings.MinimumWindowsCount, _gameSettings.MaximumWindowsCount + 1);
        _upperColumn.transform.localPosition = new Vector3(0, CalculateCurrentHeight(_currentWindowsCount) / 2 + _upperColumn.transform.localScale.y / 2, 0);
        _lowerColumn.transform.localPosition = new Vector3(0, -CalculateCurrentHeight(_currentWindowsCount) / 2 - _lowerColumn.transform.localScale.y / 2, 0);

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

    private void ReconstuructColumn(int windowsCount)
    {
        _currentWindowsCount = windowsCount;
        _upperColumn.transform.localPosition = new Vector3(0, CalculateCurrentHeight(_currentWindowsCount) / 2 + _upperColumn.transform.localScale.y / 2, 0);
        _lowerColumn.transform.localPosition = new Vector3(0, -CalculateCurrentHeight(_currentWindowsCount) / 2 - _lowerColumn.transform.localScale.y / 2, 0);

        Vector3[] positions = CalculatePartsPositions(_currentWindowsCount);

        foreach (var part in _columnParts)
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

    private Vector3 SetRandomPos()
    {
        Vector3 pos = new Vector3(_startPosition.x, UnityEngine.Random.Range(-1f, 1f) * _gameSettings.GameDifficulty);
        this.transform.position = pos;
        return pos;
    }

    public bool Restart()
    {
        if(!_isReversing & !_isReplay)
        {
            _restartPreviousPositions.Add(_currentFrame, (this.transform.position, _currentWindowsCount));
            SetRandomPos();
            ReconstuructColumn();
            _restartNewPositions.Add(_currentFrame, (this.transform.position, _currentWindowsCount));
            return true;
        }

        return false;
    }

    public void AddToTimeManager()
    {
        _timeManager.AddNewAffectedByTime(this);
    }

    public void SetReversing(bool state)
    {
        _isReversing = state;
    }

    public void StartReplay()
    {
        _isReplay = true;
        _currentFrame = 0;
        transform.position = _restartNewPositions.First().Value.Item1;
        ReconstuructColumn(_restartNewPositions.First().Value.Item2);
        _restartNewPositions.Remove(_restartNewPositions.First().Key);
    }
}
