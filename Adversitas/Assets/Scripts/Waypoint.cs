using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Waypoint : MonoBehaviour, IWaypoint
{
    public Transform location { get; }

    void Start()
    {
        Register();
    }

    void Update()
    {

    }

    public void Register()
    {
        IPatrolManager.instance.waypoints.Add(this);
        IPatrolManager.instance.numberOfWaypoints++;
    }

    public void Unregister()
    {

    }

    public Transform ShareSignal()
    {
        //Debug.Log("Broadcasting");
        return location;
    }
}
