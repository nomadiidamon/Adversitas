using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina
{
    public Stat stat;

    public Stamina (string name, int level, int expToNextLvl, float levelRate)
    {
       stat.m_name = name;
       stat.m_level.m_value = level;
       stat.m_level.m_expToNextLevel = expToNextLvl;
       stat.m_level.m_levelRate = levelRate;
    }
}
