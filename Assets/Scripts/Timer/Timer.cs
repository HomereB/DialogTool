using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float beginTime;
    public float currentTime;
    public bool isPaused;
    public bool isTimeIncreasing;

    public bool hasEndTime;
    public float endTime;

    public TimerUI timerUI;

    void Start()
    {
        currentTime = beginTime;
        timerUI.SetTimer(this);
        isPaused = true;
    }

    void Update()
    {
        if(!isPaused)
        {
            ComputeCurrentTime();
            if(timerUI!=null)
            {
                timerUI.UpdateUI();
            }
        }
    }

    void ComputeCurrentTime()
    {
        if(isTimeIncreasing)
        {
            currentTime += Time.deltaTime;
            if(currentTime>=endTime && hasEndTime)
            {
                currentTime = endTime;
                SetPause(true);
                TimerEvents.instance.TimerEnding(gameObject.GetInstanceID());
            }
        }
        else
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= endTime && hasEndTime)
            {
                currentTime = endTime;
                SetPause(true);
                TimerEvents.instance.TimerEnding(this.GetInstanceID());
            }
        }
    }

    public void ResetTimer()
    {
        currentTime = beginTime;
        timerUI.UpdateUI();
        isPaused = true;
    }

    public void SetTimerValues(float beginValue, bool TimeIncreasing, bool Paused, bool UIDispalyed)
    {
        beginTime = beginValue;
        currentTime = beginTime;
        isTimeIncreasing = TimeIncreasing;
        hasEndTime = false;

        isPaused = Paused;
        timerUI.DisplayUI(UIDispalyed);
    }

    public void SetTimerValues(float beginValue,float endValue,bool Paused, bool UIDisplayed)
    {
        SetTimerValues(beginValue, true, Paused, UIDisplayed);
        endTime = endValue;
        hasEndTime = true;

        if (endTime > beginTime)
            isTimeIncreasing = true;
        else
            isTimeIncreasing = false;
    }

    public void SetPause(bool pause)
    {
        isPaused = pause;
    }
}
