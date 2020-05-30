using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Toaster : MonoBehaviour
{
    public Text txt;
    public static Toaster _instance;
    
    public static Toaster Instance
    {
        get 
        {
            return _instance;
        }
    }

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

    public void showToast(string text,int duration) 
    {
        Instance.StartCoroutine(showToastCoroutine(text,duration));
    }

    private IEnumerator showToastCoroutine(string text, int duration) 
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

    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration) 
    {
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
}
