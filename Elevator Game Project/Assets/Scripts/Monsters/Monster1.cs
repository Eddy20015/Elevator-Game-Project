using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class Monster1 : Monster, IPunObservable
{
    //Tim Kashani, Ed Slee

    [SerializeField] GameObject eyes;

    public float Player1Distance;
    public float Player2Distance;
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
        request = gameObject.GetComponent<RequestOwnership>();

        //assigns the Player GameObjects for photon
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
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
        }

        

        //if there are no players in scene then it will break
        //so use this if nothing as been assigned
        //however in online, sometimes if the player that shares the same view is not being followed,
        //its ok for it to be null, so this only applicable in LOCAL
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL && player == null)
        {
            //player = GameObject.FindGameObjectWithTag("Player");
        }

        RandomPoint();
    }

    //sets the distances for the other view
    [PunRPC]
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
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            Chase();
            eyes.transform.LookAt(player.transform);
            eyes.transform.eulerAngles = new Vector3(0, eyes.transform.eulerAngles.y, 0);
        } else
        {
            Patrol();
        }
    }

    public override void Chase()
    {
        //chase after player

        

        agent.SetDestination(player.transform.position);

        float f = Vector3.Distance(transform.position, player.transform.position);

        RaycastHit h;

        //Physics.SphereCast(transform.position, sphere1Radius, Vector3.zero, out h, 3);

        //Debug.Log(h.collider != null);

        //if (h.collider != null)
        //{
        //    player = h.collider.transform.parent.gameObject;
        //    Debug.Log("Found Player in Sphere Cast");
        //}

        Physics.Raycast(transform.position, eyes.transform.forward, out h);

        bool foundPlayer = false;

        if (h.distance < findTrigger.radius)
        {
            if (Vector3.Distance(new Vector3(h.point.x, player.transform.position.y, h.point.z), player.transform.position) < 1)
            {
                foundPlayer = true;
            }
        }

        if (f > findTrigger.radius || !foundPlayer)
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

    void Patrol()
    {
        if (Vector3.Distance(transform.position, points[currentPoint]) < 1)
        {
            RandomPoint();
        }
    }

    void RandomPoint()
    {
        currentPoint = (int)Random.Range(0, points.Length - 0.01f);
        agent.SetDestination(points[currentPoint]);
    }

    //useful for Photon and switching players
    private void OnTriggerStay(Collider other)
    {
        //not running after a player
        if (!isRunning)
        {
            //the gameobject as the player tag
            if (other.tag == "Player")
            {
                //the Player is not the one already being chased
                if (/*other.gameObject != player*/ !APlayerIsInRange)
                {
                    //make this new player the one that will be chased
                    player = other.gameObject;
                    view.RPC("RPC_SetPlayerToNull", RpcTarget.Others);
                    request.MakeRequest();
                    APlayerIsInRange = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isRunning)
        {
            player = null;
        }
    }

    //sets the other player's view to null
    //this way the monster won't try to follow the new player on this view 
    //and the old player on the old view 
    //this has no adverse effects if the "old" player == the new player, since it would be on the save view
    [PunRPC]
    private void RPC_SetPlayerToNull()
    {
        player = null;
    }
    
    //Essentially this is PhotonUpdate
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //get the distance of the players that each view has access to from the monster
        if (PhotonNetwork.IsMasterClient)
        {
            Player1Distance = Vector3.Distance(transform.position, Player1.transform.position);
        }
        else
        {
            Player2Distance = Vector3.Distance(transform.position, Player2.transform.position);
        }


        if (stream.IsWriting)
        {
            //first will be from Master or not, and second will be if the distance from either player1 or player2

            //Master will send the Player1Distance, as master is Player1
            if (PhotonNetwork.IsMasterClient)
            {
                
                stream.SendNext(Player1Distance);
            }
            //Not Master will send the Player2Distance, as not master is Player2
            else
            {
                stream.SendNext(Player2Distance);
            }
        }
        else
        {
            object Received = stream.ReceiveNext();
            if (Received is float)
            {
                float PlayerDistance = (float) Received;

                //we want Master to have the Distance from NotMaster's Player2
                if (PhotonNetwork.IsMasterClient)
                {
                    Player2Distance = PlayerDistance;
                }

                //we want NotMaster to have the Distance from Master's Player1
                else if (!PhotonNetwork.IsMasterClient)
                {
                    Player1Distance = PlayerDistance;
                }
            }
            else
            {
                Debug.LogError("BuddySystemReceived didn't received a float");
            }
        }

        if (Player1Distance > findTrigger.radius && Player2Distance > findTrigger.radius)
        {
            APlayerIsInRange = false;
        }
    }
}
