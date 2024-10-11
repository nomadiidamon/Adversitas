using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    [SerializeField] Canvas enemyCanvas;
    [SerializeField] Image lockOnReticle;
    private lockOnController player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<lockOnController>();
    }

    void Update()
    {
        if (player.lockOnTarget != null)
        {
            enemyCanvas.enabled = true;
            lockOnReticle.enabled = true;
            enemyCanvas.transform.localScale = Vector3.one * ((player.playerCamera.transform.position - player.lockOnTarget.position).magnitude * (lockOnReticle.transform.localScale.y / 2));
            lockOnReticle.transform.position = player.lockOnTarget.transform.position;
            lockOnReticle.transform.LookAt(player.playerCamera.transform);
            
        }
        else
        {
            enemyCanvas.enabled = false;
            lockOnReticle.enabled = false;
        }
    }
}
