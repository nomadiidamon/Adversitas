using System;

public class StatRequirement
{
	int minLevel;
	bool meetsRequirment = false;

	public StatRequirement(Stat stat)
	{
		if (stat != null) { 
			if (stat.m_level.m_value >= minLevel)
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
