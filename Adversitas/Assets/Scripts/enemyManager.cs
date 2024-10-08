using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    public static enemyManager instance;
    public Transform playerPosition;


    void Awake()
    {
        instance = this;
        playerPosition = playerManager.instance.player.transform;
    }

}
