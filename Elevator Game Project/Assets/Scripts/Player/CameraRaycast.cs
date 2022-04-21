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

        //Physics.queriesHitTriggers = true;

        Physics.Raycast(transform.position, transform.forward, out h);

        if (h.collider)
        {
            if (h.collider.tag.Equals("Indicator"))
            {
                indicator = h.collider.GetComponent<Indicator>();
                indicator.Activate(true);
            } else
            {
                indicator.Activate(false);
            }
        } else
        {
            indicator.Activate(false);
        }
    }
}
