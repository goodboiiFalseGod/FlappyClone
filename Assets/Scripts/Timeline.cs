using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour, IAffectedByTime
{
    [SerializeField] GameSettings _gameSettings;
    [SerializeField] TimeManager _timeManager;
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] float _startDistance = 20;
    [SerializeField] float _distanceStepMultiplier = 10;

    private float _currentDistance = 0;
    private float _currentMaxDistance;
    private int _keyFrame = 1;
    private int _currentFrame = 0;

    private bool _isReversing = false;

    private void Start()
    {
        _currentMaxDistance = _startDistance;
        _timeManager.AddNewAffectedByTime(this);
    }

    public void SetReversing(bool state)
    {
        _isReversing = state;
    }

    private void FixedUpdate()
    {
        _currentFrame++;

        if (!_isReversing)
        {
            _currentDistance += (1f / 50f) * _gameSettings.GameSpeed;
            if (_currentDistance >= _currentMaxDistance)
            {
                _currentMaxDistance *= _distanceStepMultiplier;
                _keyFrame *= (int)_distanceStepMultiplier / 2;
            }
        }
        else
        {
            _currentDistance -= (1f / 50f) * _gameSettings.GameSpeed;
        }

        if(_currentFrame == _keyFrame)
        {
            _scrollbar.value = _currentDistance / _currentMaxDistance;
            _currentFrame = 0;
        }   
    }

    public void StartReplay()
    {
        _currentDistance = 0f;
    }
}
