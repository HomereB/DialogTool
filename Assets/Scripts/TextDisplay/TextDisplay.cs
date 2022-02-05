using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    public bool isFullTextDisplayedInstantly;
    public float textDisplaySpeedSetting;
    bool isTextFullyDisplayed = true;
    [SerializeField]
    TMP_Text txtmpTextBox;
    [SerializeField]
    Scrollbar textVerticalScrollbar;

    string currentText = "sqdg\nqSDEF\n\n\nedfqsgsrsdgsqg\nqzsgf\nqzgfqsg\nqsdfgwqsdfg\nqsdgqsfdg";
    float displayTime;
    float timeToDisplay = 1;

    float TimeToDisplayCharacter = 0.05f;

    Coroutine WriteTextCoroutine = null;

    public string skipTextKeyCode;

    // Start is called before the first frame update
    private void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DisplayText();
        }
    }

    void DisplayText()
    {
/*        if(isTextFullyDisplayed)
        {

        }
        else
        {*/
            Debug.Log("blub");
            txtmpTextBox.text = currentText;
            txtmpTextBox.maxVisibleCharacters = 0;
            WriteTextCoroutine = StartCoroutine(WriteText(isFullTextDisplayedInstantly));
        //}
    }

    IEnumerator WriteText(bool isFullTextDisplayedInstantly)
    {
        yield return new WaitForEndOfFrame();

        int caractercount = txtmpTextBox.textInfo.characterCount;
        float displayspeed = TimeToDisplayCharacter * timeToDisplay / textDisplaySpeedSetting;
        int cptr = 0;
        

        if (isFullTextDisplayedInstantly)
            cptr = caractercount;

        while (cptr < caractercount)
        {
            textVerticalScrollbar.value = 1; //TODO: handle vertical scrollbar better according to amount of text revealed
            txtmpTextBox.maxVisibleCharacters = cptr;
            yield return new WaitForSeconds(displayspeed);

            cptr++;
        }

        isTextFullyDisplayed = true;
    }

    //TODO: Add Event for text fully displayed


    //TODO: Add event for displayTime over

}
