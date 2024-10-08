using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

[System.Serializable]
public class cameraSettings
{


    [Range(0, 5)] public float lookSpeed = 2f;
    [Range(0, 5)] public float distanceFromPlayer = 5f;
    [Range(0, 5)] public float height = 2f;
    [Range(0, 200)] public float pitchLimit = 80f;
    [Range(0, 5)] public float turnSpeed = 2f;

    public float[] GetValues() {     
        float [] fields = { lookSpeed, distanceFromPlayer, height, pitchLimit, turnSpeed };
        return fields;
    }

    public void SmoothTransition(float[] values, float changeSpeed = 5f)
    {
        lookSpeed = Mathf.Lerp(lookSpeed, values[0], Time.deltaTime * changeSpeed);
        distanceFromPlayer = Mathf.Lerp(distanceFromPlayer, values[0], Time.deltaTime * changeSpeed);
        height = Mathf.Lerp(height, values[0], Time.deltaTime * changeSpeed);
        pitchLimit = Mathf.Lerp(pitchLimit, values[0], Time.deltaTime * changeSpeed);
        turnSpeed = Mathf.Lerp(turnSpeed, values[0], Time.deltaTime * changeSpeed);

    }
}
