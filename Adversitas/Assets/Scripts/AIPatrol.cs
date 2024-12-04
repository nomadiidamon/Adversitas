using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIPatrol : AIComponent, IPatrol, IAIComponent
{
    [SerializeField] public bool isPatrolling { get; set; }
    [SerializeField] public bool atLocation;
    [SerializeField] public List<Waypoint> waypoints = new List<Waypoint>();
    [SerializeField] private Waypoint currentWaypoint;
    [SerializeField] private int index = 0;
    [Range(0,20)][SerializeField] private float waitTime = 0.0f;
    [Range(2.9f, 50)][SerializeField] float stoppingDistance;
    [SerializeField] MoveToDestinationRigidbody mover;
    [SerializeField] Timer patrolTimer;

    void Start()
    {
        mover = GetComponentInParent<MoveToDestinationRigidbody>();
        stoppingDistance = mover.stoppingDistance;
        atLocation = mover.isAtDestination;
        patrolTimer.SetTargetTime(waitTime);
        mover.SetStoppingDistance(stoppingDistance);
        Register();
    }

    void FixedUpdate()
    {
        PerformRole();
    }

    public override void PerformRole()
    {
        Patrol();
    }

    public void Patrol()
    {
        if (waypoints.Count > 0)
        {
            if (currentWaypoint == null)
            {
                currentWaypoint = waypoints[index];
                Debug.Log("No waypoints. First waypoint added");
            }
            else
            {

                if (this.transform.position != currentWaypoint.transform.position) {

                    if (mover != null)
                    {
                        isPatrolling = true;
                        mover.SetDestination(currentWaypoint.transform.position);
                    }   
                    
                }

                if ((currentWaypoint.transform.position - transform.position).magnitude < stoppingDistance)
                {
                    Debug.Log("Arrived at Destination");
                    patrolTimer.stopwatch.Start();

                    if (patrolTimer.isDone)
                    {
                        Debug.Log("Patrol Timer finished");
                        patrolTimer.SetTargetTime(waitTime);
                        index++;
                        if (index >= waypoints.Count)
                        {
                            index = 0;
                        }
                        currentWaypoint = waypoints[index];
                        mover.SetDestination(currentWaypoint.transform.position);
                        isPatrolling = false;
                        patrolTimer.ResetTimer();
                    }
                    else
                    {
                        //
                        mover.SetDestination(transform.position);
                    }

                }

            }



        }
        else
        {
            for (int i = 0; i < IPatrolManager.instance.numberOfWaypoints; i++)
            {
                waypoints.Add(IPatrolManager.instance.waypoints[i]);
            }
        }

    }

    public void Register()
    {
        IPatrolManager.instance.patrols.Add(this);
        IPatrolManager.instance.numberOfPatrols++;
    }

    public void Unregister()
    {

    }

}
