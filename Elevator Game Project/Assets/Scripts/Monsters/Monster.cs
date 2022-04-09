using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    //Tim Kashani

    [SerializeField] protected float speed;

    [SerializeField] protected GameObject player;

    protected NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    public virtual void Chase()
    {

    }
}
