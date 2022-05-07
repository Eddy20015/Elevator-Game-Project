using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Monster : MonoBehaviourPunCallbacks
{
    //Tim Kashani

    [SerializeField] protected float speed;

    protected GameObject player;

    //used for online players
    //protected GameObject Player1;
    //protected GameObject Player2;
    //protected bool APlayerIsInRange;    //will be used to determine if either player is in range,
                                        //as if no one is, then the next player to walk in the range is the new target

    protected Collider monsterCollider;
    protected SphereCollider findTrigger;

    protected NavMeshAgent agent;

    protected bool isRunning;

    protected PhotonView view;
    protected RequestOwnership request;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterCollider = GetComponent<Collider>();
    }

    public virtual void Chase()
    {

    }

    public virtual void SetPlayer(GameObject _player, bool b)
    {

    }

    public virtual void Kill()
    {

    }

    /*public void KillPlayer()
    {
        player.GetKilled();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            KillPlayer();
        }
    }*/
}
