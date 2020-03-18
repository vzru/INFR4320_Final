//Regan Tran 100622360
//Mathooshan Thevakumaran 100553777
//Victor Zhang 100421055

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int changepath = 0;
    private int wavepointIndex = 0;
    private static int destroyed;
    public Text killedtext;
    private GameObject destruction;
  
    // When mouse clicked , destory the enemy  
    void OnMouseDown()
    {
       
        if ((Waypoints.points[0] || Waypoints.points[1]) || (Waypoints.points[2] || Waypoints.points[3]))
        {
            Destroy(gameObject);
            // destroyed++;
        }
    }
    
    //start point for new enemy
    void Start()
    {
        target = Waypoints.points[0];
    }

    //based on waypoint , translate to new waypoint
    void Update()
    {

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime,Space.World);
        changepath++;
        if (Vector3.Distance(transform.position,target.position) <= 0.2f)
        {
      
            GetNextWayPoint();
        }
        
    }
    
    //find next waypoint
    void GetNextWayPoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject);
            return;

        }
        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }



}


