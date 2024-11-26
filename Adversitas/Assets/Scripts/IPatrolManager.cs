using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IPatrolManager : MonoBehaviour
{
    public static IPatrolManager instance;

    [SerializeField] public List<IPatrol> patrols = new List<IPatrol>();
    public int numberOfPatrols;

    [SerializeField] public List<Waypoint> waypoints = new List<Waypoint>();
    public int numberOfWaypoints;

    void Awake()
    {
        if (instance == null)
            instance = this;

        numberOfPatrols = patrols.Count;
        numberOfWaypoints = waypoints.Count;


    }



    void Update()
    {
        for (int i = 0; i < patrols.Count; i++)
        {
            patrols[i].Patrol();
        }
        for (int i = 0;i < waypoints.Count; i++)
        {
            waypoints[i].ShareSignal();
        }


    }

}
