using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Has the functionality to fade in and fade out for transitions 
/// </summary>
public class FadingScript : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [Range(0.0f,0.10f)][SerializeField] private float fadeSpeed;

    private void Start()
    {
        blackScreen = GetComponent<Image>();
        FadeOut();
    }

    //Makes the panel turn pitch black slowly
    public void FadeIn()
    {
        StartCoroutine(FadeInBlack());
    }

    private IEnumerator FadeInBlack()
    {
        Color temp = Color.black;
        for(float a = 0.2f; a < 1; a += fadeSpeed)
        {
            temp.a = a;
            blackScreen.color = temp;
            yield return new WaitForSeconds(0.01f);
        }
        //Ensures it is pitch black
        temp.a = 1;
        blackScreen.color = temp;
    }

    //Makes the panel turn transparent
    public void FadeOut()
    {
        StartCoroutine(FadeOutBlack());
    }

    private IEnumerator FadeOutBlack()
    {
        Color temp = Color.black;
        for (float a = 1; a > 0.2; a -= fadeSpeed)
        {
            temp.a = a;
            blackScreen.color = temp;
            yield return new WaitForSeconds(0.01f);
        }
        //Ensures it is pitch black
        temp.a = 0.2f;
        blackScreen.color = temp;
    }
}
