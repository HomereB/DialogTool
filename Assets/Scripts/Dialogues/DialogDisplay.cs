using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using System;
using TMPro;

public class DialogDisplay : MonoBehaviour
{
    Dialog currentDialog;

    Node currentNode;
    int currentDialogNodeIndex;

    Reply currentReply;
    int currentDialogReplyIndex;

    [SerializeField]
    GameObject characterPortaitLeft;
    Image imgCharacterPortraitLeft;

    [SerializeField]
    GameObject characterPortraitRight;
    Image imgCharacterPortraitRight;

    [SerializeField]
    GameObject characterName;
    TMP_Text txtCharacterName;



    [SerializeField]
    GameObject answerButtonPrefab;
    [SerializeField]
    GameObject answerButtonsPanel;
    [SerializeField]
    List<AnswerButton> answerButtons;

    //TODO : add to separate text display
    public float textDisplaySpeedSetting;
    public bool autoSkip = true;
    bool isTextFullyDisplayed = true;
    [SerializeField]
    GameObject dialogBox;
    TMP_Text txtmpDialogBox;
    [SerializeField]
    Scrollbar dialogVerticalScrollbar;


    bool areAnswersDisplayed = false;
    public Timer answerTimer;  //Add default answer

    float TimeToDisplayCharacter = 0.05f;


    //TODO: check to send diallog coroutine to text display
    Coroutine WriteDialogCoroutine = null;
    Coroutine AutoSkipCoroutine = null;

    private void Awake()
    {
        imgCharacterPortraitLeft = characterPortaitLeft.GetComponent<Image>();

        imgCharacterPortraitRight = characterPortraitRight.GetComponent<Image>();

        txtCharacterName = characterName.GetComponent<TMP_Text>();

        txtmpDialogBox = dialogBox.GetComponent<TMP_Text>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DisplayReply();             
        }
    }

    public void SetCurrentDialog(Dialog dialog)
    {
        currentDialog = dialog;
    }

    public void StartDialog()
    {
        currentDialogNodeIndex = 0;
        currentDialogReplyIndex = 0;

        DisplayNode(currentDialogNodeIndex);
    }

    public void DisplayNode(int nodeID)
    {
        isTextFullyDisplayed = true;
        if (nodeID >= 0)
        {
            currentNode = currentDialog.Node[nodeID];
            currentDialogNodeIndex = nodeID;

            currentDialogReplyIndex = 0;
            DeleteAnswerButtons();
            DisplayReply();
        }
        else
            EndDialog();
    }

    void DisplayReply()
    {
        if (!isTextFullyDisplayed)
        {
            StopCoroutine(WriteDialogCoroutine);
            WriteDialogCoroutine = StartCoroutine(WriteReply(true));
        }
        else
        {
            answerTimer.timerUI.DisplayUI(false);

            if(AutoSkipCoroutine != null)
                StopCoroutine(AutoSkipCoroutine);

            isTextFullyDisplayed = false;
            if (currentDialogReplyIndex < currentDialog.Node[currentDialogNodeIndex].Reply.Count)
            {
                currentReply = currentDialog.Node[currentDialogNodeIndex].Reply[currentDialogReplyIndex];
                txtCharacterName.text = currentReply.CharacterID.ToString();
                txtmpDialogBox.text = currentReply.Text;
                txtmpDialogBox.maxVisibleCharacters = 0;
                WriteDialogCoroutine = StartCoroutine(WriteReply(false));
                DisplayCharacterImages(currentReply.CharacterImageLeft, currentReply.CharacterImageRight);
            }
            else if (currentDialogReplyIndex >= currentDialog.Node[currentDialogNodeIndex].Reply.Count && !areAnswersDisplayed)
            {
                DisplayAnswers();
            }
            currentDialogReplyIndex++;
        }
    }

    private void DisplayCharacterImages(string left, string right)
    {
        Sprite leftCharacterSprite = Resources.Load<Sprite>(left);
        Sprite rightCharacterSprite = Resources.Load<Sprite>(right);

        imgCharacterPortraitLeft.sprite = leftCharacterSprite;
        imgCharacterPortraitRight.sprite = rightCharacterSprite;
    }

    void DisplayAnswers()
    {
        areAnswersDisplayed = true;
        Answers currentAnswers = currentNode.Answers;
        bool isTimerDisplayed = true;
        bool isTimerPaused = false;
        if (currentAnswers.AnswerTime <= 0)
        {
            isTimerDisplayed =  false;
            isTimerPaused = true;
        }

        answerTimer.SetTimerValues(currentAnswers.AnswerTime, 0.0f, isTimerPaused, isTimerDisplayed);

        for(int i=0;i< currentAnswers.Choices.Count;i++)
        {
            Choice choice = currentAnswers.Choices[i];
            bool isDefaultChoice = false;
            if (i == currentAnswers.defaultChoice)
                isDefaultChoice = true;
            GameObject button = GameObject.Instantiate(answerButtonPrefab, answerButtonsPanel.transform);
            AnswerButton ab = button.GetComponent<AnswerButton>();

            ab.SetCurrentAnswer(this, choice.ChoiceText, choice.DisplayTime, choice.TimeBeforeDisplay, answerTimer,choice.NextNodeID, isDefaultChoice);
            answerButtons.Add(ab);
        }
    }

    void DeleteAnswerButtons()
    {
        areAnswersDisplayed = false;
        foreach (AnswerButton ab in answerButtons)
        {
            GameObject.Destroy(ab.gameObject);
        }
        answerButtons = new List<AnswerButton>();
    }

    private void EndDialog()
    {
        throw new NotImplementedException();
    }

    IEnumerator WriteReply(bool displayFullReply)
    {
        yield return new WaitForEndOfFrame();

        int caractercount = txtmpDialogBox.textInfo.characterCount;
        float displayspeed = TimeToDisplayCharacter * currentReply.TimeToDisplay / textDisplaySpeedSetting;
        int cptr = 0;

        if (displayFullReply)
            cptr = caractercount;

        while(cptr <= caractercount)
        {
            dialogVerticalScrollbar.value = 0; //TODO: handle vertical scrollbar better according to amount of text revealed
            txtmpDialogBox.maxVisibleCharacters = cptr;
            yield return new WaitForSeconds(displayspeed);

            cptr++;
        }

        isTextFullyDisplayed = true;
        if (autoSkip)
        {
            AutoSkipCoroutine = StartCoroutine(SkipToNextReply());
        }
    }

    IEnumerator SkipToNextReply()
    {
        yield return new WaitForSeconds(currentReply.DisplayTime);
        DisplayReply();
    }
}
