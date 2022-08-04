using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initilizer : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private ColumnSpawner _columnSpawner;

    public void Awake()
    {
        _columnSpawner.Init(_gameSettings);
    }
}
