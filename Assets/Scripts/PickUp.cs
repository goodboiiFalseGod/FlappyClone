using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour, IAffectedByTime
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Collider2D _collider;

    protected GameSettings _gameSettings;
    protected PlayerHealth _playerHealth;
    protected PlayerMovement _playerMovement;
    protected TimeManager _timeManager;

    private bool _isReversing = false;
    private bool _isReplay = true;
    protected Color _startColor;
    protected bool _isActive = true;

    private ulong _currentFrame = 0;
    private ulong _spawnFrame;
    private ulong _deactivateFrame = 0; 

    private Vector2 _startPosition;

    public void Init(GameSettings gameSettings, PlayerHealth playerHealth, PlayerMovement playerMovement, TimeManager timeManager, ulong currentFrame)
    {
        _gameSettings = gameSettings;
        _playerHealth = playerHealth;
        _playerMovement = playerMovement;
        _timeManager = timeManager;
        _startColor = _spriteRenderer.color;
        _startPosition = this.transform.position;
        _currentFrame = currentFrame;
        _spawnFrame = currentFrame;

        _timeManager.AddNewAffectedByTime(this);
        this.transform.position = RandomStartPos();
    }

    protected void Update()
    {
        if(_isReversing)
        {
            this.transform.Translate(_gameSettings.GameSpeed * Time.deltaTime, 0, 0);
            return;
        }
        this.transform.Translate(-_gameSettings.GameSpeed * Time.deltaTime, 0, 0);
    }

    private void FixedUpdate()
    {
        if(_isReplay)
        {            
            if(_currentFrame == _spawnFrame)
            {
                ShowSelf();
                transform.position = _startPosition;
            }

            _currentFrame++;
            return;
        }

        if(_isReversing)
        {
            if(_currentFrame == _deactivateFrame)
            {
                ShowSelf();
            }

            _currentFrame--;
            return;
        }

        _currentFrame++;

    }

    protected void Activate()
    {
        Effect();
        HideSelf();
        _deactivateFrame = _currentFrame;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isActive && !_isReversing)
        {
            Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(_isReversing)
        {
            ShowSelf();
        }
    }

    private void HideSelf()
    {
        _spriteRenderer.color = new Color(255, 255, 255, 0);
        _isActive = false;
    }

    private void ShowSelf()
    {
        _spriteRenderer.color = _startColor;
        _isActive = true;
    }

    protected abstract void Effect();

    protected Vector3 RandomStartPos()
    {
        Vector3 pos = new Vector3(this.transform.position.x, UnityEngine.Random.Range(-1f, 1f) * _gameSettings.GameDifficulty);
        return pos;
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
        HideSelf();
    }
}
