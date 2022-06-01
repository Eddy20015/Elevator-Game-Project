using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WarningScene : MonoBehaviour
{
    [SerializeField] float wait;

    [SerializeField] Image image;

    bool fade;

    // Start is called before the first frame update
    void Start()
    {
        fade = false;
        StartCoroutine(MainMenu());
    }

    // Update is called once per frame
    void Update()
    {
        if (!fade)
        {
            if (image.color.a < 1)
            {
                image.color += Color.black * Time.deltaTime / wait;
            } else
            {
                image.color = Color.black;
            }
        } else
        {
            if (image.color.a > 0)
            {
                image.color -= Color.black * Time.deltaTime / wait;
            }
            else
            {
                image.color = Color.clear;
            }
        }
    }

    IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(1);
        fade = true;
        yield return new WaitForSeconds(wait);
        fade = false;
        yield return new WaitForSeconds(wait + 1);
        SceneManager.LoadScene("MainMenu");
    }
}
