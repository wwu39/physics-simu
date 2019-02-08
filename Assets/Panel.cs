using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Panel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update () {

    }

    public static bool isMouseOver()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
