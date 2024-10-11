using UnityEngine;

public class LockOnMe : MonoBehaviour//, ILockable
{
    [SerializeField] public Transform lockOnPosition;
    Transform playerTarget;


    // Start is called before the first frame update
    void Start()
    {
        playerTarget = playerManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTarget);
    }

    public void CameraFollowsMe(Camera playerCamera)
    {
        playerCamera.transform.LookAt(lockOnPosition);
    }

}
