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

public class UI : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string button_press;
    [FMODUnity.EventRef]
    public string cancel;
    [FMODUnity.EventRef]
    public string hintSound;

    static public Vector3 ContinuousForce;
    static public float MAX_FORCE = 10;
    static public float delta = 0; // in Radius
    static public float MAX_WORK = 225;
    static private bool showHint = true;

    public Button StartButton;
    public Button NextLevel2;
    public Button ToMenu;
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

        StartButton.onClick.AddListener(PlayAgianClick);
        NextLevel2.onClick.AddListener(NextLevelClick);
        ToMenu.onClick.AddListener(ToMenuClick);
        Time.timeScale = 0; // freeze the game

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
            Time.timeScale = 1;
        }
        else FMODUnity.RuntimeManager.PlayOneShot(hintSound);
        showHint = false;
    }

    void PlayAgianClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Scoring.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    void NextLevelClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        SceneManager.LoadScene(2);
    }

    void ToMenuClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        showHint = true;
        SceneManager.LoadScene(0);
    }

    void HintNextClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Hint.transform.localPosition = new Vector3(9999, 9999, 0);
        Hint2.transform.localPosition = new Vector3(0, 0, 0);
        FMODUnity.RuntimeManager.PlayOneShot(hintSound);
    }

    void HintOKClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Hint2.transform.localPosition = new Vector3(9999, 9999, 0);
        Time.timeScale = 1;
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

    void FixedUpdate()
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
            FMODUnity.RuntimeManager.PlayOneShot(cancel);
            ContinuousForceScrollbar.value = 0;
        }
        // scroll can change the size of the force too
        if (Input.mouseScrollDelta.y < 0) ContinuousForceScrollbar.value += 1f / ContinuousForceScrollbar.numberOfSteps;
        else if (Input.mouseScrollDelta.y > 0) ContinuousForceScrollbar.value -= 1f / ContinuousForceScrollbar.numberOfSteps;

        // direction keys control
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) CFAngle.text = "45";
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) CFAngle.text = "135";
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) CFAngle.text = "225";
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) CFAngle.text = "315";
        else if (Input.GetKey(KeyCode.W)) CFAngle.text = "90";
        else if (Input.GetKey(KeyCode.A)) CFAngle.text = "180";
        else if (Input.GetKey(KeyCode.D)) CFAngle.text = "1";
        else if (Input.GetKey(KeyCode.S)) CFAngle.text = "270";

        // Work Scrollbar
        WorkScrollbar.value = Cube.workDone / MAX_WORK;

        // goto menu
        if (Input.GetKeyDown(KeyCode.Escape)) ToMenuClick();
    }
}
