using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ForceType { Continuous, Impulse }

public class Cube : MonoBehaviour {
    
    public GameObject arrowPrefab;

    public bool ____________________________;

    public GameObject centerPoint;
    public Vector3 centerPos;

    // UI stuff
    static public Vector3 mouseDelta;
    public Vector3 m_pos;
    private bool applyForce = false;
    static public bool clear;

    // 0 = player_force
    // 1 = gravity
    // 2 = support
    // 3 = friction
    // 4 = ???
    public List<GameObject> arrows;
    static public bool drawForce = false;
    public bool hasSupport;
    public Vector3 player_force, G, N, f; // forces
    public bool drawOtherForces = false;
    // work related
    static public float workDone;
    private Vector3 lastPos;

    // debug

    private void Awake()
    {
        Transform centerPointTrans = transform.Find("CenterPoint");
        centerPoint = centerPointTrans.gameObject;
        centerPoint.SetActive(false);
        G = Physics.gravity * GetComponent<Rigidbody>().mass;
        arrowPrefab.transform.position = new Vector3(1000, 1000);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Floor") hasSupport = true;

        if (col.gameObject.name == "Star") // level clear
        {
            clear = true;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "Floor") hasSupport = false;
    }

    private void OnMouseEnter()
    {
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
    void Start () {
        for (int i = 0; i < 4; ++i) arrows.Add(Instantiate(arrowPrefab) as GameObject);
        lastPos = transform.position;
        applyForce = false;
        drawForce = false;
        arrows[0].transform.position = new Vector3(9999, 9999, 9999);
        workDone = 0;
        clear = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_pos = Input.mousePosition;
        centerPos = centerPoint.transform.position;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        mouseDelta = mousePos3D - centerPos;
        if (!applyForce)
        {
            player_force = drawForce ? 2 * mouseDelta : new Vector3(0f, 0f, 0f);
            if (player_force.magnitude > UI.MAX_FORCE) player_force = player_force.normalized * UI.MAX_FORCE;
            if (drawForce) Utils.DrawForce(centerPos, arrows[0], player_force, Color.red); // drag out a force
            UI.ContinuousForce = player_force; // release mouse to apply the force
            if (Input.GetMouseButtonUp(0)) applyForce = true;
        }
        if (UI.ContinuousForce.magnitude != 0 && applyForce) // apply force to object
        {
            Utils.DrawForce(centerPos, arrows[0], UI.ContinuousForce, Color.red);
            if (Time.timeScale != 0) GetComponent<Rigidbody>().AddForce(UI.ContinuousForce, ForceMode.Force);
        }
        if (UI.ContinuousForce.magnitude == 0) arrows[0].transform.position = new Vector3(9999, 9999, 9999); // hide the arrow if no force
        if (drawOtherForces)
        {
            // drawing gravity
            if (GetComponent<Rigidbody>().useGravity)
            {
                if (arrows[1] == null) arrows[1] = Instantiate(arrowPrefab) as GameObject;
                if (arrows[1] != null) Utils.DrawForce(centerPos, arrows[1], G, new Color(55, 0, 55));
            }
            else
            {
                Destroy(arrows[1]);
                arrows[1] = null;
            }

            // draw support force
            if (hasSupport)
            {
                // direction
                var sf_dir = transform.rotation; // rotation of the box, pointed downwards
                                                 // size
                float size = -G.y * Mathf.Cos(sf_dir.eulerAngles.z);
                if (arrows[2] == null) arrows[2] = Instantiate(arrowPrefab) as GameObject;
                if (arrows[2] != null) Utils.DrawForce(centerPos, arrows[2], sf_dir, size, new Color(0, 0, 55));
            }
            else
            {
                Destroy(arrows[2]);
                arrows[2] = null;
            }

            // draw friction
            if (hasSupport)
            {
                // direction
                var ff_dir = transform.rotation;
                ff_dir = Quaternion.Euler(new Vector3(0, 0, -90));
                // size
                float size = -G.y * Mathf.Sin(ff_dir.eulerAngles.z);
                if (arrows[3] == null) arrows[3] = Instantiate(arrowPrefab) as GameObject;
                if (arrows[3] != null) Utils.DrawForce(centerPos, arrows[3], ff_dir, size, new Color(0, 55, 0));
            }
            else
            {
                Destroy(arrows[3]);
                arrows[3] = null;
            }
        }

        // calcalate work done for con force
        Vector3 ds = transform.position - lastPos;
        if (UI.ContinuousForce.magnitude != 0 && ds.magnitude != 0)
            workDone += UI.ContinuousForce.magnitude * ds.magnitude * Utils.includedAngle2DCos(UI.ContinuousForce, ds);
        lastPos = transform.position;
    }
}
