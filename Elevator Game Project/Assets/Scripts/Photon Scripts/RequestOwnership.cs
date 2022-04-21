using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RequestOwnership : MonoBehaviourPun
{
    public void MakeRequest()
    {
        base.photonView.RequestOwnership();
    }
}
