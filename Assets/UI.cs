// To use this example, attach this script to an empty GameObject.
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

public enum SBS { START, STOP }

public class UI : MonoBehaviour
{
    static public Vector3 ContinuousForce;
    static public SBS sb_status = SBS.START; //Done
    static public float MAX_FORCE = 10;
    static public float delta = 0; // in Radius
    static public float MAX_WORK = 225;
    static private bool showHint = true;

    public Button StartButton;
    public Scrollbar ContinuousForceScrollbar;
    public InputField CFAngle;
    public Scrollbar WorkScrollbar;

    public Button HintNext;
    public Button HintOK;
    public Image Hint;
    public Image Hint2;

    public Button PlayAgian;
    public Button NextLevel;
    public Image Scoring;
    public Text Rank;

    // debug
    public int angle;
    
    void Start()
    {
        // deselect any
        EventSystem.current.SetSelectedGameObject(null);

        StartButton.onClick.AddListener(StartButtonClick);
        Time.timeScale = 0; // freeze the game
        sb_status = SBS.START;

        HintNext.onClick.AddListener(HintNextClick);
        HintOK.onClick.AddListener(HintOKClick);
        Hint.transform.localPosition = new Vector3(0, 0, 0);
        Hint2.transform.localPosition = new Vector3(9999, 9999, 0);

        PlayAgian.onClick.AddListener(PlayAgianClick);
        NextLevel.onClick.AddListener(NextLevelClick);
        Scoring.transform.localPosition = new Vector3(9999, 9999, 0);

        ContinuousForceScrollbar.onValueChanged.AddListener(delegate { TaskOnValueChanged(); });
        WorkScrollbar.onValueChanged.AddListener(delegate { TaskOnWorkChanged(); });
        CFAngle.onValueChanged.AddListener(delegate { TaskAngleChanged(); });

        CFAngle.text = "0";

        if (!showHint)
        {
            Hint.transform.localPosition = new Vector3(9999, 9999, 0);
            Hint2.transform.localPosition = new Vector3(9999, 9999, 0);
            StartButtonClick();
        }
        showHint = false;
    }

    void PlayAgianClick()
    {
        SceneManager.LoadScene(0);
    }

    void NextLevelClick()
    {
        SceneManager.LoadScene(1);
    }

    void HintNextClick()
    {
        Hint.transform.localPosition = new Vector3(9999, 9999, 0);
        Hint2.transform.localPosition = new Vector3(0, 0, 0);
    }

    void HintOKClick()
    {
        Hint2.transform.localPosition = new Vector3(9999, 9999, 0);
        StartButtonClick();
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
        WorkScrollbar.value = Cube.workDone / MAX_WORK;
    }

    void Update()
    {
        if (Cube.clear)
        {
            if (Cube.workDone <= 75f)
            {
                Rank.text = "SS";
                Rank.color = new Color(243f / 255f, 240f / 255f, 10f / 255f);
            }
            else if (Cube.workDone <= 105)
            {
                Rank.text = "S";
                Rank.color = new Color(243f / 255f, 240f / 255f, 10f / 255f);
            }
            else if (Cube.workDone <= 135)
            {
                Rank.text = "A";
                Rank.color = new Color(243f / 255f, 161f / 255f, 255f / 255f);
            }
            else if (Cube.workDone <= 165)
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

        // pause & resume
        if (Input.GetKeyDown(KeyCode.Space)) StartButtonClick();

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
        if (Input.GetKeyDown(KeyCode.Mouse1)) ContinuousForceScrollbar.value = 0;
        // scroll can change the size of the force too
        if (Input.mouseScrollDelta.y < 0) ContinuousForceScrollbar.value += 1f / ContinuousForceScrollbar.numberOfSteps;
        else if (Input.mouseScrollDelta.y > 0) ContinuousForceScrollbar.value -= 1f / ContinuousForceScrollbar.numberOfSteps;

        // Work Scrollbar
        WorkScrollbar.value = Cube.workDone / MAX_WORK;
    }

    void StartButtonClick()
    {
        switch (sb_status)
        {
            case SBS.START:
                Time.timeScale = 1;
                StartButton.GetComponentInChildren<Text>().text = "Pause";
                sb_status = SBS.STOP;
                break;
            case SBS.STOP:
                Time.timeScale = 0;
                StartButton.GetComponentInChildren<Text>().text = "Start";
                sb_status = SBS.START;
                break;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
}
