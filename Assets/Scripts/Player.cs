using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerSettings _settings;
    [SerializeField] GameSettings _gameSettings;
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] Text _hpText;
    private Rigidbody2D _rigidbody;
    private Vector2 _jumpStrenght;
    private float _jumpCooldown;
    private float _jumpCurrentCooldown;
    private int _hp;
    private Vector2 _startPos;
    private bool _isInvincible = false;
    [SerializeField] private Collider2D collider2D;

    public Action<int> Hitted;

    private bool _jumpOnCooldown { get => _jumpCurrentCooldown > 0; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _jumpStrenght = _settings.JumpStrenght;
        _jumpCooldown = _settings.JumpCooldown;
        _hp = _settings.HP;
        _hpText.text = _hp.ToString();
        _startPos = CalculateStartPos();
        Hitted += OnPlayerHit;
    }

    private Vector2 CalculateStartPos()
    {
        Vector2 result = new Vector2((Initilizer.GetCorners()[Corners.rUpper].x - Initilizer.GetCorners()[Corners.lUpper].x) / 5 + Initilizer.GetCorners()[Corners.lUpper].x, transform.position.y);
        return result;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.anyKey && !_jumpOnCooldown)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(_jumpStrenght);
            _jumpCurrentCooldown = _jumpCooldown;
        }

        if(_jumpOnCooldown)
        {
            _jumpCurrentCooldown -= Time.deltaTime;
        }

        transform.position = new Vector3(_startPos.x, transform.position.y);

        if (this.transform.position.y < Initilizer.GetCorners()[Corners.lLower].y)
        {
            GetHit();
            _rigidbody.velocity = Vector2.zero;
            transform.position = _startPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetHit();
    }

    private void GetHit()
    {
        if (_hp <= 0 & !_isInvincible)
        {
            Time.timeScale = 0;
            _restartButton.gameObject.SetActive(true);
        }
        else if (_hp > 0 & !_isInvincible)
        {
            StartCoroutine(Invicibility());
            _hp--;
        }

        Hitted?.Invoke(_hp);
    }

    public void OnPlayerHit(int hp)
    {
        _hpText.text = hp.ToString();
    }

    private IEnumerator Invicibility()
    {
        _isInvincible = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        collider2D.enabled = false;
        yield return new WaitForSeconds(2);
        _isInvincible = false;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        collider2D.enabled = true;
    }
}
