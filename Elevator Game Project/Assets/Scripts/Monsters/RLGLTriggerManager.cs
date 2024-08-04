using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLTriggerManager : MonoBehaviour
{
    //Must be the same size
    [SerializeField] private Transform[] TeleportPoints;
    [SerializeField] private RLGLTrigger[] Triggers;
    private int CurrIdx;
    private int NumTriggers;

    public void Awake()
    {
        for(int i = 0; i < TeleportPoints.Length; i++)
        {
            int r = Random.Range(i, TeleportPoints.Length - 1);
            Transform temp = TeleportPoints[i];
            TeleportPoints[i] = TeleportPoints[r];
            TeleportPoints[r] = temp;

            if (Triggers[i] != null)
            {
                NumTriggers++;
                Triggers[i].transform.position = TeleportPoints[i].position;
                Triggers[i].transform.rotation = TeleportPoints[i].rotation;
            }
        }

    }

    public void TeleportTrigger(RLGLTrigger CurrTrigger)
    {
        CurrIdx = CurrIdx < TeleportPoints.Length ? CurrIdx : 0;
        Triggers[CurrIdx].transform.position = TeleportPoints[(CurrIdx + NumTriggers) % TeleportPoints.Length].position;
        Triggers[CurrIdx].transform.rotation = TeleportPoints[(CurrIdx + NumTriggers) % TeleportPoints.Length].rotation;
        Triggers[(CurrIdx + NumTriggers) % TeleportPoints.Length] = Triggers[CurrIdx];
        Triggers[CurrIdx] = null;
        CurrIdx++;
    }
}