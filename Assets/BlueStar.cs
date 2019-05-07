using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueStar : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string interact;

    public GameObject centerpoint;
    public static bool IsMouseOver;

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

    private void OnMouseDown()
    {
        UI_lv3.curBlueStarGO = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
