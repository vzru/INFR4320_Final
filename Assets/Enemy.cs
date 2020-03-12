using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int changepath = 0;
    private int wavepointIndex = 0;

    void Start()
    {
        target = Waypoints.points[0];
    }
    void Update()
    {

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime,Space.World);
        changepath++;
        if (Vector3.Distance(transform.position,target.position) <= 0.2f)
        {
            if (changepath % 2 == 0)
            {
               GetNextWayPoint1();
            }
            else
            GetNextWayPoint();
        }
    }
    
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

void GetNextWayPoint1()
{
    if (wavepointIndex >= Waypoints1.points.Length - 1)
    {
        Destroy(gameObject);
        return;

    }
    wavepointIndex++;
    target = Waypoints1.points[wavepointIndex];
}


}


