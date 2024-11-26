using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaypoint
{
    Transform location { get; }
    public Transform ShareSignal();
    public void Register();
    public void Unregister();
}
