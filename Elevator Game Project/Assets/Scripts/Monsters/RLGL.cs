using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGL : Monster
{
    private List<GameObject> players;
    private List<GameObject> lights;
    [SerializeField] private float MaxTimeBeforePounce;

    public enum RLGLSTATE
    {
        DOCILE,
        ACTIVATED,
        DEACTIVATED
    }

    private RLGLSTATE CurrState = RLGLSTATE.DOCILE;
    

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        // Turn off all the lights in its vicinity during that time

        // Begin the red light green light and have it turn all the lights
        StartCoroutine(RLGLCycle());
    }

    private IEnumerator RLGLCycle()
    {
        while (CurrState == RLGLSTATE.ACTIVATED)
        {
            // maybe parameterize these range values?
            yield return new WaitForSeconds(Random.Range(3, 5));
            GreenLight();
            yield return new WaitForSeconds(Random.Range(6, 10));
            RedLight();
        }
    }

    private void RedLight()
    {

    }

    private void GreenLight()
    {

    }

    public override void SetPlayer(GameObject _player, bool b)
    {

    }

    public override void Kill()
    {

    }

    public void AggroMonster()
    {
        CurrState = RLGLSTATE.DEACTIVATED;
    }

    public void TryActivateMonster(GameObject player)
    {
        if(CurrState == RLGLSTATE.DEACTIVATED)
        {
            //Do other stuff
            CurrState = RLGLSTATE.ACTIVATED;
        }
        else
        {
            // Do nothing?
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
            players.Add(other.gameObject);
        }

        if (other.gameObject.GetComponent<Light1>())
        {
            lights.Add(other.gameObject);
        }
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

        if (other.gameObject.GetComponent<Light1>())
        {
            lights.Remove(other.gameObject);
        }
    }
}
