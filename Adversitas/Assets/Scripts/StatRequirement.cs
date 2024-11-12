using System;

public class StatRequirement
{
	int minLevel;
	bool meetsRequirment = false;

	public StatRequirement(Stat stat)
	{
		if (stat != null) { 
			if (stat.m_level.value >= minLevel)
			{
				meetsRequirment = true;
			}
			else
			{
				meetsRequirment = false;
			}

		}
	}
}
