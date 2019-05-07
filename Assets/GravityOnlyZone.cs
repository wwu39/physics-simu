using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityOnlyZone : MonoBehaviour
{ 
    [FMODUnity.EventRef]
    public string cancel;
    [FMODUnity.EventRef]
    public string entergate;

    public Scrollbar ContinuousForceScrollbar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name=="Cube")
        {
            if (ContinuousForceScrollbar.value != 0) FMODUnity.RuntimeManager.PlayOneShot(cancel);
            FMODUnity.RuntimeManager.PlayOneShot(entergate);
            Cube_lv2.insideGOZone = true;
            if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude < 3)
            other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(3, 0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
