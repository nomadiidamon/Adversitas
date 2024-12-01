using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Timer))]
public class WaypointSpawner : MonoBehaviour
{
    static WaypointSpawner instance;
    [SerializeField] public BoxCollider spawnArea;
    public Timer spawnTimer;


    [SerializeField] public float spawnRate;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
        spawnTimer.SetTargetTime(spawnRate);

    }

    void Update()
    {
        if (!spawnTimer.stopwatch.IsRunning)
        {
            spawnTimer.StartTimer();
        }
        else
        {
            if (spawnTimer.elapsedTime >= spawnTimer.targetTime)
            {
                spawnTimer.isDone = true;
                AddRandomWaypointInBounds(spawnArea.bounds);

            }
        }
    }

    void AddRandomWaypointInBounds(Bounds bounds)
    {
        Waypoint newPoint = new Waypoint();

        newPoint.transform.position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
        Debug.Log("New waypoint added");
    }

    void AddWaypoint(Vector3 newWaypoint)
    {
        Waypoint newPoint = new Waypoint();

    }

}
