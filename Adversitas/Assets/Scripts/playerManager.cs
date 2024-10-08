using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    public static playerManager instance;
    public Transform player;


    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player").transform;
    }

}
