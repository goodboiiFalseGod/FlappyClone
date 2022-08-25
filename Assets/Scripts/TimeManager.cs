using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private List<IAffectedByTime> affectedByTimes;
    private bool _isReversing = false;

    public void AddNewAffectedByTime(IAffectedByTime affectedByTime)
    {
        if(affectedByTimes == null)
        {
            affectedByTimes = new List<IAffectedByTime>();
        }
        affectedByTimes.Add(affectedByTime);
        affectedByTime.SetReversing(_isReversing);
    }

    public void RemoveAffectedByTime(IAffectedByTime affectedByTime)
    {
        if(affectedByTimes.Contains(affectedByTime))
        {
            affectedByTimes.Remove(affectedByTime);
        }
    }

    public void StartReplay()
    {
        Time.timeScale = 1f;
        foreach(IAffectedByTime affectedByTime in affectedByTimes)
        {
            affectedByTime.StartReplay();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(IAffectedByTime affectedByTime in affectedByTimes)
            {
                affectedByTime.SetReversing(true);
                _isReversing = true;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            foreach (IAffectedByTime affectedByTime in affectedByTimes)
            {
                affectedByTime.SetReversing(false);
                _isReversing = false;
            }
        }
    }
}
