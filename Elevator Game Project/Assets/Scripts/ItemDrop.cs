using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Item will drop from above and hit the player
/// </summary>
public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private float destroyTime;
    [SerializeField] private AudioSource audio;
    private void Start()
    {
        item.SetActive(false);
        audio.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if it hits a player
        if(other.tag == "Player")
        {
            //Drop the item by activating it
            item.SetActive(true);
            audio.Play();
            Destroy(gameObject, destroyTime);
        }
    }
}
