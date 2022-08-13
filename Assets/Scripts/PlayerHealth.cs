using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private PlayerMovement _playerMovement;

    private int _hp;
    private int Health { get => _hp; set { _hp = value; HpChanged?.Invoke(_hp); } }
    private bool _isInvincible = false;
    public event Action<int> HpChanged;

    private void Start()
    {
        Health = _settings.HP;
        _playerMovement.ReachedLethalHeight += GetHit;
        _playerMovement.HittedByObstacle += GetHit;
    }

    private void GetHit()
    {
        if (_isInvincible) return;

        if (Health <= 0)
        {
            Time.timeScale = 0;
            _restartButton.gameObject.SetActive(true);
            return;
        }

        StartCoroutine(Invicibility());
        --Health;
    }

    public void Heal(int _hp)
    {
        Health += _hp;
    }

    private IEnumerator Invicibility()
    {
        _isInvincible = true;
        _spriteRenderer.color = Color.red;
        _collider2D.enabled = false;
        yield return new WaitForSeconds(2);
        _isInvincible = false;
        _spriteRenderer.color = Color.white;
        _collider2D.enabled = true;
    }
}
