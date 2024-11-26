using System;
using UnityEngine;

[SerializeField]
public interface IAIComponent
{
	public string Name { get; set; }
	void PerformRole();
}

[SerializeField]
[Serializable]
public class AIComponent : MonoBehaviour, IAIComponent
{
	public string Name { get; set; }
	public virtual void PerformRole()
	{

	}	   
	
}
