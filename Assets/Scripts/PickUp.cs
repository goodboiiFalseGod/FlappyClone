using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    protected GameSettings _gameSettings;
    protected PlayerHealth _playerHealth;
    protected PlayerMovement _playerMovement;

    public void Init(GameSettings gameSettings, PlayerHealth playerHealth, PlayerMovement playerMovement)
    {
        _gameSettings = gameSettings;
        _playerHealth = playerHealth;
        _playerMovement = playerMovement;
        this.transform.position = RandomStartPos();
    }

    protected void Update()
    {
        this.transform.Translate(-_gameSettings.GameSpeed * Time.deltaTime, 0, 0);
    }

    protected void Activate()
    {
        Effect();
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Activate();
    }

    protected abstract void Effect();

    protected Vector3 RandomStartPos()
    {
        Vector3 pos = new Vector3(this.transform.position.x, UnityEngine.Random.Range(-1f, 1f) * _gameSettings.GameDifficulty);
        return pos;
    }
}
