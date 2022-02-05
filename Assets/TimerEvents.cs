using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEvents : MonoBehaviour
{
    public static TimerEvents instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action<int> OnTimerEnding;

    public void TimerEnding(int id)
    {
        OnTimerEnding?.Invoke(id);
    }
}
