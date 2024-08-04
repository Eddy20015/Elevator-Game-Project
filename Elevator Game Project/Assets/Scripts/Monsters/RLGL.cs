using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RLGL : Monster
{
    private List<GameObject> players;
    //private List<GameObject> lights;
    [SerializeField] private float MaxTimeBeforePounce;
    [SerializeField] private float MinTimeOfLight;
    [SerializeField] private float MaxTimeOfLight;
    [SerializeField] private float LookAtThreshhold;
    [SerializeField] private float MoveResponseTime;
    [SerializeField] private float LookResponseTime;
    private float MoveTimeSinceSwitch = 0.0f;
    private float LookTimeSinceSwitch = 0.0f;

    public enum RLGLSTATE
    {
        DOCILE,
        ACTIVATED,
        DEACTIVATED
    }

    public enum ACTIVELIGHT
    {
        RED,
        GREEN,
        NONE
    }

    private RLGLSTATE CurrState = RLGLSTATE.DOCILE;
    private ACTIVELIGHT CurrLight = ACTIVELIGHT.NONE;
   
    private ChargingStationManager CSM;
    private GameObject LocalPlayer = null;
    private Vector3 LocalPlayerPrevPosition;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();
        CSM = ChargingStationManager.chargingStationManager;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrState == RLGLSTATE.ACTIVATED)
        {
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
            {
                LocalPlayer = players[0];
            }
            else
            {
                // might be wrong need to test
                if(players[0].GetComponent<PhotonView>().IsMine)
                {
                    LocalPlayer = players[0];
                }
                else if(players.Count == 2)
                {
                    LocalPlayer = players[1];
                }
            }

            // no reason to continue down the line if the player isn't real
            if (LocalPlayer == null)
            {
                return;
            }

            //Check if the player is looking at the monster
            if (CurrLight != ACTIVELIGHT.NONE)
            {
                LookTimeSinceSwitch += Time.deltaTime;
                if (LookTimeSinceSwitch > LookResponseTime)
                {
                    Vector3 PlayerForward = LocalPlayer.transform.forward;
                    Vector3 BetweenVector = LocalPlayer.transform.position - transform.position;
                    if (Vector3.Dot(PlayerForward, BetweenVector) < LookAtThreshhold)
                    {
                        players.Remove(LocalPlayer);
                        LocalPlayer.GetComponent<PlayerScript>().GetKilled();
                    }
                }
            }

            //Check if the player is still
            if (CurrLight == ACTIVELIGHT.RED)
            {
                MoveTimeSinceSwitch += Time.deltaTime;
                if (MoveTimeSinceSwitch > MoveResponseTime)
                {
                    if (LocalPlayer.transform.position != LocalPlayerPrevPosition)
                    {
                        players.Remove(LocalPlayer);
                        LocalPlayer.GetComponent<PlayerScript>().GetKilled();
                    }
                }
            }
            //Check if the player is moving
            else if(CurrLight == ACTIVELIGHT.GREEN)
            {
                MoveTimeSinceSwitch += Time.deltaTime;
                if (MoveTimeSinceSwitch > MoveResponseTime)
                {
                    if (LocalPlayer.transform.position == LocalPlayerPrevPosition)
                    {
                        players.Remove(LocalPlayer);
                        LocalPlayer.GetComponent<PlayerScript>().GetKilled();
                    }
                }
            }

            LocalPlayerPrevPosition = LocalPlayer.transform.position;
        }
        else
        {
            LocalPlayer = null;
        }
    }

    public RLGLSTATE GetCurrState()
    {
        return CurrState;
    }

    public override void Chase()
    {
        if(CurrState != RLGLSTATE.DEACTIVATED)
        {
            return;
        }
        else
        {
            CurrState = RLGLSTATE.ACTIVATED;
            StartCoroutine(WaitToPounce());
        }
    }

    private IEnumerator WaitToPounce()
    {
        float TimeToWait = Random.Range(0, MaxTimeBeforePounce);
        yield return new WaitForSeconds(TimeToWait);
        RedLightGreenLight();
    }

    private void RedLightGreenLight()
    {
        //Provide some time between when the player sees the monster and when it begins
        // Begin the red light green light and have it turn all the lights
        float NumOfLights = CSM.NumOfCompletedStations / 2 + 1;
        StartCoroutine(RLGLCycle(NumOfLights));
    }

    private IEnumerator RLGLCycle(float NumOfLights)
    {
        //perhaps an animation? idk
        // Make it visible
        yield return new WaitForSeconds(LookResponseTime);

        float LightColorDeterminer = 0.5f;
        float LightColorModifier = 0.125f;
        while (0 < NumOfLights--)
        {
            // The more of one color appears, the less likely it will continue to appear
            if( LightColorDeterminer >= Random.Range(0, 1))
            {
                LightColorDeterminer -= LightColorModifier;
                GreenLight();
            }
            else
            {
                LightColorDeterminer += LightColorModifier;
                RedLight();
            }

            yield return new WaitForSeconds(Random.Range(MinTimeOfLight, MaxTimeOfLight));
        }
        TurnOffLight();
    }

    private void RedLight()
    {
        MoveTimeSinceSwitch = 0;
        CurrLight = ACTIVELIGHT.RED;
    }

    private void GreenLight()
    {
        MoveTimeSinceSwitch = 0;
        CurrLight = ACTIVELIGHT.GREEN;
    }

    private void TurnOffLight()
    {
        LookTimeSinceSwitch = 0;
        CurrLight = ACTIVELIGHT.NONE;
    }

    public void AggroMonster()
    {
        CurrState = RLGLSTATE.DEACTIVATED;
    }

    public void TryActivateMonster(GameObject player)
    {
        if(CurrState == RLGLSTATE.DEACTIVATED)
        {
            Chase();
        }
    }

    public void DeativateMonster()
    {
        CurrState = RLGLSTATE.DEACTIVATED;
        players.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player")
        {
            return; 
        }

        if(players.Count == 0 || (players.Count == 1 && other.gameObject != players[0]))
        {
            if(PerformListAddRaycast(other))
                players.Add(other.gameObject);
        }

        /*if (other.gameObject.GetComponent<Light1>())
        {
            if(PerformListAddRaycast(other))
                lights.Add(other.gameObject);
        }*/
    }

    private bool PerformListAddRaycast(Collider other)
    {
        RaycastHit Hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(other.transform.position), out Hit, Mathf.Infinity))
        {
            if (other.gameObject.CompareTag(Hit.collider.gameObject.tag))
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(players.Count == 0 || other.gameObject.tag != "Player")
        {
            return;
        }
        else
        {
            players.Remove(other.gameObject);
        }

        /*if (other.gameObject.GetComponent<Light1>())
        {
            lights.Remove(other.gameObject);
        }*/
    }
}
