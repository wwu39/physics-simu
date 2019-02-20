﻿// To use this example, attach this script to an empty GameObject.
// Create three buttons (Create>UI>Button). Next, select your
// empty GameObject in the Hierarchy and click and drag each of your
// Buttons from the Hierarchy to the Your First Button, Your Second Button
// and Your Third Button fields in the Inspector.
// Click each Button in Play Mode to output their message to the console.
// Note that click means press down and then release.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class UI2 : MonoBehaviour
{
    static public Vector3 ContinuousForce;
    static public SBS sb_status = SBS.START; //Done
    static public float MAX_FORCE = 10;
    static public float delta = 0; // in Radius
    static public float MAX_WORK = 100;

    public Button StartButton;
    public Scrollbar ContinuousForceScrollbar;
    public InputField CFAngle;
    public Scrollbar WorkScrollbar;

    public Button PlayAgian;
    public Button NextLevel;
    public Image Scoring;
    public Text Rank;

    public GameObject sticky;
    public GameObject Crate;
    bool suckForce;
    static bool showHint = true;

    // intro stuff
    public Image intro1;
    public Image intro2;
    public Button next;
    public Button ok;
    public Image Restarthint;
    public Button ok2;
    static bool showRSHint = true;

    // debug
    public int angle;

    void Start()
    {
        // deselect any
        EventSystem.current.SetSelectedGameObject(null);

        StartButton.onClick.AddListener(StartButtonClick);

        PlayAgian.onClick.AddListener(PlayAgianClick);
        NextLevel.onClick.AddListener(NextLevelClick);
        Scoring.transform.localPosition = new Vector3(9999, 9999, 0);

        ContinuousForceScrollbar.onValueChanged.AddListener(delegate { TaskOnValueChanged(); });
        WorkScrollbar.onValueChanged.AddListener(delegate { TaskOnWorkChanged(); });
        CFAngle.onValueChanged.AddListener(delegate { TaskAngleChanged(); });

        CFAngle.text = "0";
        suckForce = false;

        next.onClick.AddListener(ToIntro2);
        ok.onClick.AddListener(StartGame);
        ok2.onClick.AddListener(RestartGame);
        Restarthint.transform.localPosition = new Vector3(9999, 9999, 0);

        if (showHint) introduceNewStuff();
        else
        {
            intro1.transform.localPosition = new Vector3(9999, 9999, 0);
            intro2.transform.localPosition = new Vector3(9999, 9999, 0);

        }
    }

    void introduceNewStuff()
    {
        Time.timeScale = 0;
        intro1.transform.localPosition = new Vector3(0, 0, 0);
        intro2.transform.localPosition = new Vector3(9999, 9999, 0);
        showHint = false;
    }
    void ToIntro2() {
        intro1.transform.localPosition = new Vector3(9999, 9999, 0);
        intro2.transform.localPosition = new Vector3(0, 0, 0);
    }
    void StartGame()
    {

        intro2.transform.localPosition = new Vector3(9999, 9999, 0);
        Time.timeScale = 1;
    }

    void PlayAgianClick()
    {
        SceneManager.LoadScene(1);
    }

    void NextLevelClick()
    {
        SceneManager.LoadScene(0);
    }

    private void TaskAngleChanged()
    {
        float angleInDegree = float.Parse(CFAngle.text);
        delta = angleInDegree / 360f * 2 * Mathf.PI;
        var size = ContinuousForceScrollbar.value * MAX_FORCE;
        ContinuousForce = new Vector3(size * Mathf.Cos(delta), size * Mathf.Sin(delta), 0);
    }

    void TaskOnValueChanged()
    {
        float angleInDegree = float.Parse(CFAngle.text);
        delta = angleInDegree / 360f * 2 * Mathf.PI;
        var size = ContinuousForceScrollbar.value * MAX_FORCE;
        ContinuousForce = new Vector3(size * Mathf.Cos(delta), size * Mathf.Sin(delta), 0);
    }

    void TaskOnWorkChanged()
    {
        WorkScrollbar.value = Cube2.workDone / MAX_WORK;
    }

    void Update()
    {
        if (Cube2.clear)
        {
            if (Cube2.workDone <= 10f)
            {
                Rank.text = "SS";
                Rank.color = new Color(243f / 255f, 240f / 255f, 10f / 255f);
            }
            else if (Cube2.workDone <= 30f)
            {
                Rank.text = "S";
                Rank.color = new Color(243f / 255f, 240f / 255f, 10f / 255f);
            }
            else if (Cube2.workDone <= 50f)
            {
                Rank.text = "A";
                Rank.color = new Color(243f / 255f, 161f / 255f, 255f / 255f);
            }
            else if (Cube2.workDone <= 70f)
            {
                Rank.text = "B";
                Rank.color = new Color(105f / 255f, 215f / 255f, 255f / 255f);
            }
            else
            {
                Rank.text = "C";
                Rank.color = new Color(158f / 255f, 235f / 255f, 93f / 255f);
            }
            Scoring.transform.localPosition = new Vector3(0, 0, 0);
            return;
        }

        if (Sticky.IsMouseOver && Input.GetMouseButtonDown(0))
        {
            suckForce = true;
        }

        if (suckForce)
        {
            Vector3 force = sticky.transform.position - Crate.transform.position;
            delta = Mathf.Acos(force.x / force.magnitude);
            if (force.y > 0) angle = Mathf.FloorToInt(delta * Mathf.Rad2Deg);
            else if (force.y < 0) angle = 360 - Mathf.FloorToInt(delta * Mathf.Rad2Deg);
            CFAngle.text = angle.ToString();
        }

        // update angle input field
        if (ContinuousForce.magnitude != 0)
        {
            delta = Mathf.Acos(ContinuousForce.x / ContinuousForce.magnitude);
            if (ContinuousForce.y > 0) angle = Mathf.FloorToInt(delta * Mathf.Rad2Deg);
            else if (ContinuousForce.y < 0) angle = 360 - Mathf.FloorToInt(delta * Mathf.Rad2Deg);
            CFAngle.text = angle.ToString();
        }
        // update force scrollbar
        ContinuousForceScrollbar.value = ContinuousForce.magnitude > MAX_FORCE ? 1f : ContinuousForce.magnitude / MAX_FORCE;

        // right click 0 out con force
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ContinuousForceScrollbar.value = 0;
            suckForce = false;
        }
        // scroll can change the size of the force too
        if (Input.mouseScrollDelta.y < 0) ContinuousForceScrollbar.value += 1f / ContinuousForceScrollbar.numberOfSteps;
        else if (Input.mouseScrollDelta.y > 0) ContinuousForceScrollbar.value -= 1f / ContinuousForceScrollbar.numberOfSteps;

        // Work Scrollbar
        WorkScrollbar.value = Cube2.workDone / MAX_WORK;
    }

    void StartButtonClick()
    {
        if (showRSHint)
        {
            Time.timeScale = 0;
            Restarthint.transform.localPosition = new Vector3();
            showRSHint = false;
        }
        else SceneManager.LoadScene(1);
        EventSystem.current.SetSelectedGameObject(null);
    }
    void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        EventSystem.current.SetSelectedGameObject(null);
    }
}