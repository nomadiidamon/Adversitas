using System;
using UnityEngine;

[System.Serializable]
public class StatRequirement
{
    [SerializeField] int minLevel;
    [SerializeField] bool meetsRequirment = false;

	public StatRequirement(Stat stat, int minLevel)
	{
		this.minLevel = minLevel;

		if (stat != null) { 

			meetsRequirment = stat.m_value >= minLevel;
		}
	}
}
