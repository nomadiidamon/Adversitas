using UnityEngine;

public class LockOnMe : MonoBehaviour//, ILockable
{
    [SerializeField] public Transform lockOnPosition;
    [SerializeField] public Sprite lockOnReticle;
    Transform playerTarget;
    //private int distanceFromPlayer;

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

        //Instantiate(lockOnReticle, lockOnPosition.position, playerCamera.transform.rotation);
    }



    public static LockOnMe Closest(LockOnMe first, LockOnMe second, Vector3 distanceToCompare)
    {
        float firstDistance = (distanceToCompare - first.lockOnPosition.position).magnitude;
        float secondDistance = (distanceToCompare - second.lockOnPosition.position).magnitude;

        if (firstDistance < secondDistance)
        {
            return first;
        }
        else
        {
            return second;
        }

    }

}
