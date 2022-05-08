using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Tasks : MonoBehaviourPunCallbacks
{
    //Tim Kashani

    [SerializeField] Text t;

    public class Task
    {
        public string name = "Task";
        public bool completed = false;
        //logic for how the task is completed
    }

    Task[] tasks;

    // Start is called before the first frame update
    void Start()
    {
        PhotonView view = gameObject.GetComponentInParent<PhotonView>();

        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
          (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && view.IsMine))
        {
            tasks = new Task[3];
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i] == null)
                {
                    tasks[i] = new Task();
                }
            }
            tasks[0].name = "Task 1";
            tasks[1].name = "Task 2";
            tasks[2].name = "Task 3";
        }
        else
        {
            t.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            CompleteTask();
        }*/
    }

    public void CompleteTask()
    {
        bool nextTask = true;
        string s = "Task: ";

        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].completed)
            {
                nextTask = true;
            } else if (nextTask)
            {
                tasks[i].completed = true;
                nextTask = false;
                s += tasks[i].name;
                t.text = s;
            }
        }
    }
}
