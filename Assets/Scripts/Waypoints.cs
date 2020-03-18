//Regan Tran 100622360
//Mathooshan Thevakumaran 100553777
//Victor Zhang 100421055

using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
//set pathway based on waypoints
    void Awake()
    {
        points = new Transform[transform.childCount];
        for (int i =0; i< points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }

    }
}
