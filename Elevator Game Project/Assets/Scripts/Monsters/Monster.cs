using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    //Tim Kashani

    [SerializeField] protected float speed;

    [SerializeField] protected PlayerScript player;

    protected Collider monsterCollider;

    protected NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    public virtual void Chase()
    {

    }

    public void KillPlayer()
    {
        player.GetKilled();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            KillPlayer();
        }
    }
}
