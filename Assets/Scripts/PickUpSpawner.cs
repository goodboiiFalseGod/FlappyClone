using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private List<PickUp> pickUpsPrefabs;

    private List<PickUp> _activePickUps;

    private ColumnPool _columnPool;

    private int _currentChance = 0; //1 = 10%

    public void Init(ColumnPool columnPool)
    {
        _columnPool = columnPool;
        _columnPool.ColumnRestarted += SpawnPickUp;
        _activePickUps = new List<PickUp>();
    }

    private void Update()
    {
        if (_activePickUps == null)
            return;

        for (int i = 0; i < _activePickUps.Count; i++)
        {
            if (!_activePickUps[i].gameObject.active)
            {
                Destroy(_activePickUps[i].gameObject);
                _activePickUps.Remove(_activePickUps[i]);
                return;
            }

            if (_activePickUps[i].transform.position.x < Initilizer.GetCornerPosition(Corner.LeftUpper).x)
            {
                Destroy(_activePickUps[i].gameObject);
                _activePickUps.Remove(_activePickUps[i]);
            }
        }
    }

    public void SpawnPickUp(Vector2 position)
    {
        int chance = Random.Range(_currentChance, 10);
        if (chance >= 10)
        {
            PickUp instance = Instantiate(pickUpsPrefabs[Random.Range(0, pickUpsPrefabs.Count)]);
            instance.transform.position = position;
            instance.transform.Translate(Vector2.right * gameSettings.ColumnInterval / 2);
            instance.Init(gameSettings, playerHealth, null);
            _activePickUps.Add(instance);
            _currentChance = 0;
        }
        _currentChance++;
    }
}
