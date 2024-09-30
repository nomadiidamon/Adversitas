using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;

    [Header("-----Attributes-----")]
    [Range(0, 100)][SerializeField] int speed;
    [Range(0, 100)][SerializeField] int sprintMod;
    [Range(0, 100)][SerializeField] int jumpMax;
    [Range(0, 100)][SerializeField] int jumpSpeed;
    [Range(0, 100)][SerializeField] int gravity;


    Vector3 move;
    Vector3 playerVel;
    int jumpCount;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
