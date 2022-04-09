using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example of something that is interactable 
/// </summary>
public class Something : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        //Do something
    }

    private void OnTriggerStay(Collider other)
    {
        //If they press a certain key button, interact
        if(true)
        {
            Interact();
        }
    }


}
