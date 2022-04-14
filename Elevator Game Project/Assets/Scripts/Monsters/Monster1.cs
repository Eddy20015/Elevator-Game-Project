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

        RaycastHit h;

        Physics.Raycast(transform.position, eyes.transform.forward, out h);

        bool foundPlayer = false;

        if (h.distance < 15)
        {
            if (Vector3.Distance(new Vector3(h.point.x, player.transform.position.y, h.point.z), player.transform.position) < 1)
            {
                foundPlayer = true;
            }
        }

        

        if (f > 15 || !foundPlayer)
        {
            agent.speed = speed;
        } else
        {
            agent.speed = speed * 2;
        }

        Debug.Log(foundPlayer);
        Debug.Log(h.point);
    }
}
