using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReferencePointBehaviour : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public List<ReferencePointBehaviour> neighbours = new List<ReferencePointBehaviour>();
    public int Xcoor, Ycoor, Zcoor;
    int totalNumberOfNeighoursFound = 0;

    public Vector3 targetPosition;
    public float lerpSpeed = 0.2f;

	void Start()
    {
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
        //Debug.Log(gameObject.name + " is looking for neighbours");
        //Debug.Log(allNodes.Count);
        totalNumberOfNeighoursFound = 0;
        int row = Xcoor;
        int col = Ycoor;
        int dep = Zcoor;
        
        int START_OF_GRID = 0; // This determines search space
        int xStart  = Mathf.Max( row - 1, START_OF_GRID); // Prevent searching outside of 0
        int xFinish = Mathf.Min( row + 1, maxRes - 1); // Prevent searching outside of the max resolution
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
                    if (!(curX==row && curY == col && curZ == dep))
                        {
                        // Dont add itself
                        AddNeighbour(TransformationGrid.NODES[curX, curY, curZ]);
                        totalNumberOfNeighoursFound++;
                    }
                    
                }   
            }
        }

        //Debug.Log(gameObject.name + " has "+totalNumberOfNeighoursFound+" neighbours");
    }
    public void SetIndex (int x,int y,int z)
    {
        Xcoor = x;
        Ycoor = y;
        Zcoor = z;
        gameObject.name = "Node: " + Xcoor+","+Ycoor+","+Zcoor;
    }
	public void CheckNeighboursForColor(List<ColorIndex> colorcheck)
    {
        int neighbourHasColor = 0;
        for (int i = 0; i < colorcheck.Count; i++)
        {
            //Debug.Log("CHECKING FOR: "+colorcheck[i].ToString());
            if (colorcheck[i] != ColorIndex.NONE)
            {
                foreach (ReferencePointBehaviour neighbour in neighbours)
                {
                    if (neighbour.GetComponent<RingPointManager>().HaveColor(colorcheck[i]))
                    {
                        neighbourHasColor++;
                        //TODO Work on game logic behaviour
                        Vector3 backwardsVector = new Vector3(Xcoor, Ycoor, Zcoor) - new Vector3(neighbour.Xcoor, neighbour.Ycoor, neighbour.Zcoor);
                        //Vector3 indexToFind = 
                        // DO BACKWARD CHECK - YES (ADD MATCH DATA TO GAME CONTROLLER)
                        // IF NOT IN THE MIDDLE, TELL FOUND NEIGHBOUR TO DO BACKWARD CHECK
                    }
                }
            }
        }
        // CHECKS HAS BEEN DONE - CLEAR ALL MARKED RINGS OFF GAME CONTROLLERS
        Debug.Log("NEIGHBOURS HAS " + neighbourHasColor + " SIMILAR COLORS TIERS");
    }
	// Update is called once per frame
	void Update () {
        //transform.position = Vector3.MoveTowards(transform.position,targetPosition,lerpSpeed*Time.deltaTime);
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (RingDragBehaviour.dragging)
        {
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        return;
        throw new NotImplementedException();
    }
}
