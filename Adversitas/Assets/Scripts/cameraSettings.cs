using UnityEditor;
using UnityEngine;

[System.Serializable]
public class cameraSettings
{
    /// <summary> 
    ///     "--##--Any additional factors should be added to the back of the vector in the 'SmoothTransition' function and in the back of teh GetValues function to avoid range issues"
    /// </summary>

    [Range(0, 25)] public float lookSpeed = 2f;
    [Range(0, 15)] public float distanceFromPlayer = 5f;
    [Range(0, 15)] public float height = 2f;
    [Range(0, 200)] public float pitchLimit = 80f;
    [Range(0, 25)] public float turnSpeed = 2f;

    public float[] GetValues() {     
        float [] fields = { lookSpeed, distanceFromPlayer, height, pitchLimit, turnSpeed };
        /// Possible extra value(s): 
        ///     Individual transition speeds for more finite adjustments
        return fields;
    }

    public void SmoothCameraTransitions(cameraSettings currentCam, cameraSettings futureCam, float changeSpeed = 5f)
    {
        SmoothLookSpeed(currentCam, futureCam, changeSpeed);
        SmoothCameraDistance(currentCam, futureCam, changeSpeed);
        SmoothHeight(currentCam, futureCam, changeSpeed);
        SmoothPitchLimit(currentCam, futureCam, changeSpeed);
        SmoothTurnSpeed(currentCam, futureCam, changeSpeed);
    }

    private static void SmoothCameraDistance(cameraSettings currentCam, cameraSettings futureCam, float changeSpeed = 5f)
    {
        currentCam.distanceFromPlayer = Mathf.Lerp(currentCam.distanceFromPlayer, futureCam.distanceFromPlayer, Time.deltaTime * changeSpeed);
    }
    private static void SmoothLookSpeed(cameraSettings currentCam, cameraSettings futureCam, float changeSpeed = 5f)
    {
        currentCam.lookSpeed = Mathf.Lerp(currentCam.lookSpeed, futureCam.lookSpeed, Time.deltaTime * changeSpeed);
    }
    private static void SmoothHeight(cameraSettings currentCam, cameraSettings futureCam, float changeSpeed = 5f)
    {
        currentCam.height = Mathf.Lerp(currentCam.height, futureCam.height, Time.deltaTime * changeSpeed);
    }
    private static void SmoothPitchLimit(cameraSettings currentCam, cameraSettings futureCam, float changeSpeed = 5f)
    {
        currentCam.pitchLimit = Mathf.Lerp(currentCam.pitchLimit, futureCam.pitchLimit, Time.deltaTime * changeSpeed);
    }
    private static void SmoothTurnSpeed(cameraSettings currentCam, cameraSettings futureCam, float changeSpeed = 5f)
    {
        currentCam.turnSpeed = Mathf.Lerp(currentCam.turnSpeed, futureCam.turnSpeed, Time.deltaTime * changeSpeed);
    }
}
