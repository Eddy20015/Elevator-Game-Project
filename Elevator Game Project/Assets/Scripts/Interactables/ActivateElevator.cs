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
    [SerializeField] private float timeToOpenAndClose, howSmooth;
    private void Start()
    {
        initialLeftDoorPosition = leftDoor.transform.position;
        initialRightDoorPosition = rightDoor.transform.position;
        StartCoroutine(OpenLeftDoor());
        StartCoroutine(OpenRightDoor());
    }

    public void CloseDoors()
    {
        StartCoroutine(CloseLeftDoor());
        StartCoroutine(CloseRightDoor());
    }

    private IEnumerator OpenLeftDoor()
    {
        float elapsedTime = 0;
        while(elapsedTime < timeToOpenAndClose/10)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftOpenDoorPosition.transform.position, elapsedTime / timeToOpenAndClose);
            elapsedTime += howSmooth;
            yield return new WaitForSeconds(howSmooth);
            Debug.Log(elapsedTime / timeToOpenAndClose);
        }
    }

    private IEnumerator OpenRightDoor()
    {
        float elapsedTime = 0;
        while(elapsedTime < timeToOpenAndClose / 10)
        {
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightOpenDoorPosition.transform.position, elapsedTime / timeToOpenAndClose);
            elapsedTime += howSmooth;
            yield return new WaitForSeconds(howSmooth);
        }
    }

    private IEnumerator CloseLeftDoor()
    {
        float elapsedTime = 0;
        while (elapsedTime < timeToOpenAndClose / 10)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, initialLeftDoorPosition, elapsedTime / timeToOpenAndClose);
            elapsedTime += howSmooth;
            yield return new WaitForSeconds(howSmooth);
        }
    }

    private IEnumerator CloseRightDoor()
    {
        float elapsedTime = 0;
        while (elapsedTime < timeToOpenAndClose / 10)
        {
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, initialRightDoorPosition, elapsedTime / timeToOpenAndClose);
            elapsedTime += howSmooth;
            yield return new WaitForSeconds(howSmooth);
        }
    }


}
