using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragOrbitQuaternionScript : MonoBehaviour {

    public Quaternion deltaRot;
    private Vector3 delta;
    public Vector3 normalisedDeltaDirection;
    private Vector3 lastPos;

    public Vector3 Delta
    {
        get
        {
            return delta;
        }

        set
        {
            delta = value;
            normalisedDeltaDirection = delta.normalized;
            deltaRot.SetFromToRotation(Vector3.up, normalisedDeltaDirection);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Delta = Input.mousePosition - lastPos;
            lastPos = Input.mousePosition;
        }
    }
}
