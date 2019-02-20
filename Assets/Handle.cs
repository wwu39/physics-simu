using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour {

    public GameObject centerpoint;
    public GameObject Crate;

    private void OnMouseEnter()
    {
        centerpoint.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (Cube2.insideAccel)
        {
            Accelerator.launch = true;
            Crate.GetComponent<Rigidbody>().velocity = new Vector3(0f, -5f, 0f);
            GetComponent<Animation>().Play();
        }
    }

    private void OnMouseExit()
    {
        centerpoint.SetActive(false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
