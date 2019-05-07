using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour {

    [FMODUnity.EventRef]
    public string interact;

    public GameObject centerpoint;
    public static bool IsMouseOver;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot(interact);
        centerpoint.SetActive(true);
        IsMouseOver = true;
    }

    private void OnMouseExit()
    {
        centerpoint.SetActive(false);
        IsMouseOver = false;
    }
}
