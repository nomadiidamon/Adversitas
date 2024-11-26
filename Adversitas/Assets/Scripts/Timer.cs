using System.Diagnostics;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] public Stopwatch stopwatch;
    [SerializeField] public float targetTime = 0;
    [SerializeField] public float elapsedTime = 0;
    [SerializeField] public float timeRemaining = 0;


}
