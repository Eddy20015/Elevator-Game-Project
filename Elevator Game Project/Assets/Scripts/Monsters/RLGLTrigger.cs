using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLTrigger : MonoBehaviour
{
    [SerializeField] private RLGL Monster;
    [SerializeField] private RLGLTriggerManager Manager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Monster.TryActivateMonster(other.gameObject);
            Manager.TeleportTrigger(this);
            //include manager system that then moves them
        }
    }
}
