using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cube_lv3 : MonoBehaviour
{

    public GameObject arrowPrefab;

    public bool ____________________________;

    public GameObject centerPoint;
    public Vector3 centerPos;

    // Sound stuff
    [FMODUnity.EventRef]
    public string drag;
    FMOD.Studio.EventInstance dragEvent;
    bool hasSupport;
    public float param;
    [FMODUnity.EventRef]
    public string win;
    [FMODUnity.EventRef]
    public string impact;
    [FMODUnity.EventRef]
    public string interact;

    // UI stuff
    static public Vector3 mouseDelta;
    public Vector3 m_pos;
    private bool applyForce = false;
    static public bool clear;

    public List<GameObject> arrows;
    static public bool drawForce = false;
    public Vector3 player_force;
    // work related
    static public float workDone;
    private Vector3 lastPos;

    // debug

    private void Awake()
    {
        Transform centerPointTrans = transform.Find("CenterPoint");
        centerPoint = centerPointTrans.gameObject;
        centerPoint.SetActive(false);
        arrowPrefab.transform.position = new Vector3(1000, 1000);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Star") // level clear
        {
            FMODUnity.RuntimeManager.PlayOneShot(win);
            Destroy(col.gameObject);
            clear = true;
            gameObject.SetActive(false);
            UI_lv3.ContinuousForce = new Vector3();
        }
        else FMODUnity.RuntimeManager.PlayOneShot(impact);

        if (col.gameObject.name == "glass" || col.gameObject.name == "metal") hasSupport = true;
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "glass" || col.gameObject.name == "metal") hasSupport = false;
    }

    private void OnMouseEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot(interact);
        centerPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        centerPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        drawForce = true;
        applyForce = false;
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 4; ++i) arrows.Add(Instantiate(arrowPrefab) as GameObject);
        lastPos = transform.position;
        applyForce = false;
        drawForce = false;
        arrows[0].transform.position = new Vector3(9999, 9999, 9999);
        workDone = 0;
        clear = false;
        dragEvent = FMODUnity.RuntimeManager.CreateInstance(drag);
        dragEvent.start();

        // start spinning
        applyForce = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // sound
        param = GetComponent<Rigidbody>().velocity.magnitude;
        if (param > 1) param = 1;
        if (!hasSupport) param = 0;
        dragEvent.setParameterValue("Speed", param * 0.8f);

        m_pos = Input.mousePosition;
        centerPos = centerPoint.transform.position;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        mouseDelta = mousePos3D - centerPos;
        if (!applyForce)
        {
            player_force = drawForce ? 2 * mouseDelta : new Vector3(0f, 0f, 0f);
            if (player_force.magnitude > UI_lv3.MAX_FORCE) player_force = player_force.normalized * UI_lv3.MAX_FORCE;
            if (drawForce) Utils.DrawForce(centerPos, arrows[0], player_force, Color.red); // drag out a force
            UI_lv3.ContinuousForce = player_force; // release mouse to apply the force
            if (Input.GetMouseButtonUp(0)) applyForce = true;
        }
        if (UI_lv3.ContinuousForce.magnitude != 0 && applyForce) // apply force to object
        {
            Utils.DrawForce(centerPos, arrows[0], UI_lv3.ContinuousForce, Color.red);
            if (Time.timeScale != 0) GetComponent<Rigidbody>().AddForce(UI_lv3.ContinuousForce, ForceMode.Force);
        }
        if (UI_lv3.ContinuousForce.magnitude == 0) arrows[0].transform.position = new Vector3(9999, 9999, 9999); // hide the arrow if no force

        // calcalate work done for con force
        Vector3 ds = transform.position - lastPos;
        if (UI_lv3.ContinuousForce.magnitude != 0 && ds.magnitude != 0)
            workDone += UI_lv3.ContinuousForce.magnitude * ds.magnitude * Utils.includedAngle2DCos(UI_lv3.ContinuousForce, ds);
        lastPos = transform.position;
    }

    private void OnDestroy()
    {
        dragEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
