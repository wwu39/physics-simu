﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class esc : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string impact;

    private void OnCollisionEnter(Collision collision)
    {
        FMODUnity.RuntimeManager.PlayOneShot(impact);
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
