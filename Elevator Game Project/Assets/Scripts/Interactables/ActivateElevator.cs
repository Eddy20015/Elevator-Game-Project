using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Opens the door when game starts
/// Closes the door when all players are in the elevator and the puzzles are done
/// </summary>
public class ActivateElevator : MonoBehaviour
{
    [SerializeField] private GameObject leftDoor, rightDoor;
    [SerializeField] private GameObject leftOpenDoorPosition, rightOpenDoorPosition;
    [SerializeField] private Vector3 initialLeftDoorPosition, initialRightDoorPosition;
    [SerializeField] private float timeToOpenAndClose, BeforeDoorsOpen;
    [SerializeField] private AudioSource OpenDoorsAudio;
    [SerializeField] private AudioSource CloseDoorsAudio;

    private bool ClosedDoorsStarted;

    private void Start()
    {
        initialLeftDoorPosition = leftDoor.transform.position;
        initialRightDoorPosition = rightDoor.transform.position;
        OpenDoorsAudio.Play();
        StartCoroutine(OpenLeftDoor());
        StartCoroutine(OpenRightDoor());
    }

    public void CloseDoors()
    {
        CloseDoorsAudio.Play();
        ClosedDoorsStarted = true;
        timeToOpenAndClose *= 1.9f;
        StartCoroutine(CloseLeftDoor());
        StartCoroutine(CloseRightDoor());
    }

    private IEnumerator OpenLeftDoor()
    {
        yield return new WaitForSeconds(BeforeDoorsOpen);
        float elapsedTime = 0;
        while(elapsedTime < timeToOpenAndClose)
        {
            if (ClosedDoorsStarted)
            {
                break;
            }
            leftDoor.transform.position = Vector3.Lerp(initialLeftDoorPosition, leftOpenDoorPosition.transform.position, elapsedTime / timeToOpenAndClose);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            //Debug.Log(elapsedTime / timeToOpenAndClose);
        }
        leftDoor.transform.position = leftOpenDoorPosition.transform.position;
    }

    private IEnumerator OpenRightDoor()
    {
        yield return new WaitForSeconds(BeforeDoorsOpen);
        float elapsedTime = 0;
        while(elapsedTime < timeToOpenAndClose )
        {
            if (ClosedDoorsStarted)
            {
                break;
            }
            rightDoor.transform.position = Vector3.Lerp(initialRightDoorPosition, rightOpenDoorPosition.transform.position, elapsedTime / timeToOpenAndClose);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        rightDoor.transform.position = rightOpenDoorPosition.transform.position;
    }

    private IEnumerator CloseLeftDoor()
    {
        float elapsedTime = 0;
        while (elapsedTime < timeToOpenAndClose)
        {
            leftDoor.transform.position = Vector3.Lerp(leftOpenDoorPosition.transform.position, initialLeftDoorPosition, elapsedTime / timeToOpenAndClose);
            elapsedTime += Time.deltaTime;
            //Debug.Log(elapsedTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        leftDoor.transform.position = initialLeftDoorPosition;
    }

    private IEnumerator CloseRightDoor()
    {
        float elapsedTime = 0;
        while (elapsedTime < timeToOpenAndClose )
        {
            rightDoor.transform.position = Vector3.Lerp(rightOpenDoorPosition.transform.position, initialRightDoorPosition, elapsedTime / timeToOpenAndClose);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        rightDoor.transform.position = initialRightDoorPosition;
    }


}
