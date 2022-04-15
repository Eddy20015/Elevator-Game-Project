using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example of what a collectible would be
/// </summary>
public class Item : MonoBehaviour, ICollectible
{
    public void Collect()
    {
        //Do something
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            Collect();
        }
    }
}
