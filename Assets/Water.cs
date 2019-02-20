using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider o)
    {
        if (o.name == "Cube")
        {
            o.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    void OnTriggerExit(Collider o)
    {
        if (o.name == "Cube")
        {
            o.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
