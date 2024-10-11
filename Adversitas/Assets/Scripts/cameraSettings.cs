using UnityEditor;
using UnityEngine;

[System.Serializable]
public class cameraSettings
{
    [Range(0, 25)] public float lookSpeed = 2f;
    [Range(0, 15)] public float distanceFromPlayer = 5f;
    [Range(0, 15)] public float height = 2f;
    [Range(0, 200)] public float pitchLimit = 80f;
    [Range(0, 25)] public float turnSpeed = 2f;


}
