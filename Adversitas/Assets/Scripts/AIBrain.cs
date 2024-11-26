using System;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

[SaveDuringPlay]
[System.Serializable]
[RequireComponent(typeof(Rigidbody))]


public class AIBrain : MonoBehaviour
{
    [Header("-----Components-----")]
	[SerializeField] public Transform target;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Animator animator;

	[Space(10)]
	[SerializeField] public List<AIStateBase> states = new List<AIStateBase>();
    [Space(10)]
    [SerializeField] public List<AIComponent> aiComponents = new List<AIComponent>();
    

	[Header("-----Factors-----")]
	[SerializeField] public bool isActive;
    [SerializeField] public float speed;
	[SerializeField] public CharacterStats statBlock;
    [SerializeField] public DamageValue damageValue;


    public void Start()
    {
		//AIBrain();
    }

    public void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }
        for (int i = 0; i < aiComponents.Count; i++) {
			aiComponents[i].PerformRole();
		}

    }



}
