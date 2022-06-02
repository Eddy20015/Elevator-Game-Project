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
        //if()
    }

    private void Update()
    {
        //since it is two floors, we have to make sure that the players don't see the vignette when the monster is above or below them
        if (Mathf.Abs(monster.transform.position.y - player.transform.position.y) <= 7)
        {
            Debug.Log(Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)));
            if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 0);
                vignette[0].SetActive(false);
                vignette[1].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 1)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 1);
                vignette[0].SetActive(false);
                vignette[1].SetActive(true);
                vignette[2].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 2)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 2);
                vignette[1].SetActive(false);
                vignette[2].SetActive(true);
                vignette[3].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 3)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 3);
                vignette[2].SetActive(false);
                vignette[3].SetActive(true);
                vignette[4].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 4)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 4);
                vignette[3].SetActive(false);
                vignette[4].SetActive(true);
                vignette[5].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 5)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 5);
                vignette[4].SetActive(false);
                vignette[5].SetActive(true);
                vignette[6].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 6)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 6);
                vignette[5].SetActive(false);
                vignette[6].SetActive(true);
                vignette[7].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 7)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 7);
                vignette[6].SetActive(false);
                vignette[7].SetActive(true);
                vignette[8].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 8)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 8);
                vignette[7].SetActive(false);
                vignette[8].SetActive(true);
                vignette[9].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 9)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 9);
                vignette[8].SetActive(false);
                vignette[9].SetActive(true);
                vignette[10].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 10)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 10);
                vignette[9].SetActive(false);
                vignette[10].SetActive(true);
                vignette[11].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 11)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 11);
                vignette[10].SetActive(false);
                vignette[11].SetActive(true);
                vignette[12].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 12)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 12);
                vignette[11].SetActive(false);
                vignette[12].SetActive(true);
                vignette[13].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 13)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 13);
                vignette[12].SetActive(false);
                vignette[13].SetActive(true);
                vignette[14].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 14)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 14);
                vignette[13].SetActive(false);
                vignette[14].SetActive(true);
                vignette[15].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 15)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 15);
                vignette[14].SetActive(false);
                vignette[15].SetActive(true);
                vignette[16].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 16)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 16);
                vignette[15].SetActive(false);
                vignette[16].SetActive(true);
                vignette[17].SetActive(false);
            }
            else if (Math.Abs(Vector3.Distance(player.transform.position, monster.transform.position)) > farthestDistance - decreaseAmount * 17)
            {
                //gameObject.GetComponent<Animator>().SetInteger("Level", 17);
                vignette[16].SetActive(false);
                vignette[17].SetActive(true);
            }
        }
    }
}
