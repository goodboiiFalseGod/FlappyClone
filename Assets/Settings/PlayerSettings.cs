using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] private float _jumpStrenght;
    [SerializeField] private int _hp;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _invincibilityTime;

    public Vector2 JumpStrenght { get => Vector2.up * _jumpStrenght; }
    public int HP { get => _hp; }
    public float JumpCooldown { get => _jumpCooldown; }
    public float invincibilityTime { get => _invincibilityTime; }
}
