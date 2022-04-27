using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light1 : MonoBehaviour
{
    //Tim Kashani

    [SerializeField] GameObject l;

    [SerializeField] GameObject pointLight;

    //temporary if I think of a better solution
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 100)
        {
            pointLight.SetActive(false);
        } else
        {
            pointLight.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Monster")
        {
            l.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            StartCoroutine(TurnLightOn());
        }
    }

    IEnumerator TurnLightOn()
    {
        yield return new WaitForSeconds(3);
        l.SetActive(true);
    }
}
