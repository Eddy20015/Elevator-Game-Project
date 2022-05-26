using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vignette : MonoBehaviour
{
    [SerializeField] private GameObject[] vignette;
    [SerializeField] private GameObject player, monster;
    [SerializeField] private float farthestDistance, decreaseAmount;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        Debug.Log(Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)));
        if (Math.Abs(Vector3.Distance(player.transform.position,monster.transform.position)) < farthestDistance)
        {
            vignette[0].SetActive(true);
            vignette[1].SetActive(false);
        }
        else if(Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 1)
        {
            vignette[0].SetActive(false);
            vignette[1].SetActive(true);
            vignette[2].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 2)
        {
            vignette[1].SetActive(false);
            vignette[2].SetActive(true);
            vignette[3].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 3)
        {
            vignette[2].SetActive(false);
            vignette[3].SetActive(true);
            vignette[4].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 4)
        {
            vignette[3].SetActive(false);
            vignette[4].SetActive(true);
            vignette[5].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 5)
        {
            vignette[4].SetActive(false);
            vignette[5].SetActive(true);
            vignette[6].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 6)
        {
            vignette[5].SetActive(false);
            vignette[6].SetActive(true);
            vignette[7].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 7)
        {
            vignette[6].SetActive(false);
            vignette[7].SetActive(true);
            vignette[8].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 8)
        {
            vignette[7].SetActive(false);
            vignette[8].SetActive(true);
            vignette[9].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 9)
        {
            vignette[8].SetActive(false);
            vignette[9].SetActive(true);
            vignette[10].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 10)
        {
            vignette[9].SetActive(false);
            vignette[10].SetActive(true);
            vignette[11].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 11)
        {
            vignette[10].SetActive(false);
            vignette[11].SetActive(true);
            vignette[12].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 12)
        {
            vignette[11].SetActive(false);
            vignette[12].SetActive(true);
            vignette[13].SetActive(false);
        }
        else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) < farthestDistance - decreaseAmount * 13)
        {
            vignette[12].SetActive(false);
            vignette[13].SetActive(true);
            vignette[14].SetActive(false);
        }
    }
}
