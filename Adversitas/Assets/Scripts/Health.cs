using System;

public class Health : Stat
{
    Factors factors;
    public int currentHealthPoints { get; set; }
    public int maxHealthPoints { get; set; }

    public Health(string name, int level, int expToNextLvl, float levelRate)
        :base(name, level)
	{
        m_level.m_expToNextLevel = expToNextLvl;
        m_level.m_levelRate = levelRate;

        CalculateInitialHealthPoints();        
    }

    private void CalculateInitialHealthPoints()
    {
        if (m_level.m_value > 1)
        {
            int target = 175 * m_level.m_value;
            currentHealthPoints = target;
            maxHealthPoints = 300 * m_level.m_value;
        }
        else
        {
            currentHealthPoints = 175;
            maxHealthPoints = 300;
        }
    }
}