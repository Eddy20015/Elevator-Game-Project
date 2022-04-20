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
        //request = gameObject.GetComponent<RequestOwnership>();

        //assigns the Player GameObjects for photon
        /*if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            //this is all assignment information for Player1 (master) and Player2 (!master)
            GameObject PlayerFound = GameObject.FindGameObjectWithTag("Player");
            if (PhotonNetwork.IsMasterClient)
            {
                Player1 = PlayerFound;
            }
            else
            {
                Player2 = PlayerFound;
            }

            //get the distance between itselfs and the player it can access, then RPC's the other number
            if (PhotonNetwork.IsMasterClient)
            {
                Player1Distance = Vector3.Distance(transform.position, Player1.transform.position);
                view.RPC("RPC_InitialDistances", RpcTarget.Others, Player1Distance);
            }
            else
            {
                Player2Distance = Vector3.Distance(transform.position, Player2.transform.position);
                view.RPC("RPC_InitialDistances", RpcTarget.Others, Player2Distance);
            }

            if (Player1Distance > Player2Distance)
            {
                //if player2 is closer, then set player to be player2 on player2's view
                if (!PhotonNetwork.IsMasterClient)
                {
                    player = Player2;
                    request.MakeRequest();
                }
            }
            else
            {
                //if player1 is closer, then set player to be player1 on player1's view
                if (PhotonNetwork.IsMasterClient)
                {
                    player = Player1;
                    request.MakeRequest();
                }
            }
        }*/

        

        //if there are no players in scene then it will break
        //so use this if nothing as been assigned
        //however in online, sometimes if the player that shares the same view is not being followed,
        //its ok for it to be null, so this only applicable in LOCAL
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL && player == null)
        {
            //player = GameObject.FindGameObjectWithTag("Player");
        }

        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
          (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && PhotonNetwork.IsMasterClient))
        {
            RandomPoint();
        }
    }

    //sets the distances for the other view
    /*[PunRPC]
    public void RPC_InitialDistances(float Distance)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Player1Distance = Distance;
        }
        else
        {
            Player2Distance = Distance;
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
          (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && PhotonNetwork.IsMasterClient))
        {
            if (player != null)
            {
                Debug.LogError("Is view mine? " + player.GetPhotonView().IsMine);
                Chase();
                eyes.transform.LookAt(player.transform);
                eyes.transform.eulerAngles = new Vector3(0, eyes.transform.eulerAngles.y, 0);
            }
            else
            {
                Patrol();
                Debug.LogError("Patrolling");
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

        if (hit.distance < findTrigger.radius)
        {
            if (Vector3.Distance(new Vector3(hit.point.x, player.transform.position.y, hit.point.z), player.transform.position) < 1)
            {
                foundPlayer = true;
            }
        }

        Debug.LogError("foundPlayer == " + foundPlayer);
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
        Debug.LogError("isRunning == " + isRunning);

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
            player = _player ;
        } else if (!isRunning)
        {
            if (player == _player)
            {
                player = null;
            }
        } 
    }

    //useful for Photon and switching players
    private void OnTriggerStay(Collider other)
    {
        //not running after a player
        if (!isRunning)
        {
            //the gameobject as the player tag
            if (other.tag.Equals("PlayerCollider") || other.tag.Equals("Player"))
            {
   
                    //make this new player the one that will be chased
                    player = other.gameObject;
                    //view.RPC("RPC_SetPlayerToNull", RpcTarget.Others);
                    //request.MakeRequest();
                    Debug.Log("Is view mine? " + view.IsMine);

            }

            Debug.Log("Is Not Running");
        }

        Debug.Log("What");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isRunning)
        {
            player = null;
        }
    }
}