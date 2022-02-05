using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplayEvents : MonoBehaviour
{
    public static TextDisplayEvents instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action OnTextFullyDisplayed;
    public event Action OnTextDisplayTimeElapsed;
    public event Action OnTextDisplayBeggining;

    public void TextFullyDisplayed()
    {
        if (OnTextFullyDisplayed != null)
        {
            OnTextFullyDisplayed();
        }
    }

    public void FullTextDisplayTimeElapsed()
    {
        if (OnTextDisplayTimeElapsed != null)
        {
            OnTextDisplayTimeElapsed();
        }
    }

    public void TextDisplayBeggining()
    {
        if (OnTextDisplayBeggining != null)
        {
            OnTextDisplayBeggining();
        }
    }
}
