using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if(ChargingStationManager.chargingStationManager.GetPuzzleState())
        {
            StartCoroutine(CloseLeftDoor());
            StartCoroutine(CloseRightDoor());
        }
    }

    private IEnumerator OpenLeftDoor()
    {
        float elapsedTime = 0;
        while(elapsedTime < timeToOpenAndClose)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftOpenDoorPosition.transform.position, elapsedTime / timeToOpenAndClose);
            elapsedTime += howSmooth;
            yield return new WaitForSeconds(howSmooth);
        }
    }

    private IEnumerator OpenRightDoor()
    {
        float elapsedTime = 0;
        while(elapsedTime < timeToOpenAndClose)
        {
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightOpenDoorPosition.transform.position, elapsedTime / timeToOpenAndClose);
            elapsedTime += howSmooth;
            yield return new WaitForSeconds(howSmooth);
        }
    }

    private IEnumerator CloseLeftDoor()
    {
        float elapsedTime = 0;
        while (elapsedTime < timeToOpenAndClose)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, initialLeftDoorPosition, elapsedTime / timeToOpenAndClose);
            elapsedTime += howSmooth;
            yield return new WaitForSeconds(howSmooth);
        }
    }

    private IEnumerator CloseRightDoor()
    {
        float elapsedTime = 0;
        while (elapsedTime < timeToOpenAndClose)
        {
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, initialRightDoorPosition, elapsedTime / timeToOpenAndClose);
            elapsedTime += howSmooth;
            yield return new WaitForSeconds(howSmooth);
        }
    }


}
