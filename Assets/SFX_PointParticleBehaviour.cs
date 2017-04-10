using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_PointParticleBehaviour : MonoBehaviour {

    Vector3 velocity;
    public float initVelocityMult = 1f;
    public float speed;
    public float lerpDeltaMin,lerpDeltaMax;
    float lerpDelta;
    float lerpVal;
    public Vector3 endPoint;
    public delegate void OnEvent();
    public static event OnEvent OnFinishLerpEvent;
    public event OnEvent OnFinishLerpEventSelf;

    public void Initialize (float intVel,float spd,float deltaMin,float deltaMax,Vector3 endPt)
    {
        initVelocityMult = intVel;
        speed = spd;
        lerpDeltaMin = deltaMin;
        lerpDeltaMax = deltaMax;
        endPoint = endPt;
        Restart();
        OnFinishLerpEventSelf += DestroySelf;
    }
    void Restart()
    {
        velocity = (transform.position - Vector3.zero).normalized * initVelocityMult;
        lerpDelta = Random.Range(lerpDeltaMin, lerpDeltaMax);
        lerpVal = 0;
    }
    private void Update()
    {
        Vector3 endDir = endPoint - transform.position;
        endDir.Normalize();
        lerpVal = Mathf.Clamp01(lerpVal + (lerpDelta * Time.deltaTime));
        velocity = Mathfx.Coserp(velocity, endDir, lerpVal);
        transform.position += velocity * speed * Time.deltaTime;
        if (lerpVal >= 0.5f)
        {
            if (OnFinishLerpEvent != null)
            {
                OnFinishLerpEvent();
            }
            if (OnFinishLerpEventSelf != null)
            {
                OnFinishLerpEventSelf();
            }
        }
    }
    void DestroySelf()
    {
        OnFinishLerpEventSelf -= DestroySelf;
        Destroy(gameObject);
    }
}
