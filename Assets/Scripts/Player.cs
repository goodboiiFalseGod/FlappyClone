using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerSettings _settings;
    [SerializeField] private RestartButton _restartButton;
    private Rigidbody2D _rigidbody;
    private Vector2 _jumpStrenght;
    private float _jumpCooldown;
    private float _jumpCurrentCooldown;
    private int _hp;
    

    private Vector2 startPos;

    private bool _jumpOnCooldown { get => _jumpCurrentCooldown > 0; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _jumpStrenght = _settings.JumpStrenght;
        _jumpCooldown = _settings.JumpCooldown;
        _hp = _settings.HP;

        startPos = transform.position;
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

        transform.position = new Vector3(startPos.x, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Time.timeScale = 0;
        _restartButton.gameObject.SetActive(true);
    }
}
