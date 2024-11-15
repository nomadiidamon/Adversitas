using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina
{
    public Stat stat;
    public int currentHealthPoints {get; set;}
    public int maxHealthPoints { get; set; }

    public Stamina (string name, int level, int expToNextLvl, float levelRate)
    {
       stat.m_name = name;
       stat.m_level.m_value = level;
       stat.m_level.m_expToNextLevel = expToNextLvl;
       stat.m_level.m_levelRate = levelRate;

        CalculateInitialStaminaPoints();
    }

    private void CalculateInitialStaminaPoints()
    {
        if (stat.m_level.m_value > 1)
        {
            int target = 175 * stat.m_level.m_value;
            currentHealthPoints = target;
            target = 300 * stat.m_level.m_value;
        }
        else
        {
            currentHealthPoints = 175;
            maxHealthPoints = 300;
        }
    }
}
