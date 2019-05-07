using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Limbo : MonoBehaviour
{
    bool playerLost = false;
    [FMODUnity.EventRef]
    public string playerDies;
    FMOD.Studio.EventInstance playerDiesEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Cube")
        {
            playerLost = true;
            playerDiesEvent.start();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerDiesEvent = FMODUnity.RuntimeManager.CreateInstance(playerDies);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLost)
        {
            FMOD.Studio.PLAYBACK_STATE pdepbs;
            playerDiesEvent.getPlaybackState(out pdepbs);
            if (pdepbs != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
