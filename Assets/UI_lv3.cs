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

public class UI_lv3 : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string button_press;
    [FMODUnity.EventRef]
    public string cancel;
    [FMODUnity.EventRef]
    public string hintSound;
    [FMODUnity.EventRef]
    public string attached;

    public GameObject BlueStarGO;
    public static GameObject curBlueStarGO;
    public GameObject Crate;
    bool suckForce;
    public GameObject StarGO;

    static public Vector3 ContinuousForce;
    static public float MAX_FORCE = 10;
    static public float delta = 0; // in Radius
    static public float MAX_WORK = 225;

    public Button StartButton;
    public Button NextLevel2;
    public Button ToMenu;
    public Scrollbar ContinuousForceScrollbar;
    public InputField CFAngle;
    public Scrollbar WorkScrollbar;

    public Button PlayAgian;
    public Button NextLevel;
    public Image Scoring;
    public Text Rank;

    public Image Intro;
    static bool showedIntro = false;
    public Button IntroOK;

    // debug
    public int angle;

    void Start()
    {
        // deselect any
        EventSystem.current.SetSelectedGameObject(null);

        StartButton.onClick.AddListener(PlayAgianClick);
        NextLevel2.onClick.AddListener(NextLevelClick);
        ToMenu.onClick.AddListener(ToMenuClick);

        IntroOK.onClick.AddListener(IntroOKClick);

        PlayAgian.onClick.AddListener(PlayAgianClick);
        NextLevel.onClick.AddListener(NextLevelClick);
        Scoring.transform.localPosition = new Vector3(9999, 9999, 0);

        ContinuousForceScrollbar.onValueChanged.AddListener(delegate { TaskOnValueChanged(); });
        WorkScrollbar.onValueChanged.AddListener(delegate { TaskOnWorkChanged(); });
        CFAngle.onValueChanged.AddListener(delegate { TaskAngleChanged(); });

        CFAngle.text = "0";

        if (!showedIntro)
        {
            showedIntro = true;
            Time.timeScale = 0;
            FMODUnity.RuntimeManager.PlayOneShot(hintSound);
            Intro.gameObject.SetActive(true);
            Water_lv3.underwaterEvent.start();
        }
        else
        {
            Intro.gameObject.SetActive(false);
            Water_lv3.underwaterEvent.start();
        }

        // cube starts spinning
        curBlueStarGO = BlueStarGO;
        Crate.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 4, 0);
        suckForce = true;
        ContinuousForceScrollbar.value = 1f;

        // creat other blue stars
        Vector3 ds = (StarGO.transform.position - BlueStarGO.transform.position).normalized;
        float step = (Crate.transform.position - BlueStarGO.transform.position).magnitude;
        for (int i =0; i< 3; ++i)
        {
            GameObject newBlueStarGO = Instantiate(BlueStarGO) as GameObject;
            newBlueStarGO.transform.position = BlueStarGO.transform.position + ds * step * 2 * (i + 1);
        }
    }

    void IntroOKClick()
    {
        Intro.gameObject.SetActive(false);
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Time.timeScale = 1;
        Water_lv3.underwaterEvent.start();
    }

    void PlayAgianClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Scoring.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void NextLevelClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void ToMenuClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        showedIntro = false;
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
        WorkScrollbar.value = Cube_lv3.workDone / MAX_WORK;
    }

    void FixedUpdate()
    {
        if (Cube_lv3.clear)
        {
            if (Cube_lv3.workDone <= 10)
            {
                Rank.text = "SS";
                Rank.color = new Color(243f / 255f, 240f / 255f, 10f / 255f);
            }
            else if (Cube_lv3.workDone <= 64)
            {
                Rank.text = "S";
                Rank.color = new Color(243f / 255f, 240f / 255f, 10f / 255f);
            }
            else if (Cube_lv3.workDone <= 118)
            {
                Rank.text = "A";
                Rank.color = new Color(243f / 255f, 161f / 255f, 255f / 255f);
            }
            else if (Cube_lv3.workDone <= 172)
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

        if (BlueStar.IsMouseOver && Input.GetMouseButtonDown(0))
        {
            FMODUnity.RuntimeManager.PlayOneShot(attached);
            suckForce = true;
        }

        if (suckForce)
        {
            Vector3 force = curBlueStarGO.transform.position - Crate.transform.position;
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
            FMODUnity.RuntimeManager.PlayOneShot(cancel);
            ContinuousForceScrollbar.value = 0;
            suckForce = false;
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
        WorkScrollbar.value = Cube_lv3.workDone / MAX_WORK;

        // goto menu
        if (Input.GetKeyDown(KeyCode.Escape)) ToMenuClick();
    }
}
