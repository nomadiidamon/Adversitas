using System;

public class MyAI
{
	Health health;
	Stamina stamina;
	public bool isActive {  get; set; }
	public Transform target { get; set; }
	public float speed { get; set; }
	public int damage { get; set;} 


	public MyAi(Transform target, float speed, int health, int stamina, int damage)
	{
		this.target = target;
		this.speed = speed;
		this.health.m_level.value = health;
		this.stamina.m_level.value = stamina;
		this.damage = damage;
	}
}
