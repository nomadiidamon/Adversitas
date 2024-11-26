using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrol
{
    bool isPatrolling { get; set; }
    public void Patrol();
    public void Register();
    public void Unregister();
}
