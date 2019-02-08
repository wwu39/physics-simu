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
    static public ForceType forceType = ForceType.Continuous;
    static public Vector3 mouseDelta;
    public Vector3 m_pos;

    // 0 = player_force
    // 1 = gravity
    // 2 = support
    // 3 = friction
    // 4 = ???
    public List<GameObject> arrows;
    public bool drawForce = false;
    public bool hasSupport;
    public Vector3 player_force, G, N, f; // forces
    public bool drawOtherForces = false;
    // work related
    public float workDone;
    private Vector3 lastPos;

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
            SceneManager.LoadScene(0);
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
        if (forceType == ForceType.Impulse)
        {
            drawForce = true;
            arrows[0] = Instantiate(arrowPrefab) as GameObject;
            arrows[0].transform.position = centerPos;
            arrows[0].transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
            arrows[0].transform.GetChild(1).GetComponent<Renderer>().material.color = Color.red;
        }
    }

    // Use this for initialization
    void Start () {
        for (int i = 0; i < 4; ++i) arrows.Add(Instantiate(arrowPrefab) as GameObject);
        lastPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        m_pos = Input.mousePosition;
        centerPos = centerPoint.transform.position;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        mouseDelta = mousePos3D - centerPos;
        // draw impluse force
        if (forceType == ForceType.Impulse)
        {
            player_force = drawForce ? 10 * mouseDelta : new Vector3(0f, 0f, 0f);
            if (drawForce)
            {
                Utils.DrawForce(centerPos, arrows[0], player_force, Color.red);
            }
            if (Input.GetMouseButtonUp(0))
            {
                // when player releases mouse, add the force
                drawForce = false;
                GetComponent<Rigidbody>().AddForce(player_force);
                Destroy(arrows[0]);
                arrows[0] = null;
            }
        }
        // draw con force
        if (forceType == ForceType.Continuous)
        {
            if (UI.ContinuousForce.sqrMagnitude != 0)
            {
                if (arrows[0] == null) arrows[0] = Instantiate(arrowPrefab) as GameObject;
                Utils.DrawForce(centerPos, arrows[0], UI.ContinuousForce, Color.red);
                if (Time.timeScale != 0) GetComponent<Rigidbody>().AddForce(UI.ContinuousForce, ForceMode.Force);
            }
            else
            {
                Destroy(arrows[0]);
                arrows[0] = null;
            }
        }
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
    }
}
