using System;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

[SaveDuringPlay]
[System.Serializable]
[RequireComponent(typeof(Rigidbody))]

public class AIBrain : MonoBehaviour
{
    [Header("Components")]
	[SerializeField] public Transform target;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Animator animator;
	[SerializeField] public List<AIStateBase> states = new List<AIStateBase>();
    [SerializeField] public List<GameObject> aiComponents = new List<GameObject>();

    [Header("Factors")]
	[SerializeField] public bool isActive;
	[SerializeField] public float speed;
    [SerializeField] public Stamina stamina;
    [SerializeField] public Health health;
    [SerializeField] public Mana mana;
    [SerializeField] public DamageValue damageValue;


	public AIBrain(Transform target, float speed, int _stamina, int _health, int _mana, int _damage)
	{
		this.target = target;
		this.speed = speed;
		health = new Health("Health", _health, 100, 1.25f);
		//this.health.m_level.m_value = health;
		stamina = new Stamina("Stamina", _stamina, 100, 1.25f);
		//this.stamina.m_level.m_value = stamina;
		mana = new Mana("Mana", _mana, 100, 1.25f);
		damageValue = new DamageValue(_damage);
		//this.damage = damage;
	}

    public void Start()
    {
		//AIBrain();
    }

    public void AddState(AIStateBase stateToAdd)
	{
		//states.Add(stateToAdd);
	}

	public void AddComponent(IAIComponent componentToAdd)
	{
		//components.Add(componentToAdd);
	}

}
