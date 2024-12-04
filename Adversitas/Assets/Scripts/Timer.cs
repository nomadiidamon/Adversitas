using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public class Timer : MonoBehaviour
{
    public bool hasStarted = false;
    public bool isDone = false;
    public bool isPaused = false;
    [SerializeField] public Stopwatch stopwatch = new Stopwatch();
    [SerializeField] public float targetTime = 0f;
    [SerializeField] public float elapsedTime = 0f;
    [SerializeField] public float timeRemaining = 0f;
    private float pauseTime = 0.0f;
    private void Update()
    {
        if (stopwatch.IsRunning && !isPaused)
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
        if (!stopwatch.IsRunning && !isPaused)
        {
            stopwatch.Start();
            hasStarted = true;
            isDone = false;
            UnityEngine.Debug.Log("Timer started");
        }
    }

    public void StopTimer()
    {
        if (stopwatch.IsRunning || isPaused)
        {
            stopwatch.Stop();
            UnityEngine.Debug.Log("Timer stopped");
        }
    }

    public void PauseTimer()
    {
        if (!isPaused && stopwatch.IsRunning)
        {
            stopwatch.Stop();
            pauseTime = elapsedTime;
            isPaused = true;
            UnityEngine.Debug.Log("Timer paused");
        }
    }

    public void ResumeTimer()
    {
        if (isPaused)
        {
            stopwatch.Start();
            isPaused = false;
            UnityEngine.Debug.Log("Timer resumed");
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timeRemaining = targetTime;
        isDone = false;
        isPaused = false;
        hasStarted = false;
        stopwatch.Reset();
        UnityEngine.Debug.Log("Timer reset");
    }

    public void SetTargetTime(float newTargetTime)
    {
        targetTime = newTargetTime;
        ResetTimer();
    }
}
