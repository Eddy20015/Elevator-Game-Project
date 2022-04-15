using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackwards : MonoBehaviour
{
    public void Interact()
    {
        gameObject.transform.Translate(Vector3.left);
    }
}
