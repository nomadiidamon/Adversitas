using System;

public class Health
{
    Factors factors;
    public Stat stat;
	public Health(string name, int level, int expToNextLvl, float levelRate)
	{
        stat.m_name = name;
        stat.m_level.m_value = level;
        stat.m_level.m_expToNextLevel = expToNextLvl;
        stat.m_level.m_levelRate = levelRate;
    }
}