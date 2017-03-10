using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationGrid : MonoBehaviour {

    public Transform parentTransform;
    public Transform prefab;
    public bool changeColorBasedOnResolution;
    public int gridResolution = 3;
    List<Transformation> transformations;
    Matrix4x4 transformation;

    Transform[] transformGrid;
    ReferencePointBehaviour[ , , ] Nodes;

    void Awake()
    {
        Nodes = new ReferencePointBehaviour[gridResolution, gridResolution, gridResolution];
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
        if (prefab.GetComponent(typeof (ReferencePointBehaviour)) != null)
        {
            ReferencePointBehaviour prefabComponent = prefab.GetComponent(typeof(ReferencePointBehaviour)) as ReferencePointBehaviour;
            prefabComponent.SetIndex(x, y, z);
            Nodes[x,y,z] = prefabComponent;
            Debug.Log(Nodes[x,y,z].gameObject.name);
        }
        else
        {
            Debug.Log("PREFAB ATTACHED DOESN'T HAVE REFERENCEPOINTBEHAVIOUR ATTACHED!");
        }
        if(parentTransform)
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
}
