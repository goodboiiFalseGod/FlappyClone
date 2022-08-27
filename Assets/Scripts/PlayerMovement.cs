using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IAffectedByTime
{
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private TimeManager _timeManager;

    private Vector2 _jumpStrenght;
    private float _jumpCooldown;
    private float _jumpCurrentCooldown;
    
    private Vector2 _startPos;
 
    public event Action ReachedLethalHeight;
    public event Action HittedByObstacle;
    public event Action ScoreZoneEntered;
    public event Action ScoreZoneReEntered;

    private bool _jumpOnCooldown { get => _jumpCurrentCooldown > 0; }

    private Stack<Vector2> _velocitys;
    private Stack<Vector2> _positions;
    private Stack<Vector2> _velocitysReplay;
    private Stack<Vector2> _positionsReplay;
    public Action ReplayOvered;
    private bool _isReversing = false;
    private bool _isReplay = false;
    private Stack<Vector2> _positionsForReplay;

    private void Start()
    {
        _jumpStrenght = _settings.JumpStrenght;
        _jumpCooldown = _settings.JumpCooldown;
        _startPos = CalculateStartPos();
        _velocitys = new Stack<Vector2>();
        _positions = new Stack<Vector2>();
        _timeManager.AddNewAffectedByTime(this);
    }

    private Vector2 CalculateStartPos()
    {
        Vector2 result = new Vector2((Initilizer.GetCornerPosition(Corner.RightUpper).x - Initilizer.GetCornerPosition(Corner.LeftUpper).x) / 5 + Initilizer.GetCornerPosition(Corner.LeftUpper).x, transform.position.y);
        return result;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HittedByObstacle?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(_isReversing)
        {
            ScoreZoneReEntered?.Invoke();
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_isReversing)
        {
            ScoreZoneEntered?.Invoke();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_isReplay)
        {
            this.transform.position = _positionsReplay.Pop();
            _rigidbody.velocity = _velocitysReplay.Pop();
            ReplayOvered?.Invoke();
            return;
        }

        if (_isReversing)
        {
            FrameBack();
            return;
        }

        SaveLastFrame();

        CheckInBounds();

        transform.position = new Vector3(_startPos.x, transform.position.y);

        if (_jumpOnCooldown)
        {
            _jumpCurrentCooldown -= Time.deltaTime;
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(_jumpStrenght);
            _jumpCurrentCooldown = _jumpCooldown;
            return;
        }
    }

    private void CheckInBounds()
    {
        if (this.transform.position.y < Initilizer.GetCornerPosition(Corner.LeftLower).y || transform.position.y > Initilizer.GetCornerPosition(Corner.LeftUpper).y)
        {
            _rigidbody.velocity = Vector2.zero;
            transform.position = _startPos;
            ReachedLethalHeight?.Invoke();
        }
    }

    public void SetReversing(bool state)
    {
        _isReversing = state;
    }

    public void FrameBack()
    {
        if (_velocitys.Count == 0)
        {
            SetReversing(false);
            return;
        }
            
        _rigidbody.velocity = _velocitys.Pop();
        transform.position = _positions.Pop(); ;
    }

    public void SaveLastFrame()
    {
        _velocitys.Push(_rigidbody.velocity);
        _positions.Push(transform.position);
    }

    public void StartReplay()
    {
        _isReplay = true;
        _velocitysReplay = new Stack<Vector2>(_velocitys.ToArray());
        _positionsReplay = new Stack<Vector2>(_positions.ToArray());
    }
}
