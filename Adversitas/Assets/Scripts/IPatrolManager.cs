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

    [SerializeField] BoxCollider boxCollider;
    [SerializeField] public GameObject defaultWaypoint;

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

        if (RegenerateWaypoints(3))
        {

        }

    }

    public void CreateRandomWaypoint()
    {
        Instantiate(defaultWaypoint, RandomPositionWithinBoxCollider(boxCollider), Quaternion.identity);
    }

    public Vector3 RandomPositionWithinBoxCollider(BoxCollider collider)
    {
        Vector3 center = collider.bounds.center;
        Vector3 size = collider.bounds.size;
        
        float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        float y = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);
        float z = Random.Range(center.z - size.z / 2f, center.z + size.z / 2f);

        return new Vector3(x, y, z);
    } 

    public bool RegenerateWaypoints(int numOfWaypoints)
    {
        if (waypoints.Count != 0) return false; 
        if (waypoints.Count ==  0)
        {
            for(int i = 0; i < numOfWaypoints; i++) { 
                CreateRandomWaypoint();
            }
        }
        return true;
    }


}
