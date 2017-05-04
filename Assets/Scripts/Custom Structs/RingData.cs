using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum SpawnType {Normal,Anomaly,ForceDrop}
 public class RingData {
    public bool Outer,Middle,Inner;
    public List<ColorIndex> ringColors;
    public SpawnType spawnType = SpawnType.Normal;
 }
