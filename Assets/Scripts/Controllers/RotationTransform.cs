using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTransform : Transformation {

    public Vector3 rotation;
    public Vector2 rotationSensitivity = new Vector2 (0.1f,0.2f);
    [Header("Animation")]
    public bool autoRotateWhenNotDragging;
    public float rotSpeed;
    public float rotRecoverySpeed = 2f;
    private float rotLerpVal;
    private Vector3 rotVector;

    void Start ()
    {
        DragOrbitTouchScript.mouseMovementDeltaChange += Rotate;
        rotVector = Vector3.one * rotSpeed;
    }
    void Update()
    {
        if (autoRotateWhenNotDragging)
        {

            if(!Input.GetMouseButton(0))
            {
                rotLerpVal = Mathf.Lerp(rotLerpVal, 1, Time.deltaTime * rotRecoverySpeed);
            }
            else
            {
                rotLerpVal = Mathf.Lerp(rotLerpVal, 0, Time.deltaTime * rotRecoverySpeed);
            }
            rotVector = Vector3.Lerp(Vector3.zero, Vector3.one * rotSpeed, rotLerpVal);
            rotation += Time.deltaTime * rotVector;
            WrapEulerRotation(rotation);
        }
    }
    private void Rotate (Vector2 delta)
    {
        if (delta.magnitude > 100)
        {
            return;
        }
        //Debug.Log(delta);
        rotation.y += rotationSensitivity.x * delta.x;
        rotation.x += rotationSensitivity.y * delta.y;
        rotation = WrapEulerRotation(rotation);
    }
    Vector3 WrapEulerRotation (Vector3 rotationToWrap)
    {
        Vector3 newRot = rotationToWrap;
        newRot.x %= 360;
        newRot.y %= 360;
        newRot.z %= 360;
        return newRot;
    }
    // Transformation Grid will call Apply()
    public override Vector3 Apply(Vector3 point)
    {
        float radX = rotation.x * Mathf.Deg2Rad;
        float radY = rotation.y * Mathf.Deg2Rad;
        float radZ = rotation.z * Mathf.Deg2Rad;

        float sinX = Mathf.Sin(radX);
        float cosX = Mathf.Cos(radX);

        float sinY = Mathf.Sin(radY);
        float cosY = Mathf.Cos(radY);

        float sinZ = Mathf.Sin(radZ);
        float cosZ = Mathf.Cos(radZ);

        Vector3 xAxis = new Vector3(
            cosY * cosZ,
            cosX * sinZ + sinX * sinY * cosZ,
            sinX * sinZ - cosX * sinY * cosZ
        );
        Vector3 yAxis = new Vector3(
            -cosY * sinZ,
            cosX * cosZ - sinX * sinY * sinZ,
            sinX * cosZ + cosX * sinY * sinZ
        );
        Vector3 zAxis = new Vector3(
            sinY,
            -sinX * cosY,
            cosX * cosY
        );

        return xAxis * point.x + yAxis * point.y + zAxis * point.z;
    }

    public override Matrix4x4 Matrix
    {
        get
        {
            float radX = rotation.x * Mathf.Deg2Rad;
            float radY = rotation.y * Mathf.Deg2Rad;
            float radZ = rotation.z * Mathf.Deg2Rad;
            float sinX = Mathf.Sin(radX);
            float cosX = Mathf.Cos(radX);
            float sinY = Mathf.Sin(radY);
            float cosY = Mathf.Cos(radY);
            float sinZ = Mathf.Sin(radZ);
            float cosZ = Mathf.Cos(radZ);

            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetColumn(0, new Vector4(
                cosY * cosZ,
                cosX * sinZ + sinX * sinY * cosZ,
                sinX * sinZ - cosX * sinY * cosZ,
                0f
            ));
            matrix.SetColumn(1, new Vector4(
                -cosY * sinZ,
                cosX * cosZ - sinX * sinY * sinZ,
                sinX * cosZ + cosX * sinY * sinZ,
                0f
            ));
            matrix.SetColumn(2, new Vector4(
                sinY,
                -sinX * cosY,
                cosX * cosY,
                0f
            ));
            matrix.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        }
    }
}
