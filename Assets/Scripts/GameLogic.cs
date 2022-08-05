using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] Text _hpText;
    [SerializeField] Text _scoreText;

    private int _score = 0;

    public void OnPlayerHit(int hp)
    {
        _hpText.text = hp.ToString();
    }

    public void OnColumnPass()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    public void Start()
    {
        _scoreText.text = _score.ToString();
    }
}
