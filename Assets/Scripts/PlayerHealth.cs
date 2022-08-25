using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IAffectedByTime
{
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private GameObject _replayButton;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private TimeManager _timeManager;

    private int _hp;
    private int Health 
    { get => _hp; 
        set 
        { 
            if (!_isReversing && _healthRecords != null && !_isReplay && _healthNewRecords != null)
                _healthRecords.Add(_currentFrame, _hp);
            _hp = value;
            if(!_isReversing && _healthRecords != null && !_isReplay && _healthNewRecords != null)
                _healthNewRecords.Add(_currentFrame, _hp);
            HpChanged?.Invoke(_hp); 
        } 
    }
    private float _invincibilityRemains = 0;
    public event Action<int> HpChanged;

    private bool _isReversing = false;
    private bool _isReplay = false;

    private ulong _currentFrame = 0;
    private Dictionary<ulong, float> _invincibilityRecords;
    private Dictionary<ulong, int> _healthRecords;
    private Dictionary<ulong, int> _healthNewRecords;

    public bool IsInvincible { get => _invincibilityRemains > 0; }

    private void Start()
    {
        Health = _settings.HP;
        _playerMovement.ReachedLethalHeight += GetHit;
        _playerMovement.HittedByObstacle += GetHit;
        _timeManager.AddNewAffectedByTime(this);

        _invincibilityRecords = new Dictionary<ulong, float>();
        _healthRecords = new Dictionary<ulong, int>();
        _healthNewRecords = new Dictionary<ulong, int>();
    }

    private void FixedUpdate()
    {
        if(_isReplay)
        {
            if (_invincibilityRecords.ContainsKey(_currentFrame))
            {
                _invincibilityRemains = _invincibilityRecords[_currentFrame];
                SetTimedInvincibilyty(_invincibilityRemains);
            }
            else
            {
                SetInvincibility(false);
            }

            if (_healthRecords.ContainsKey(_currentFrame))
            {
                Health = _healthNewRecords[_currentFrame];
                _healthNewRecords.Remove(_currentFrame);
            }
            _currentFrame++;

            return;
        }

        if(_isReversing)
        {
            if(_invincibilityRecords.ContainsKey(_currentFrame))
            {
                _invincibilityRemains = _invincibilityRecords[_currentFrame];
                _invincibilityRecords.Remove(_currentFrame);
                SetTimedInvincibilyty(_invincibilityRemains);
            }
            else
            {
                SetInvincibility(false);
            }

            if(_healthRecords.ContainsKey(_currentFrame))
            {
                Health = _healthRecords[_currentFrame];
                _healthRecords.Remove(_currentFrame);
            }
            if(_healthNewRecords.ContainsKey(_currentFrame))
            {
                _healthNewRecords.Remove(_currentFrame);
            }
            _currentFrame--;
            if (_currentFrame < 0)
                SetReversing(false);

            return;
        }
        
        if (IsInvincible)
        {
            _invincibilityRemains -= (float)(1f / 50f);
            _invincibilityRecords.Add(_currentFrame, _invincibilityRemains);
        }
        else
        {
            SetInvincibility(false);
        }

        _currentFrame++;

    }

    private void GetHit()
    {
        if (IsInvincible) return;
        if (_isReversing) return;

        if (Health <= 0)
        {
            Time.timeScale = 0;
            _restartButton.gameObject.SetActive(true);
            _replayButton.gameObject.SetActive(true);
            return;
        }

        if (!_isReversing)
        {
            SetInvincibility(true);
            --Health;
        }
    }

    public void Heal(int _hp)
    {
        Health += _hp;
    }

    private void SetInvincibility(bool state)
    {
        _collider2D.enabled = !state;
        if (state)
        {
            _invincibilityRemains = _settings.invincibilityTime;
            _spriteRenderer.color = Color.red;
        }            
        else
        {
            _invincibilityRemains = 0;
            _spriteRenderer.color = Color.white;
        }            
    }

    private void SetTimedInvincibilyty(float time)
    {
        _collider2D.enabled = !IsInvincible;
        if (IsInvincible)
        {
            _spriteRenderer.color = Color.red;
        }
        else
        {
            _spriteRenderer.color = Color.white;
        }
    }

    public void SetReversing(bool state)
    {
        _isReversing = state;
    }

    public void StartReplay()
    {
        _isReplay = true;
        _currentFrame = 0;
    }
}
