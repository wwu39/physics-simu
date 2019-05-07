using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerator : MonoBehaviour {

    [FMODUnity.EventRef]
    public string catchCrate;

    static public bool launch;

	// Use this for initialization
	void Start () {
        launch = false;
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Cube")
        {
            FMODUnity.RuntimeManager.PlayOneShot(catchCrate);
            other.GetComponent<Rigidbody>().velocity = new Vector3();
            other.transform.position = transform.position;
            Cube2.insideAccel = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Cube")
        {
            if (!launch)
            {
                other.GetComponent<Rigidbody>().velocity = new Vector3();
                other.transform.position = transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Cube")
        {
            Cube2.insideAccel = false;
            launch = false;
        }
    }
}
