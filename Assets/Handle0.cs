using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Handle0 : MonoBehaviour
{
    public GameObject upper;
    public GameObject interact;
    public GameObject Esc;
    Vector3 delta;
    bool crap = true;
    bool reload = false;

    [FMODUnity.EventRef]
    public string explo;
    [FMODUnity.EventRef]
    public string knob;
    [FMODUnity.EventRef]
    public string hover;

    private void OnMouseEnter()
    {
        interact.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot(hover);
    }

    private void OnMouseExit()
    {
        interact.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!GetComponent<Animation>().isPlaying)
        {
            FMODUnity.RuntimeManager.PlayOneShot(knob);
            GetComponent<Animation>().Play();
            if (crap)
            {
                FMODUnity.RuntimeManager.PlayOneShot(explo);
                upper.GetComponent<Rigidbody>().freezeRotation = false;
                upper.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -1), ForceMode.Impulse);
                crap = false;
            }
            else reload = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        delta = Esc.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Esc.transform.position - delta;
        if (reload)
        {
            if (!GetComponent<Animation>().isPlaying)
            {
                SceneManager.LoadScene(0);
            }
        }
        if (upper.transform.position.y < -500)
        {
            upper.transform.position = new Vector3(900, 900, 900);
            upper.GetComponent<Rigidbody>().useGravity = false;
            upper.GetComponent<Rigidbody>().velocity = new Vector3();
            upper.GetComponent<Rigidbody>().angularVelocity = new Vector3();
        }
    }
}
