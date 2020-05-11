using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public GravityType thisType; // 指定重力类型
    public float GravityAccelerate = 9.8f;
}

public enum GravityType
{
    Static_into = 0, // 固定方向，指向
    Static_away = 1, // 固定方向，离向
    Sphere_into = 2, // 球面，指向球心
    Sphere_away = 3, // 球面，远离球心
    UnShape_into = 4, // 不规则面，指向面
    Unshape_away = 5 // 不规则面，原理面
}