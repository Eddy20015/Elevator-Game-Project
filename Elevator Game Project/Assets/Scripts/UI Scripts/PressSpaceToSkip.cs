using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PressSpaceToSkip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CornerText;
    private bool SpacePressed;
    private bool Invisible;

     // Update is called once per frame
    void Update()
    {
        //when space is pressed, the text should become fully visible and switch to Waiting
        if (SpacePressed)
        {
            CornerText.text = "Waiting...";
            CornerText.color = new Color32(255, 255, 255, 255);
        }
        //slowly lower the alpha of the normal text
        else if(!Invisible)
        {
            CornerText.color = new Color(1.0f, 1.0f, 1.0f, CornerText.color.a - (Time.deltaTime * 0.25f));
            if(CornerText.color.a <= 0)
            {
                Invisible = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpacePressed = true;
        }
    }
}
