using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeadSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private string HeadName;
    [SerializeField] private Vector3 MasterLocation;
    [SerializeField] private Vector3 FollowLocation;
    private GameObject[] HeadMonsters;
    private bool Found;

    GameObject LocalHead;
    GameObject OnlineHead;

    // Start is called before the first frame update
    void Start()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            Destroy(gameObject);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(HeadName, MasterLocation, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(HeadName, FollowLocation, Quaternion.identity);
            }
        }

        HeadMonsters = new GameObject[3];
    }

    // Update is called once per frame
    void Update()
    {
        if (!Found)
        {
            HeadMonsters = GameObject.FindGameObjectsWithTag("Monster");
        }
        if(HeadMonsters.Length == 3 && HeadMonsters[0] != null && HeadMonsters[1] != null && HeadMonsters[2] != null && !Found)
        {
            Found = true;
            foreach(GameObject monster in HeadMonsters)
            {
                PhotonView MonsterView = monster.GetComponent<PhotonView>();

                //this will identify which is the monster that is local and not spawned in
                if(MonsterView == null)
                {
                    LocalHead = monster;
                }

                //this disables the monster that doesn't follow this view's player
                else if (MonsterView.IsMine == false)
                {
                    //monster.SetActive(false);
                    Destroy(monster);
                }

                //saves the monster that has the player's correct photon view
                else
                {
                    OnlineHead = monster;
                }
            }

            //sets the points of the local head to the online head
            //OnlineHead.GetComponent<HeadAI>().SetPatrolPoints(LocalHead.GetComponent<HeadAI>().GetPatrolPoints());
            HeadAI LocalHeadAI = LocalHead.GetComponent<HeadAI>();
            HeadAI OnlineHeadAI = OnlineHead.GetComponent<HeadAI>();
            GameObject PointParent = GameObject.Find("Patrol Points");
            OnlineHeadAI.patrolPoints = new GameObject[PointParent.transform.childCount];

            for (int i = 0; i < LocalHeadAI.patrolPoints.Length; i++)
            {
                OnlineHeadAI.patrolPoints[i] = PointParent.transform.GetChild(i).gameObject;
                Debug.LogWarning("Local Head AI Points " + LocalHeadAI.patrolPoints[i]);
                Debug.LogWarning("Patrol Point Directly " + PointParent.transform.GetChild(i).gameObject);
            }
            LocalHead.SetActive(false);
            //Destroy(LocalHead);
        }
    }
}
