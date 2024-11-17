using System;

public class StatRequirement
{
	int minLevel;
	bool meetsRequirment = false;

	public StatRequirement(Stat stat, int minLevel)
	{
		this.minLevel = minLevel;

		if (stat != null) { 

			meetsRequirment = stat.m_level.m_value >= minLevel;
		}
	}
}
