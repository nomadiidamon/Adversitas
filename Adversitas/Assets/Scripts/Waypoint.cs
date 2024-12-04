using UnityEngine;

[System.Serializable]
public class Waypoint : MonoBehaviour, IWaypoint, IDie
{
    public Transform location { get; }
    private BoxCollider m_collider;
    [SerializeField] private float lifespan;
    [SerializeField] bool isTouchingMe = false;


    [SerializeField] float timeTouched = 0f;


    void Start()
    {
        m_collider = GetComponent<BoxCollider>();
        Register();
    }

    void Update()
    {
        if (isTouchingMe)
        {
            timeTouched += Time.deltaTime;
        }
        if (timeTouched >= lifespan)
        {
            Die();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Test"))
        {
           isTouchingMe=true;
        }

        if (m_collider.bounds.Contains(other.transform.position)) {
            isTouchingMe = true;
        }

    }


    public void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Test"))
        {
            isTouchingMe = false;
        }

        if (!m_collider.bounds.Contains(other.transform.position)) {
            isTouchingMe=false;
        }

    }

    public void Register()
    {
        IPatrolManager.instance.waypoints.Add(this);
        IPatrolManager.instance.numberOfWaypoints++;
    }

    public void Unregister()
    {
        IPatrolManager.instance.waypoints.Remove(this);
        Destroy(gameObject);
    }

    public Transform ShareSignal()
    {
        return location;
    }

    public void Die()
    {
        Unregister();
    }
}
