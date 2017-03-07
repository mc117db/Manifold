using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencePointBehaviour : MonoBehaviour {
	List<ReferencePointBehaviour> neighbours;
    public Vector3 targetPosition;
    public float lerpSpeed = 0.2f;

	public bool Outer,Middle,Inner;
	
	public void AddNeighbour( ReferencePointBehaviour neighbour)
	{
		neighbours.Add(neighbour);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position,targetPosition,lerpSpeed*Time.deltaTime);
	}
}
