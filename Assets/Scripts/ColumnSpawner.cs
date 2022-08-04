using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _columnPrefab;
    private GameSettings _gameSettings;
    private int _columnCount = 7;

    public void Init(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        _columnCount = CalculateColumnCount();
    }

    private int CalculateColumnCount()
    {
        //int count = Mathf.RoundToInt(Camera.main.scre / _gameSettings.ColumnInterval) + 1;

        return 7;
    }

    private void Start()
    {
        /*for (int i = 0; i < _columnCount; i++)
        {
            GameObject instance = Instantiate(_columnPrefab, this.transform);
            instance.GetComponent<Column>().Init(_gameSettings, _columnCount);
            instance.transform.localPosition += Vector3.right * _gameSettings.ColumnInterval * i;
        }*/

        StartCoroutine(spawner());
    }

    IEnumerator spawner()
    {        
        for (int i = 0; i < _columnCount; i++)
        {
            GameObject instance = Instantiate(_columnPrefab, this.transform);
            instance.GetComponent<Column>().Init(_gameSettings, _columnCount);
            instance.transform.localPosition += Vector3.right * _gameSettings.ColumnInterval * i;
            yield return new WaitForSeconds(_gameSettings.ColumnInterval / _gameSettings.GameSpeed);
        }
    }
}
