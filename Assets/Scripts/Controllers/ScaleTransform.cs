using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleTransform : Transformation
{
    private float size;
    public Vector3 scale;
    //TODO shakeOnStagingAreaEvent might need to be shifted to a settings manager in the future.
    public bool shakeOnStagingAreaEvent = true;
    public void Start()
    {
        if (shakeOnStagingAreaEvent)
        {
            RingFactory.onRefreshSetEvent += Shake;
        }
    }
    public float Size
    {
        get
        {
            return size;
        }

        set
        {
            size = value;
            scale = new Vector3(1,1,1) * value;
        }
    }
    public void Shake ()
    {
        transform.DOShakeScale(0.5f, new Vector3(0.2f, 0.2f, 0), 20, 30f);
    }
    public override Vector3 Apply(Vector3 point)
    {
        point.x *= scale.x;
        point.y *= scale.y;
        point.z *= scale.z;
        return point;
    }
    public override Matrix4x4 Matrix
    {
        get
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetRow(0, new Vector4(scale.x, 0f, 0f, 0f));
            matrix.SetRow(1, new Vector4(0f, scale.y, 0f, 0f));
            matrix.SetRow(2, new Vector4(0f, 0f, scale.z, 0f));
            matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        }
    }

   
}

