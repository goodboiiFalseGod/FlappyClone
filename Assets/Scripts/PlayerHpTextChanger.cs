using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpTextChanger : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Text _textHp;

    private void Awake()
    {
        _playerHealth.HpChanged += OnPlayerHpChanged;
    }

    public void OnPlayerHpChanged(int hp)
    {
        _textHp.text = hp.ToString();
    }
}
