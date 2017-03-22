using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TransformationGrid : MonoBehaviour {

    public Transform parentTransform;
    public Transform prefab;
    public static TransformationGrid instance;
    public bool changeColorBasedOnResolution;
    int gridResolution = 3;
    List<Transformation> transformations;
    Matrix4x4 transformation;

    Transform[] transformGrid;
    static ReferencePointBehaviour[ , , ] Nodes;
    public static ReferencePointBehaviour[,,] NODES
    {
        get
        {
            return Nodes;
        }
        // NO SETTER!
    }
    public ReferencePointBehaviour GetReferencePointByIndex (int x,int y, int z)
    {
        if ((x>gridResolution-1||x<0)|| (y > gridResolution-1 || y < 0)|| (z > gridResolution-1 || z < 0))
        {
            Debug.Log("OUT OF RANGE");
            return null;
        }
        return NODES[x, y, z];
    }
    public ReferencePointBehaviour GetReferencePointByIndex (Vector3 overloadPara)
    {
        return GetReferencePointByIndex((int)overloadPara.x,(int)overloadPara.y,(int)overloadPara.z);
    }
    #region MonoBehaviour Functions
    void Awake()
    {
        instance = this;
        Nodes = new ReferencePointBehaviour[gridResolution, gridResolution, gridResolution];
        //Debug.Log("Test "+Nodes.Rank);
        //Debug.Log("Total Number of Items " + Nodes.Length);
        transformations = new List<Transformation>();
        transformGrid = new Transform[gridResolution * gridResolution * gridResolution];
        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++, i++)
                {
                    transformGrid[i] = CreateGridPoint(x, y, z);
                }
            }
        }
    }
    void Start()
    {
        if (instance!=this)
        {
            Destroy(this);
        }
        transform.position = Vector3.zero;
    }
    void Update()
    {
        UpdateTransformation();
        GetComponents<Transformation>(transformations);
        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++, i++)
                {
                    transformGrid[i].localPosition = TransformPoint(x, y, z);
                }
            }
        }
    }
    #endregion
    #region Transformation Functions
    void UpdateTransformation()
    {
        GetComponents<Transformation>(transformations);
        if (transformations.Count > 0)
        {
            transformation = transformations[0].Matrix;
            for (int i = 1; i < transformations.Count; i++)
            {
                transformation = transformations[i].Matrix * transformation;
            }
        }
    }
    Transform CreateGridPoint(int x, int y, int z)
    {
        Transform point = Instantiate<Transform>(prefab);
        if (point.GetComponent(typeof(ReferencePointBehaviour)) != null)
        {
            //Debug.Log("J: "+x + y + z);
            //POSTMORTEM The Multidimensonal array conundrum, Last time I had a bug over here that the Nodes always get populated with a 2,2,2 index node, turns out that I was accessing prefab instead of point transform, simple error but big mistake
            ReferencePointBehaviour prefabComponent = point.GetComponent(typeof(ReferencePointBehaviour)) as ReferencePointBehaviour;
            prefabComponent.SetIndex(x, y, z);
            Nodes[x, y, z] = prefabComponent;

            //Debug.Log(Nodes[x,y,z].gameObject.name);
        }
        else
        {
            Debug.Log("PREFAB ATTACHED DOESN'T HAVE REFERENCEPOINTBEHAVIOUR ATTACHED!");
        }
        if (parentTransform)
        {
            point.parent = parentTransform;
        }
        point.localPosition = GetCoordinates(x, y, z);

        if (changeColorBasedOnResolution)
        {
            point.GetComponent<SpriteRenderer>().color = new Color(
                (float)x / gridResolution,
                (float)y / gridResolution,
                (float)z / gridResolution
            );
        }
        return point;
    }
    Vector3 GetCoordinates(int x, int y, int z)
    {
        return new Vector3(
            x - (gridResolution - 1) * 0.5f,
            y - (gridResolution - 1) * 0.5f,
            z - (gridResolution - 1) * 0.5f
        );
    }
    Vector3 TransformPoint(int x, int y, int z)
    {
        Vector3 coordinates = GetCoordinates(x, y, z);
        return transformation.MultiplyPoint(coordinates);
    } 
    #endregion
}
