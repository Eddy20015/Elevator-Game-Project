using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    Indicator indicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit h;

        Physics.queriesHitTriggers = false;

        //Physics.Raycast(transform.position, transform.forward, out h, 100, 0, QueryTriggerInteraction.Ignore);

        Physics.Raycast(transform.position, transform.forward, out h);

        if (h.collider)
        {
            if (h.collider.tag.Equals("Indicator") && Vector3.Distance(transform.position, h.point) < 5)
            {
                indicator = h.collider.GetComponent<Indicator>();
                indicator.Activate(true);
            } else if (indicator)
            {
                indicator.Activate(false);
            }
        } else if (indicator)
        {
            indicator.Activate(false);
        }

        //Debug.Log(h.point);
    }
}
