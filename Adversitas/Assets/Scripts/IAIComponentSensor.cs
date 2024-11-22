using System;

public interface IAIComponentSensor
{
    void Detect();
    bool IsInRange(Transform target);
}