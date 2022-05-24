using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    MeshRenderer m;

    ChargeStation chargeStation;

    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<MeshRenderer>();
        m.enabled = false;

        chargeStation = GetComponentInParent<ChargeStation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m.enabled && chargeStation != null)
        {
            chargeStation.Interact();
        }
    }

    public void Activate(bool b)
    {
        m.enabled = b;
        //Debug.Log(b);

        if (chargeStation != null && !b)
        {
            chargeStation.LookAway();
        }
    }
}
