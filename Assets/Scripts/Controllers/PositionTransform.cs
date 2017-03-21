﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTransform : Transformation {

    // Use this for initialization
    public Vector3 position;
    public override Vector3 Apply(Vector3 point)
    {
        return point + position;
    }
    public override Matrix4x4 Matrix
    {
        get
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetRow(0, new Vector4(1f, 0f, 0f, position.x));
            matrix.SetRow(1, new Vector4(0f, 1f, 0f, position.y));
            matrix.SetRow(2, new Vector4(0f, 0f, 1f, position.z));
            matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        }
    }
}
