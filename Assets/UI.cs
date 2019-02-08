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

public enum SBS { START, STOP }

public class UI : MonoBehaviour
{
    static public Vector3 ContinuousForce;
    static public SBS sb_status = SBS.START; //Done
    static float MAX_FORCE = 10;
    static public float delta = 0; // in Radius

    public Button StartButton;
    public Image tick;
    public Scrollbar ContinuousForceScrollbar;
    public InputField CFAngle;

    // debug
    public int angle;
    
    void Start()
    {
        // deselect any
        EventSystem.current.SetSelectedGameObject(null);

        StartButton.onClick.AddListener(StartButtonClick);
        Time.timeScale = 1; // freeze the game
        // tick start at cforce
        tick.GetComponent<RectTransform>().localPosition = new Vector3(375f, 158f, 0f);

        ContinuousForceScrollbar.onValueChanged.AddListener(delegate { TaskOnValueChanged(); });
        CFAngle.onValueChanged.AddListener(delegate { TaskAngleChanged(); });

        CFAngle.text = "0";
    }

    private void TaskAngleChanged()
    {
        float angleInDegree = float.Parse(CFAngle.text);
        delta = angleInDegree / 360f * 2 * Mathf.PI;
        var size = ContinuousForceScrollbar.value * MAX_FORCE;
        ContinuousForce = new Vector3(size * Mathf.Cos(delta), size * Mathf.Sin(delta), 0);
    }

    void Update()
    {
        // pause & resume
        if (Input.GetKeyDown(KeyCode.Space)) StartButtonClick();

        // use Q & E to switch forces
        if (Input.GetKeyDown(KeyCode.Q))
        {
            tick.GetComponent<RectTransform>().localPosition = new Vector3(375f, 158f, 0f);
            Cube.forceType = ForceType.Continuous;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            tick.GetComponent<RectTransform>().localPosition = new Vector3(375f, 72.2f, 0f);
            Cube.forceType = ForceType.Impulse;
        }

        // mouse change force direction
        if (Input.GetKeyDown(KeyCode.Mouse0) && !Panel.isMouseOver())
        {
            // change the force's direction
            var size = ContinuousForceScrollbar.value * MAX_FORCE;
            ContinuousForce = Cube.mouseDelta.normalized * size;
            // update the input field
            delta = Mathf.Acos(ContinuousForce.x/Mathf.Sqrt(ContinuousForce.sqrMagnitude));
            if (ContinuousForce.y > 0) angle = Mathf.FloorToInt(delta * Mathf.Rad2Deg);
            else if (ContinuousForce.y < 0) angle = 360 - Mathf.FloorToInt(delta * Mathf.Rad2Deg);
            CFAngle.text = angle.ToString();
        }

        // right click 0 out con force
        if (Input.GetKeyDown(KeyCode.Mouse1)) ContinuousForceScrollbar.value = 0;
        // scroll can change the size of the force too
        if (Input.mouseScrollDelta.y < 0) ContinuousForceScrollbar.value += 1f / ContinuousForceScrollbar.numberOfSteps;
        else if (Input.mouseScrollDelta.y > 0) ContinuousForceScrollbar.value -= 1f / ContinuousForceScrollbar.numberOfSteps;
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


    void TaskOnValueChanged()
    {
        float angleInDegree = float.Parse(CFAngle.text);
        delta = angleInDegree / 360f * 2 * Mathf.PI;
        var size = ContinuousForceScrollbar.value * MAX_FORCE;
        ContinuousForce = new Vector3(size * Mathf.Cos(delta), size * Mathf.Sin(delta), 0);
    }
}
