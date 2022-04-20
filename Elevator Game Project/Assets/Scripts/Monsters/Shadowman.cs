using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class Shadowman : Monster
{
    //Tim Kashani, Ed Slee

    [SerializeField] GameObject eyes;

    //public float Player1Distance;
    //public float Player2Distance;
    [SerializeField] float sphere1Radius;
    [SerializeField] float sphere2Radius;

    //points that the monster goes to while patrolling

    [SerializeField] Vector3[] points;
    int currentPoint;

    // Start is called before the first frame update
    void Start()
    {
        //get components

        agent = GetComponent<NavMeshAgent>();
        monsterCollider = GetComponent<Collider>();
        findTrigger = transform.Find("Find Player").GetComponent<SphereCollider>();
        view = gameObject.GetPhotonView();

        //will start the monster looking for a random point
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
          (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && PhotonNetwork.IsMasterClient))
        {
            RandomPoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //online, only the master client version will actually move around, the one from the persepective of not master just follows
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
          (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && PhotonNetwork.IsMasterClient))
        {
            if (player != null)
            {
                //Debug.LogError("Is view mine? " + player.GetPhotonView().IsMine);
                Chase();
                eyes.transform.LookAt(player.transform);
                eyes.transform.eulerAngles = new Vector3(0, eyes.transform.eulerAngles.y, 0);
            }
            else
            {
                Patrol();
                //Debug.LogError("Patrolling");
            }
        }
    }

    public override void Chase()
    {
        //chase after player
        agent.SetDestination(player.transform.position);

        float distanceBetween = Vector3.Distance(transform.position, player.transform.position);

        RaycastHit hit;

        Physics.Raycast(transform.position, eyes.transform.forward, out hit);

        bool foundPlayer = false;

        //not sure what this does - Ed
        if (hit.distance < findTrigger.radius)
        {
            if (Vector3.Distance(new Vector3(hit.point.x, player.transform.position.y, hit.point.z), player.transform.position) < 1)
            {
                foundPlayer = true;
            }
        }

        //Debug.LogError("foundPlayer == " + foundPlayer);
        if (distanceBetween > findTrigger.radius || !foundPlayer)
        {
            agent.speed = speed;
            isRunning = false;
        } 
        else
        {
            agent.speed = speed * 2;
            isRunning = true;
        }
        //Debug.LogError("isRunning == " + isRunning);

        //Debug.Log(foundPlayer);
        //Debug.Log(h.point);
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, points[currentPoint]) < 1 && !isRunning)
        {
            RandomPoint();
        }
    }

    void RandomPoint()
    {
        currentPoint = (int)Random.Range(0, points.Length - 0.01f);
        agent.SetDestination(points[currentPoint]);
        isRunning = false;
    }

    public override void SetPlayer(GameObject _player, bool b)
    {
        //has the player set the target for the monster because collision wasnt working

        if (b)
        {
            if(player == null)
            {
                player = _player;
            }
        } 
        else if (!isRunning)
        {
            if (player == _player)
            {
                player = null;
            }
        } 
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.LogError("IS THIS WORKING");
        //not running after a player
        if (!isRunning)
        {
            //Debug.LogError("PlayerCollider? " + other.tag.Equals("PlayerCollider") + ", " + other.tag.Equals("Player"));
            //the gameobject as the player tag
            if (other.tag.Equals("PlayerCollider") || other.tag.Equals("Player"))
            {
                if(player == null)
                {
                    //make this new player the one that will be chased
                    player = other.gameObject;
                    //Debug.LogError("player = " + player + "is view mine?" + player.GetPhotonView().IsMine);
                }
            }

            //Debug.Log("Is Not Running");
        }

        //Debug.Log("What");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isRunning)
        {
            player = null;
            //Debug.LogError("player is now null");
        }
        //Debug.LogError("Exited the Trigger");
    }
}