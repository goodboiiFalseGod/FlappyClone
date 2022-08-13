using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private Rigidbody2D _rigidbody;

    private Vector2 _jumpStrenght;
    private float _jumpCooldown;
    private float _jumpCurrentCooldown;
    
    private Vector2 _startPos;
 
    public event Action ReachedLethalHeight;
    public event Action HittedByObstacle;
    public event Action ScoreZoneEntered;

    private bool _jumpOnCooldown { get => _jumpCurrentCooldown > 0; }

    private void Start()
    {
        _jumpStrenght = _settings.JumpStrenght;
        _jumpCooldown = _settings.JumpCooldown;
        _startPos = CalculateStartPos();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreZoneEntered?.Invoke();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.position.y < Initilizer.GetCornerPosition(Corner.LeftLower).y || transform.position.y > Initilizer.GetCornerPosition(Corner.LeftUpper).y)
        {
            _rigidbody.velocity = Vector2.zero;
            transform.position = _startPos;
            ReachedLethalHeight?.Invoke();
        }

        transform.position = new Vector3(_startPos.x, transform.position.y);

        if (_jumpOnCooldown)
        {
            _jumpCurrentCooldown -= Time.deltaTime;
            return;
        }

        if (Input.anyKey)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(_jumpStrenght);
            _jumpCurrentCooldown = _jumpCooldown;
            return;
        }
    }
}
