using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencePointBehaviour : MonoBehaviour {
	public List<ReferencePointBehaviour> neighbours;
    public int[] indexPointNumber = new int[3];

    public Vector3 targetPosition;
    public float lerpSpeed = 0.2f;

	public bool Outer,Middle,Inner;
	
	private void AddNeighbour( ReferencePointBehaviour neighbour)
	{
		neighbours.Add(neighbour);
	}
    public void FindNeighbours(ReferencePointBehaviour[,,] grid,int maxRes)
    {
        int totalNumberOfNeighoursFound = 0;
        for (int horzLevel = -1; horzLevel < 1; horzLevel++)
        {
            for (int vertLevel = -1; vertLevel < 1; vertLevel++)
            {
                for (int depthLevel = -1; depthLevel < 1; depthLevel++)
                {
                    if (horzLevel == 0 && vertLevel == 0 && depthLevel == 0)
                    {
                        break; // You can't be your own neighbour after all.
                    }
                }
            }
        }
    }
    public void SetIndex (int x,int y,int z)
    {
        indexPointNumber = new int[3] {x,y,z};
        gameObject.name = "Node: " + indexPointNumber.ToString();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position,targetPosition,lerpSpeed*Time.deltaTime);
	}
}
