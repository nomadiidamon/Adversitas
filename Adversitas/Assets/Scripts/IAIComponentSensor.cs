using UnityEngine;

public interface IAIComponentSensor
{
    void Detect();
    bool IsInRange(Transform target);
}