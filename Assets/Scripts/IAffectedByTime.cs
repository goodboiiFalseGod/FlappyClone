using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectedByTime
{
    abstract void SetReversing(bool state);
    abstract void StartReplay();

}
