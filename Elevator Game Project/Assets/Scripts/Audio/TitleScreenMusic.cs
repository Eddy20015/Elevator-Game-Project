using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenMusic : MonoBehaviour
{
    public static TitleScreenMusic titleScreenMusic;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton (Only one of these scripts can exist at the same time)

        if (titleScreenMusic == null)
        {
            titleScreenMusic = this;
        } else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Destroy()
    {
        //Destroys music when starting a level

        if (titleScreenMusic != null)
        {
            GameObject g = titleScreenMusic.gameObject;
            titleScreenMusic = null;
            Destroy(g);
        }
    }
}
