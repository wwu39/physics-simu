using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

    static private float arrowScale = 1.5f;

    static public void DrawForce(Vector3 pos, GameObject arrow, Vector3 force, Color color)
    {
        float sizeOfForce = Mathf.Sqrt(force.x * force.x + force.y * force.y);
        float angle = Mathf.Acos(force.y / sizeOfForce);
        float sign = 1.0f;
        if (force.x > 0) sign = -1.0f;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, sign * angle * 180.0f / Mathf.PI); // set rotation
        arrow.transform.position = pos + new Vector3(0f, 0f, -5f); // appear on top of the box
        arrow.transform.localScale = new Vector3(10.0f, arrowScale * sizeOfForce, 10.0f);
        arrow.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
        arrow.transform.GetChild(1).GetComponent<Renderer>().material.color = color;
    }

    static public void DrawForce(Vector3 pos, GameObject arrow, Quaternion dir, float size, Color color)
    {
        arrow.transform.position = pos + new Vector3(0f, 0f, -5f); // appear on top of the box
        arrow.transform.localScale = new Vector3(10.0f, arrowScale * size, 10.0f);
        arrow.transform.rotation = dir;
        arrow.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
        arrow.transform.GetChild(1).GetComponent<Renderer>().material.color = color;
    }

    static public float includedAngle2DCos(Vector3 a, Vector3 b)
    {
        return (a.x * b.x + a.y * b.y) / (a.magnitude * b.magnitude);
    }
}
