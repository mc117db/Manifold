using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencePointBehaviour : MonoBehaviour {
    public static List<ReferencePointBehaviour> allNodes = new List<ReferencePointBehaviour>();
    public List<ReferencePointBehaviour> neighbours = new List<ReferencePointBehaviour>();
    public int Xcoor, Ycoor, Zcoor;
    int totalNumberOfNeighoursFound = 0;

    public Vector3 targetPosition;
    public float lerpSpeed = 0.2f;

	void Start()
    {
        if (!allNodes.Contains(this))
        {
            allNodes.Add(this);
        }
        neighbours = new List<ReferencePointBehaviour>();
        FindNeighbours();
    }
	private void AddNeighbour( ReferencePointBehaviour neighbour)
	{
		neighbours.Add(neighbour);
	}

    public void FindNeighbours()
    {
        int maxRes = 3;
        Debug.Log(gameObject.name + " is looking for neighbours");
        Debug.Log(allNodes.Count);
        totalNumberOfNeighoursFound = 0;
        int row = Xcoor;
        int col = Ycoor;
        int dep = Zcoor;
        
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
                    if (curX==row && curY == col && curZ == dep)
                        {
                            break;
                        }
                        foreach (ReferencePointBehaviour repPoint in allNodes)
                        {
                           if (repPoint.Xcoor == curX && repPoint.Ycoor == curY && repPoint.Zcoor == curZ)
                            {
                               AddNeighbour(repPoint);
                               totalNumberOfNeighoursFound++;
                            }
                        }  
                }   
            }
        }

        Debug.Log(gameObject.name + " has "+totalNumberOfNeighoursFound+" neighbours");
    }

    public void SetIndex (int x,int y,int z)
    {
        Xcoor = x;
        Ycoor = y;
        Zcoor = z;
        gameObject.name = "Node: " + Xcoor+","+Ycoor+","+Zcoor;
    }
	
	
	// Update is called once per frame
	void Update () {
        //transform.position = Vector3.MoveTowards(transform.position,targetPosition,lerpSpeed*Time.deltaTime);
	}
}
