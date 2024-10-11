using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform target;

    private void OnEnable()
    {
        if (target == null) {
            target = Camera.main.transform;
            StartCoroutine(LookAtTarget());
        }
    }

    public IEnumerator LookAtTarget()
    {
        while (this.gameObject.activeInHierarchy)
        {
            Vector3 dir = target.position - this.transform.position;
            transform.rotation = Quaternion.LookRotation(dir);
            yield return null;
        }
    }

}
