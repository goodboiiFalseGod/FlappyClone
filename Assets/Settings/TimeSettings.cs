using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSettings : ScriptableObject
{
    [SerializeField] float _timeForReverse;
    [SerializeField] float _reverseSpeed;

    public float TimeForReverse { get { return _timeForReverse; } }
    public float ReverseSpeed { get { return _reverseSpeed; } }
}
