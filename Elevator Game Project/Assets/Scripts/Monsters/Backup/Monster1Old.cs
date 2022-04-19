using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster1Old : Monster
{
    //Tim Kashani

    [SerializeField] GameObject eyes;

    [SerializeField] float sphere1;
    [SerializeField] float sphere2;

    // Start is called before the first frame update
    void Start()
    {
        //get components

        agent = GetComponent<NavMeshAgent>();
        monsterCollider = GetComponent<Collider>();

        //calculates the distance of all players and goes after the shortest one

        float d = 500;

        foreach (PlayerScript p in FindObjectsOfType<PlayerScript>())
        {
            float f = Vector3.Distance(transform.position, p.transform.position);
            if (f < d)
            {
                player = p;
                d = f;
            }
        }

        //if no player is within 500 meters just find one
        //if there are no playerscripts in scene then it will break

        if (player == null)
        {
            player = FindObjectOfType<PlayerScript>();
        }
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
        //chase after player



        agent.SetDestination(player.transform.position);

        float f = Vector3.Distance(transform.position, player.transform.position);

        RaycastHit h;

        //Physics.CheckSphere(transform.position, sphere1, out h, 3);

        //if (h.)
        //{
        //    player = h.transform.parent;
        //}

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
            isRunning = false;
        }
        else
        {
            agent.speed = speed * 2;
            isRunning = true;
        }

        //Debug.Log(foundPlayer);
        //Debug.Log(h.point);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isRunning)
        {
            if (other.tag == "Player")
            {
                if (other.GetComponent<PlayerScript>() != player)
                {
                    player = other.GetComponent<PlayerScript>();
                }
            }
        }
    }
