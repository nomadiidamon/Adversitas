using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public class Timer : MonoBehaviour
{
    public bool isDone = false;
    [SerializeField] public Stopwatch stopwatch = new Stopwatch();
    [SerializeField] public float targetTime = 0f;
    [SerializeField] public float elapsedTime = 0f;
    [SerializeField] public float timeRemaining = 0f;

    private void Update()
    {
        if (stopwatch.IsRunning)
        {
            elapsedTime = (float)stopwatch.ElapsedMilliseconds / 1000;
            timeRemaining = targetTime - elapsedTime;

            if (timeRemaining <= 0f)
            {
                stopwatch.Stop();
                stopwatch.Reset();
                isDone = true;
                UnityEngine.Debug.Log("Timer finished");
            }
        }
    }

    public void StartTimer()
    {
        if (!stopwatch.IsRunning)
        {
            stopwatch.Start();
            isDone = false;
            UnityEngine.Debug.Log("Timer started");
        }
    }

    public void StopTimer()
    {
        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();
            UnityEngine.Debug.Log("Timer stopped");
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timeRemaining = targetTime;
        isDone = false;
        stopwatch.Reset();
        UnityEngine.Debug.Log("Timer reset");
    }

    public void SetTargetTime(float newTargetTime)
    {
        targetTime = newTargetTime;
        ResetTimer();
    }
}
