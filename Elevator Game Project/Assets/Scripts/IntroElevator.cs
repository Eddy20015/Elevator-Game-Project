using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The moment a player walks inside the elevator, it plays the sequence of eventsa
/// </summary>
public class IntroElevator : MonoBehaviour
{
    [SerializeField] private ActivateElevator elevatorAnims;
    [SerializeField] private GameObject invisibleCollider;
    [SerializeField] private FadingScript fadingScript;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(BeginGame());
        }
    }

    private IEnumerator BeginGame()
    {
        //Activate the collider
        invisibleCollider.SetActive(true);

        yield return new WaitForSeconds(5f);
        //Close the elevators 
        elevatorAnims.CloseDoors();

        yield return new WaitForSeconds(0.5f);
        //Fade In
        fadingScript.FadeIn();

        yield return new WaitForSeconds(1f);
        //LOAD SCENE

        Cursor.lockState = CursorLockMode.None;
    }

}
