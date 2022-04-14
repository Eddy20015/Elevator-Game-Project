using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster1 : Monster
{
    //Tim Kashani

    [SerializeField] GameObject eyes;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
        eyes.transform.LookAt(player.transform);
        eyes.transform.eulerAngles = new Vector3(0, eyes.transform.eulerAngles.y, 0);
    }

    public override void Chase()
    {
        agent.SetDestination(player.transform.position);

        float f = Vector3.Distance(transform.position, player.transform.position);

        if (f > 15)
        {
            agent.speed = speed;
        } else
        {
            agent.speed = speed * 2;
        }
    }
}
