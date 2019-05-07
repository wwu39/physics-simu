using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube0 : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject centerPoint;
    public GameObject star;
    GameObject arrow;
    public Vector3 player_force;
    float multiplier = 1f;
    Vector3 centerPos;

    [FMODUnity.EventRef]
    public string impact;
    [FMODUnity.EventRef]
    public string interact;

    private void OnCollisionEnter(Collision collision)
    {
        FMODUnity.RuntimeManager.PlayOneShot(impact);
    }

    // Start is called before the first frame update
    void Start()
    {
        arrow = Instantiate(arrowPrefab) as GameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player_force.magnitude < 15)
        {
            player_force = (star.transform.position - centerPoint.transform.position).normalized;
            multiplier += 0.2f;
            player_force *= multiplier;
            centerPos = centerPoint.transform.position;
            Utils.DrawForce(centerPos, arrow, player_force, Color.red);
        }
        else centerPoint.SetActive(false);
    }
}
