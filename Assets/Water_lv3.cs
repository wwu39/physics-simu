using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_lv3 : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string splash;
    [FMODUnity.EventRef]
    public string underwater;
    public static FMOD.Studio.EventInstance underwaterEvent;
    public ParticleSystem waterSplashSys;
    public List<ParticleSystem> pslist;
    public GameObject centerPoint;
    bool enter = true;

    // Use this for initialization
    void Start()
    {
        underwaterEvent = FMODUnity.RuntimeManager.CreateInstance(underwater);
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = pslist.Count - 1; i != -1; --i)
        {
            if (pslist[i].particleCount > 100)
            {
                pslist[i].Stop();
            }

            if (pslist[i].particleCount == 0)
            {
                Destroy(pslist[i]);
                pslist.RemoveAt(i);
            }
        }

        if (!enter && GetComponent<BoxCollider>().bounds.Contains(centerPoint.transform.position))
        {
            enter = true;
            centerPoint.GetComponentInParent<Rigidbody>().useGravity = false;
            ParticleSystem nps = Instantiate(waterSplashSys) as ParticleSystem;
            nps.gameObject.SetActive(true);
            nps.transform.position = centerPoint.transform.position;
            pslist.Add(nps);
            FMODUnity.RuntimeManager.PlayOneShot(splash);
            underwaterEvent.start();
        }
        if (enter && !GetComponent<BoxCollider>().bounds.Contains(centerPoint.transform.position))
        {
            enter = false;
            centerPoint.GetComponentInParent<Rigidbody>().useGravity = true;
            ParticleSystem nps = Instantiate(waterSplashSys) as ParticleSystem;
            nps.gameObject.SetActive(true);
            nps.transform.position = centerPoint.transform.position;
            pslist.Add(nps);
            FMODUnity.RuntimeManager.PlayOneShot(splash);
            underwaterEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    private void OnDestroy()
    {
        underwaterEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
