using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    Timer timer;

    public Image imgTimerUI;
    public TMP_Text textTimerUI;
    public string baseText;
    public string timerFormat;

    public void UpdateUI()
    {
        if (imgTimerUI != null)
            UpdateTimerImage();
        if (textTimerUI != null)
            UpdateTimerText();
    }

    private void UpdateTimerImage()
    {
        float fillpercentage = 1.0f - Math.Abs((timer.currentTime - timer.beginTime) / (timer.beginTime - timer.endTime));
        imgTimerUI.fillAmount = fillpercentage;
    }
    private void UpdateTimerText()
    {
        TimeSpan time = TimeSpan.FromSeconds(timer.currentTime);
        textTimerUI.text = baseText + time.ToString(timerFormat);
    }

    public void DisplayUI(bool isUIDisplayed)
    {
        if (imgTimerUI != null)
            imgTimerUI.gameObject.SetActive(isUIDisplayed);
        if (textTimerUI != null)
            textTimerUI.gameObject.SetActive(isUIDisplayed);
        if(isUIDisplayed)
            UpdateUI();
    }

    public void SetTimer(Timer t)
    {
        timer = t;
    }
}
