using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Utility Class for showing android toast-like messages 
/// Used for debug messages and state informations
/// </summary>
public class Toaster : MonoBehaviour
{ 
    #region Singleton
    private static Toaster _instance;
    public static Toaster Instance
    {
        get 
        {
            return _instance;
        }
    }
    #endregion

    #region Public Variables
    public Text txt;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Utility Function to show toast
    /// </summary>
    /// <param name="text">The message string</param>
    /// <param name="duration">Duration to show the message in screen</param>
    public static void showToast(string text,int duration) 
    {
        Instance.StartCoroutine(Instance.showToastCoroutine(text,duration));
    }
    #endregion

    #region Private Coroutines
    /// <summary>
    /// Coroutine to display text message
    /// </summary>
    /// <param name="text">The message to display</param>
    /// <param name="duration">Duration to show the text</param>
    /// <returns></returns>
    IEnumerator showToastCoroutine(string text, int duration) 
    {
        Color originalColor = txt.color;
        txt.text = text;
        txt.enabled = true;

        yield return fadeInAndOut(txt, true, 0.5f);

        float counter = 0;
        while (counter < duration) 
        {
            counter += Time.deltaTime;
            yield return null;
        }

        yield return fadeInAndOut(txt, false, 0.5f);

        txt.enabled = false;
        txt.color = originalColor;
    }

    /// <summary>
    /// Coroutine to fade in and out the text
    /// </summary>
    /// <param name="targetText">Text to display</param>
    /// <param name="fadeIn">Fade In or Fade out</param>
    /// <param name="duration">Fade in for duration and fade out</param>
    /// <returns></returns>
    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration) 
    {
        // Alpha values for text display used to fade in or out
        float a , b;
        if (fadeIn) 
        {
            a = 0f;
            b = 1f;
        }
        else 
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;
        while (counter < duration) 
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
    #endregion
}
