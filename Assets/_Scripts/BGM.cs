using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    private static BGM instance = null;
    public static BGM Instance { get { return instance; } }
    static bool isBGMPlaying = true;

    public Toggle BGMCheckbox;

    [FMODUnity.EventRef]
    public string BGMEmiiter;
    FMOD.Studio.EventInstance BGMEmiiterEvent;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        BGMEmiiterEvent = FMODUnity.RuntimeManager.CreateInstance(BGMEmiiter);
        BGMEmiiterEvent.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BGMON()
    {
        if (BGMCheckbox.isOn)
        {
            isBGMPlaying = true;
            BGMEmiiterEvent.start();
        }
        else
        {

            isBGMPlaying = false;
            BGMEmiiterEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        BGMCheckbox = UI.FindObjectOfType<Toggle>();
        if (BGMCheckbox != null)
        {
            if (isBGMPlaying) BGMCheckbox.isOn = true;
            else BGMCheckbox.isOn = false;
            BGMCheckbox.onValueChanged.AddListener(delegate { BGMON(); });
        }
    }
}
