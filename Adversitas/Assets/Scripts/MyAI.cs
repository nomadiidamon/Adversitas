using System;
using System.Collections.Generic;
using UnityEngine;

public class MyAI
{
	public bool isActive {  get; set; }
	public Transform target { get; set; }
	public float speed { get; set; }
	public Rigidbody rb;
	public Animator animator;
	public List<AIStateBase> states;
	public List<IMyAIComponent> components;
	public Stamina stamina;
	public Health health;
	public Mana mana;
	public DamageValue damageValue;
	public int damage { get; set;} 


	public MyAI(Transform target, float speed, int stamina, int health, int damage)
	{
		this.target = target;
		this.speed = speed;
		this.health.m_level.m_value = health;
		this.stamina.m_level.m_value = stamina;
		this.damage = damage;
	}

	public void AddState(AIStateBase stateToAdd)
	{
		states.Add(stateToAdd);
	}

	public void AddComponent(IMyAIComponent componentToAdd)
	{
		states.Add(componentToAdd);
	}

}
