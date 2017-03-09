using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencePointBehaviour : MonoBehaviour {
	public List<ReferencePointBehaviour> neighbours = new List<ReferencePointBehaviour>();
    public int[] indexPointNumber = new int[3];
    int totalNumberOfNeighoursFound = 0;

    public Vector3 targetPosition;
    public float lerpSpeed = 0.2f;

	void Start()
    {
        neighbours = new List<ReferencePointBehaviour>();
    }
	private void AddNeighbour( ReferencePointBehaviour neighbour)
	{
		neighbours.Add(neighbour);
	}
    public void FindNeighbours(ReferencePointBehaviour[,,] grid,int maxRes)
    {
        totalNumberOfNeighoursFound = 0;
        int row = indexPointNumber[0];
        int col = indexPointNumber[1];
        int dep = indexPointNumber[2];
        
        int START_OF_GRID = 0; // This determines search space
        int xStart  = Mathf.Max( row - 1, START_OF_GRID);
        int xFinish = Mathf.Min( row + 1, maxRes - 1);
        int yStart  = Mathf.Max( col - 1, START_OF_GRID);
        int yFinish = Mathf.Min( col + 1, maxRes - 1);
        int zStart = Mathf.Max( dep - 1, START_OF_GRID);
        int zFinish = Mathf.Min( dep + 1, maxRes - 1);

        for ( int curX = xStart; curX <= xFinish; curX++ ) 
        {
            for ( int curY = yStart; curY <= yFinish; curY++ ) 
            {
                for ( int curZ = zStart; curZ <= zFinish; curZ++ ) 
                {
                    if (!(curX==row && curY==col && curZ == dep))
                    {
                        Debug.Log("WHY");
                        AddNeighbour(grid[curX,curY,curZ]);
                        totalNumberOfNeighoursFound++;
                    }
                }   
            }
        }
        Debug.Log(gameObject.name + " has "+totalNumberOfNeighoursFound+" neighbours");
    }

    public void SetIndex (int x,int y,int z)
    {
        indexPointNumber = new int[3] {x,y,z};
        gameObject.name = "Node: " + indexPointNumber[0]+","+indexPointNumber[1]+","+indexPointNumber[2];
    }
	
	
	// Update is called once per frame
	void Update () {
        //transform.position = Vector3.MoveTowards(transform.position,targetPosition,lerpSpeed*Time.deltaTime);
	}
}
