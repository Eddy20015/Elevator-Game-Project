using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hide or show the tasks by pressing T
/// </summary>
public class HideOrShowTasks : MonoBehaviour
{
    [SerializeField] private GameObject targetPositionObject;
    //This is the background basically but this should be whatever that holds all the text together
    [SerializeField] private GameObject TaskUI;
    [SerializeField] private float transitionTime;
    [SerializeField] private Vector3 initialPosition;
    private bool pressed, hidden;
    // Start is called before the first frame update
    void Start()
    { 
        Debug.Log(initialPosition);
        initialPosition = TaskUI.transform.position;
        hidden = false;
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && pressed == false)
        {
            Debug.Log("PRESSED");
            pressed = true;
            if(hidden)
            {
                StartCoroutine(ShowTasks());
            }
            if(hidden == false)
            {
                StartCoroutine(HideTasks());
            }
        }
    }

    private IEnumerator ShowTasks()
    {
        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            TaskUI.transform.position = Vector3.Lerp(TaskUI.transform.position, initialPosition, elapsedTime /transitionTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        TaskUI.transform.position = initialPosition;
        hidden = false;
        pressed = false;
    }

    private IEnumerator HideTasks()
    {
        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            TaskUI.transform.position = Vector3.Lerp(TaskUI.transform.position, targetPositionObject.transform.position, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        TaskUI.transform.position = targetPositionObject.transform.position;
        hidden = true;
        pressed = false;
    }
}
