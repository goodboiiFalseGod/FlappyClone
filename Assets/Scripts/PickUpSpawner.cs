using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private List<PickUp> _pickUpsPrefabs;
    [SerializeField] private TimeManager _timeManager;

    private List<PickUp> _activePickUps;

    private ColumnPool _columnPool;

    private int _currentChance = 5; //1 = 10%
    private ulong _currentFrame = 0;

    public void Init(ColumnPool columnPool)
    {
        _columnPool = columnPool;
        _columnPool.ColumnRestarted += SpawnPickUp;
        _activePickUps = new List<PickUp>();
    }

    private void FixedUpdate()
    {
        _currentFrame++;
    }

    public void SpawnPickUp(Vector2 position)
    {
        int chance = Random.Range(_currentChance, 10);
        if (chance >= 10)
        {
            PickUp instance = Instantiate(_pickUpsPrefabs[Random.Range(0, _pickUpsPrefabs.Count)]);
            instance.transform.position = position;
            instance.transform.Translate(Vector2.right * _gameSettings.ColumnInterval / 2);
            instance.Init(_gameSettings, _playerHealth, _playerMovement, _timeManager, _currentFrame);
            _activePickUps.Add(instance);
            _currentChance = 0;
        }
        _currentChance++;
    }
}
