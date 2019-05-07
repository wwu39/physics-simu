using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    public Button StartButton;
    public Button SelectLevelButton;
    public Button SolutionsButton;
    public Button ExitButton;
    public GameObject menu;
    public GameObject lvlist;
    public Button lvlistback;
    bool gotolv;
    public Button Level1;
    public Button Level2;
    public Button Level3;
    public Button Level4;
    public Toggle BGMCheckbox;
    
    public GameObject[] s = new GameObject[4];
    bool isVideoPlaying = false;
    int curVideo = -1;

    public Image[] Sol1Dialog = new Image[2];
    public Button[] Sol1Button = new Button[3];
    public Image Sol2Dialog;
    public Button[] Sol2Button = new Button[2];
    public Image Sol3Dialog;
    public Button[] Sol3Button = new Button[2];
    public Image Sol4Dialog;
    public Button[] Sol4Button = new Button[2];

    [FMODUnity.EventRef]
    public string button_press;
    [FMODUnity.EventRef]
    public string cancel;
    [FMODUnity.EventRef]
    public string hint;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(StartButtonClick);
        SelectLevelButton.onClick.AddListener(SelectLevelButtonClick);
        SolutionsButton.onClick.AddListener(SolutionsButtonClick);
        ExitButton.onClick.AddListener(ExitButtonClick);
        Level1.onClick.AddListener(Level1Click);
        Level2.onClick.AddListener(Level2Click);
        Level3.onClick.AddListener(Level3Click);
        Level4.onClick.AddListener(Level4Click);
        lvlistback.onClick.AddListener(LvlistBackClick);
        Time.timeScale = 1;

        // solution specific
        Sol1Button[0].onClick.AddListener(Sol1Back);
        Sol1Button[1].onClick.AddListener(Sol1Next);
        Sol1Button[2].onClick.AddListener(Sol1Video);

        Sol2Button[0].onClick.AddListener(Sol2Back);
        Sol2Button[1].onClick.AddListener(Sol2Video);

        Sol3Button[0].onClick.AddListener(Sol3Back);
        Sol3Button[1].onClick.AddListener(Sol3Video);

        Sol4Button[0].onClick.AddListener(Sol4Back);
        Sol4Button[1].onClick.AddListener(Sol4Video);
    }

    void Sol4Back()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Sol4Dialog.gameObject.SetActive(false);
        lvlist.SetActive(true);
    }

    void Sol4Video()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Sol4Dialog.gameObject.SetActive(false);
        videoClick(3);
    }

    void Sol3Back()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Sol3Dialog.gameObject.SetActive(false);
        lvlist.SetActive(true);
    }

    void Sol3Video()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Sol3Dialog.gameObject.SetActive(false);
        videoClick(2);
    }

        void Sol2Back()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Sol2Dialog.gameObject.SetActive(false);
        lvlist.SetActive(true);
    }

    void Sol2Video()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Sol2Dialog.gameObject.SetActive(false);
        videoClick(1);
    }

    void Sol1Back()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Sol1Dialog[0].gameObject.SetActive(false);
        Sol1Dialog[1].gameObject.SetActive(false);
        Sol1Button[0].gameObject.SetActive(false);
        lvlist.SetActive(true);
    }

    void Sol1Next()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Sol1Dialog[0].gameObject.SetActive(false);
        Sol1Dialog[1].gameObject.SetActive(true);
    }

    void Sol1Video()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Sol1Dialog[1].gameObject.SetActive(false);
        Sol1Button[0].gameObject.SetActive(false);
        videoClick(0);
    }

    void StartButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        SceneManager.LoadScene(1);
    }

    void SelectLevelButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        menu.SetActive(false);
        lvlist.SetActive(true);
        gotolv = true;
    }

    void SolutionsButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        menu.SetActive(false);
        lvlist.SetActive(true);
        gotolv = false;
    }

    void ExitButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        Application.Quit();
    }

    void LvlistBackClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        menu.SetActive(true);
        lvlist.SetActive(false);
    }

    void videoClick(int i)
    {
        s[i].SetActive(true);
        s[i].GetComponent<VideoPlayer>().Play();
        curVideo = i;
        isVideoPlaying = true;
        // lvlist.SetActive(false);
        BGMCheckbox.gameObject.SetActive(false);
    }

    void Level1Click()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        if (gotolv) SceneManager.LoadScene(1);
        else
        {
            lvlist.SetActive(false);
            Sol1Dialog[0].gameObject.SetActive(true);
            Sol1Button[0].gameObject.SetActive(true);
        }
    }

    void Level2Click()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        if (gotolv) SceneManager.LoadScene(2);
        else
        {
            lvlist.SetActive(false);
            Sol2Dialog.gameObject.SetActive(true);
        }
    }

    void Level3Click()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        if (gotolv) SceneManager.LoadScene(3);
        else
        {
            lvlist.SetActive(false);
            Sol3Dialog.gameObject.SetActive(true);
        }
    }

    void Level4Click()
    {
        FMODUnity.RuntimeManager.PlayOneShot(button_press);
        if (gotolv) SceneManager.LoadScene(4);
        else
        {
            lvlist.SetActive(false);
            Sol4Dialog.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isVideoPlaying)
        {
            if (s[curVideo].GetComponent<VideoPlayer>().isPlaying == false)
            {
                lvlist.SetActive(true);
                s[curVideo].SetActive(false);
                isVideoPlaying = false;
                curVideo = -1;
                BGMCheckbox.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FMODUnity.RuntimeManager.PlayOneShot(cancel);
                if(curVideo != -1) s[curVideo].GetComponent<VideoPlayer>().Stop();
            }
        }
    }
}
