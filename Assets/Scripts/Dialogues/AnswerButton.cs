using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AnswerButton : MonoBehaviour
{
    string currentAnswerResponse;
    float currentAnswerDisplayTime;
    float currentAnswerDisplayBeginning;
    AnswerStatus answerStatus = AnswerStatus.notYetDisplayed;
    public bool isDefaultAnswer = false;

    Timer answerTimer;

    public TMP_Text answerText;

    DialogDisplay dialogDisplay;
    int nextNodeID;

    private void Start()
    {
        TimerEvents.instance.OnTimerEnding += OnSelectingDefaultChoice;
    }

    void Update()
    {    
        if (answerTimer.currentTime <= answerTimer.beginTime - currentAnswerDisplayBeginning && answerStatus == AnswerStatus.notYetDisplayed)
        {
            DisplayAnswer();
        }
        else if (answerTimer.beginTime - currentAnswerDisplayBeginning - answerTimer.currentTime > currentAnswerDisplayTime &&answerStatus == AnswerStatus.currentlyDisplayed)
        {
            EraseAnswer();
        }
    }

    public void SetCurrentAnswer(DialogDisplay display, string answerText, float answerDisplayTime, float timeToDisplayAnswer, Timer timer, int nodeID, bool isDefaultChoice)
    {
        dialogDisplay = display;
        currentAnswerResponse = answerText;
        currentAnswerDisplayBeginning = timeToDisplayAnswer;
        currentAnswerDisplayTime = answerDisplayTime;
        answerTimer = timer;
        EraseAnswer();
        answerStatus = AnswerStatus.notYetDisplayed;
        isDefaultAnswer = isDefaultChoice;
        nextNodeID = nodeID;
    }

    private void DisplayAnswer()
    {
        answerText.enabled = true;
        answerStatus = AnswerStatus.currentlyDisplayed;
        answerText.text = currentAnswerResponse;
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.GetComponent<Button>().enabled = true;
    }

    void EraseAnswer()
    {
        answerStatus = AnswerStatus.alreadyDisplayed;
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponent<Button>().enabled = false;
        answerText.enabled = false;
    }

    public void OnClick()
    {
        dialogDisplay.DisplayNode(nextNodeID);
        answerTimer.ResetTimer();
    }

    enum AnswerStatus
    {
        notYetDisplayed,
        currentlyDisplayed,
        alreadyDisplayed
    }

    private void OnSelectingDefaultChoice(int id)
    {
        if (id == answerTimer.GetInstanceID())
        {
            if(isDefaultAnswer)
                OnClick();
        }
    }
}
