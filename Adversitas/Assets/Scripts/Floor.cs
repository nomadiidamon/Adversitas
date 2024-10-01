using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour, IGravity
{
    [SerializeField] float gravity;
    [SerializeField] SphereCollider impactArea;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyGravity()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (impactArea != null)
        {
            Vector3 effect = new Vector3(collision.transform.position.x, collision.transform.position.y - gravity * Time.deltaTime, collision.transform.position.z);
            collision.transform.position = effect;
        }
    }
}
