using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vignette : MonoBehaviour
{
    [SerializeField] private Image vignette;
    [Range(0.0f, 0.10f)] [SerializeField] private float revealSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Reveals opaqeu vignette
    public void RevealOpaque()
    {
        Color temp = vignette.color;
        temp.a = 1f;
        vignette.color = temp;

        //StartCoroutine(RevealOpaqueVignette());
    }

    /*
    private IEnumerator RevealOpaqueVignette()
    {
        Color temp = vignette.color;
        for (float a = temp.a; a < 1; a += revealSpeed)
        {
            temp.a = a;
            vignette.color = temp;
            yield return new WaitForSeconds(0.01f);
        }
        //Ensures it is pitch black
        temp.a = 1;
        vignette.color = temp;
    }
    */

    //Reveals transluscent vignette 
    public void RevealTransluscent()
    {
        Color temp = vignette.color;
        temp.a = 0.5f;
        vignette.color = temp;

        //StartCoroutine(RevealTransluscentVignette());
    }

    /*
    private IEnumerator RevealTransluscentVignette()
    {
        Color temp = vignette.color;
        for (float a = temp.a; a < 0.5f; a += revealSpeed)
        {
            temp.a = a;
            vignette.color = temp;
            yield return new WaitForSeconds(0.01f);
        }
        //Ensures it is pitch black
        temp.a = 1;
        vignette.color = temp;
    }
    */
}
