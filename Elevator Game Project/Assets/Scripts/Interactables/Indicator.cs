using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    MeshRenderer m;

    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<MeshRenderer>();
        m.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(bool b)
    {
        m.enabled = b;
        //Debug.Log(b);
    }
}
