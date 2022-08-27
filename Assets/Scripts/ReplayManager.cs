using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayManager : MonoBehaviour
{
    [SerializeField] private Button _replayButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TimeManager _timeManager;

    public void ReplayButtonClick()
    {
        _replayButton.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(false);
        _timeManager.StartReplay();
    }
}
