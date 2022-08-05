using System;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField] GameObject upperColumn;
    [SerializeField] GameObject lowerColumn;

    private GameSettings _gameSettings;
    private bool _alreadyPassed = false;

    public void Init(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        upperColumn.transform.Translate(Vector3.down * _gameSettings.WindowSize / 2);
        lowerColumn.transform.Translate(Vector3.up * _gameSettings.WindowSize / 2);
        transform.position = RandomStartPos();

    }

    private void Update()
    {
        transform.Translate(-_gameSettings.GameSpeed * Time.deltaTime, 0, 0);
    }

    private Vector3 RandomStartPos()
    {
        Vector3 pos = new Vector3(this.transform.position.x, UnityEngine.Random.Range(-1f, 1f) * _gameSettings.GameDifficulty);
        return pos;
    }
}
