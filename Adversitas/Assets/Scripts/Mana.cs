using System;

public class Mana : Stat
{
    public int currentManaPoints { get; set; }
    public int maxManaPoints { get; set; }


    public Mana(string name, int level, int expToNextLevel, float levelRate)
        : base(name, level)
    {
        m_level.m_expToNextLevel = expToNextLevel;
        m_level.m_levelRate = levelRate;

        CalculateInitialManaPoints();
    }

    private void CalculateInitialManaPoints()
    {
        if (m_level.m_value > 1)
        {
            currentManaPoints = 75 * m_level.m_value;  
            maxManaPoints = 120 * m_level.m_value;    
        }
        else
        {
            currentManaPoints = 75;
            maxManaPoints = 120;
        }
    }
}
