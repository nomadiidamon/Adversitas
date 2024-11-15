using System;
using UnityEngine;

public class MyAI
{
	public bool isActive {  get; set; }
	public Transform target { get; set; }
	public float speed { get; set; }
	Stamina stamina;
	Health health;
	public int damage { get; set;} 

	public MyAI() { 
		
	
	}


	public MyAI(Transform target, float speed, int stamina, int health, int damage)
	{
		this.target = target;
		this.speed = speed;
		this.health.stat.m_level.m_value = health;
		this.stamina.stat.m_level.m_value = stamina;
		this.damage = damage;
	}
}
