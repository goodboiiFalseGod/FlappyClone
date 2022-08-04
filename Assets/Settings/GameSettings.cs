using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    [SerializeField] private float _columnInterval;
    [SerializeField] private float _gameSpeed;
    [SerializeField] private float _gameDifficulty;
    [SerializeField] private float _windowSize;

    public float ColumnInterval { get => _columnInterval; }
    public float GameSpeed { get => _gameSpeed; }
    public float GameDifficulty { get => _gameDifficulty; }
    public float WindowSize { get => _windowSize; }
}
